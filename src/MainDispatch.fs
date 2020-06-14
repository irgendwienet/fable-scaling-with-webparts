module MainDispatch

open Elmish

open GlobalTypes
open Browser

let pageHash (page:AnyPage) =
    WebPartRegistry.AllParts.TryBuildUrl page
    |> Option.map Urls.combine
    |> Option.defaultValue ""



let handleUpdateUrl (nextPage:AnyPage) model =
    match (WebPartRegistry.AllParts.TryInit nextPage model) with
    | Some (pageModel, cmd) ->
        let newModel = { model with Page = nextPage
                                    PageModel = pageModel }
        (newModel, cmd)
    | None ->
        (model, Cmd.none)

let appStart() =
    let initialUrl = window.location.hash

    let initalPage = match Urls.parseUrl initialUrl with
                     | Some page -> page
                     | _ -> Counter.Page.Counter :> AnyPage // default page for unknown urls

    let model = { Page = null
                  PageModel = null }

    handleUpdateUrl initalPage model

let handleGlobalMsg msg model =
    (model, Cmd.none)

let renderPage model dispatch =
    let navigateTo = (fun page -> dispatch (NavigateTo page))

    WebPartRegistry.AllParts.TryView model dispatch navigateTo
    |> Option.defaultWith (fun () -> Template.pageNotFound)


let update (msg:Msg) (model:Model) =
    match msg with

    | GlobalMsg msg -> handleGlobalMsg msg model

    | WebPartMsg msg ->
        let globalmsg = WebPartRegistry.AllParts.TryGetGlobalMsg msg

        match globalmsg with
        | Some m -> handleGlobalMsg m model
        | None ->
            let next = WebPartRegistry.AllParts.TryUpdate msg model

            match next with
            | Some(nextModel, nextCmd) -> (nextModel, nextCmd)
            | None -> (model, Cmd.none)

    // generic and navigation msgs
    | NavigateTo page ->
        let nextUrl = Urls.hashPrefix (pageHash page)
        (model, Urls.newUrl nextUrl)
    | UrlUpdated page ->
        handleUpdateUrl page model

let view (model : Model) (dispatch : Msg -> unit) =
    Template.main
        model
        dispatch
        (renderPage model dispatch)
