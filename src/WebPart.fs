module WebPart

open GlobalTypes
open Elmish
open Fable.Import.React

type AllWebpartFunctions =
    {
        TryUpdate : AnyPageMsg -> State -> (State * Cmd<Message>) option
        TryRender : State -> (Message -> unit) -> ReactElement option
    }

type WebPart<'WebPartMessage, 'WebPartModel, 'WebPartPage> =
    {
        Update : 'WebPartMessage -> 'WebPartModel -> ('WebPartModel * Cmd<'WebPartMessage>)
        View : 'WebPartModel -> ('WebPartMessage -> unit) -> ReactElement
    }
    with
        member inline webpart.TryUpdate (msg:AnyPageMsg) (globalmodel:State) =
            if (globalmodel.CurrentPageState :? 'WebPartModel) && (msg :? 'WebPartMessage) then
                let pageModel = globalmodel.CurrentPageState :?> 'WebPartModel
                let pageMsg = msg :?> 'WebPartMessage

                let nextPageModel, nextCmd = webpart.Update pageMsg pageModel
                let nextGlobalModel = { globalmodel with CurrentPageState = nextPageModel }
                let nextGlobalCmd = nextCmd |> Cmd.map (fun x -> (WebPartMsg (x :> AnyPageMsg)))

                Some (nextGlobalModel, nextGlobalCmd)
            else
                None

        member inline webpart.TryRender (model:State) dispatch =
            if (model.CurrentPageState :? 'WebPartModel) then
                let pagemodel = model.CurrentPageState :?> 'WebPartModel
                Some (webpart.View pagemodel (fun msg -> dispatch (WebPartMsg (msg :> AnyPageMsg)) ) )
            else
                None

        member inline webpart.Calls =
            {
                TryRender = webpart.TryRender
                TryUpdate = webpart.TryUpdate
            }

