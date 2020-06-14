module WebPart

open GlobalTypes
open Elmish
open Fable.React

// Eigentlich hätte ich gerne ein interface IWebPart, welches von WebPart implementiert wird
// aber weil wir auch inline nutzen müssen geht das am Interface nicht
// Dieses Record ist quasi unter Interface ersatz
type AllWebpartFunctions = {
    TryInit : AnyPage -> Model -> (AnyWebPartMsg * Cmd<Msg>) option
    TryUpdate : AnyWebPartModel -> Model -> (Model * Cmd<Msg>) option
    TryView : Model -> (Msg -> unit) -> (AnyPage -> unit) -> ReactElement option

    TryGetHeader : Model -> string option

    TryGetGlobalMsg : AnyWebPartMsg -> GlobalMsg option

    TryParseUrl : string list -> AnyPage option
    TryBuildUrl : AnyPage -> string list option
}

type WebPart<'WebPartMessage, 'WebPartModel, 'WebPartPage> =
    {
        Init : 'WebPartPage -> 'WebPartModel option -> ('WebPartModel * Cmd<'WebPartMessage>)
        Update : 'WebPartMessage -> 'WebPartModel -> ('WebPartModel * Cmd<'WebPartMessage>)
        View : 'WebPartModel -> ('WebPartMessage -> unit) -> (AnyPage -> unit) -> ReactElement

        GetHeader : 'WebPartModel -> string

        ParseUrl : string list -> 'WebPartPage option
        BuildUrl : 'WebPartPage -> string list

        GetGlobalMsg : 'WebPartMessage -> GlobalMsg option
    }
    with
        member inline webpart.TryInit (page:AnyPage) (model:Model) =
            if (page :? 'WebPartPage) then
                let oldModel =  if model.PageModel :? 'WebPartModel then
                                    Some (model.PageModel :?> 'WebPartModel)
                                else
                                    None

                let nextPageModel, nextCmd = webpart.Init (page :?> 'WebPartPage) oldModel
                let nextGlobalCmd = nextCmd |> Cmd.map (fun x -> (WebPartMsg (x :> AnyWebPartMsg)))

                Some (nextPageModel :> AnyWebPartModel, nextGlobalCmd)
            else
                None

        member inline webpart.TryView (model:Model) dispatch navigateTo =
            if (model.PageModel :? 'WebPartModel) then
                let pagemodel = model.PageModel :?> 'WebPartModel
                Some (webpart.View pagemodel (fun msg -> dispatch (WebPartMsg (msg :> AnyWebPartMsg)) ) navigateTo )
            else
                None

        member inline webpart.TryUpdate (msg:AnyWebPartMsg) (globalmodel:Model) =
            if (globalmodel.PageModel :? 'WebPartModel) && (msg :? 'WebPartMessage) then
                let pageModel = globalmodel.PageModel :?> 'WebPartModel
                let pageMsg = msg :?> 'WebPartMessage

                let nextPageModel, nextCmd = webpart.Update pageMsg pageModel
                let nextGlobalModel = { globalmodel with PageModel = nextPageModel }
                let nextGlobalCmd = nextCmd |> Cmd.map (fun x -> (WebPartMsg (x :> AnyWebPartMsg)))

                Some (nextGlobalModel, nextGlobalCmd)
            else
                None

        member inline webpart.TryGetGlobalMsg (msg:AnyWebPartMsg) =
            if (msg :? 'WebPartMessage) then
                webpart.GetGlobalMsg (msg :?> 'WebPartMessage)
            else
                None

        member inline webpart.TryParseUrl segments =
            segments
            |> webpart.ParseUrl
            |> Option.map (fun p -> p :> AnyPage)

        member inline webpart.TryBuildUrl (page:AnyPage) =
            if (page :? 'WebPartPage) then
                Some (webpart.BuildUrl (page :?> 'WebPartPage))
            else
                None

        member inline webpart.TryGetHeader (model:Model) =
            if model.PageModel :? 'WebPartModel then
                let model = model.PageModel :?> 'WebPartModel
                Some (webpart.GetHeader model)
            else
                None

        member inline webpart.Functions =
            { TryInit = webpart.TryInit
              TryUpdate = webpart.TryUpdate
              TryView = webpart.TryView
              TryGetHeader = webpart.TryGetHeader
              TryGetGlobalMsg = webpart.TryGetGlobalMsg
              TryParseUrl = webpart.TryParseUrl
              TryBuildUrl = webpart.TryBuildUrl
            }

let Merge (webParts:AllWebpartFunctions list) =
       {
           TryInit = fun page model -> webParts |> List.tryPick (fun x -> x.TryInit page model)
           TryView = fun model dispatch navigateTo -> webParts |> List.tryPick (fun x -> x.TryView model dispatch navigateTo)
           TryUpdate = fun msg model -> webParts |> List.tryPick (fun x -> x.TryUpdate msg model)
           TryGetGlobalMsg = fun msg -> webParts |> List.tryPick (fun x -> x.TryGetGlobalMsg msg)
           TryParseUrl = fun segments -> webParts |> List.tryPick (fun x -> x.TryParseUrl segments)
           TryBuildUrl = fun page -> webParts |> List.tryPick (fun x -> x.TryBuildUrl page)
           TryGetHeader = fun model -> webParts |> List.tryPick (fun x -> x.TryGetHeader model)
       }