module App

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open GlobalTypes

let init (page:Page) =
    match page with
    | Counter ->
        let (submodel, submsg) = Counter.init()
        let model = { CurrentPage = page; CurrentPageState = submodel}
        let cmd  = Cmd.map WebPartMsg submsg
        model, cmd

    | Loader ->
        let (submodel, submsg) = Loader.init()
        let model = { CurrentPage = page; CurrentPageState = submodel}
        let cmd  = Cmd.map WebPartMsg submsg
        model, cmd

    | Settings ->
        let (submodel, submsg) = Settings.init()
        let model = { CurrentPage = page; CurrentPageState = submodel}
        let cmd  = Cmd.map WebPartMsg submsg
        model, cmd

let update msg prevState =
    match msg with
    | WebPartMsg msg ->
        let next = WebPartRegistry.WebParts.TryUpdate msg prevState

        match next with
        | Some (nextModel, nextCmd) -> nextModel, nextCmd
        | None -> prevState, Cmd.none

    | NavigateTo page ->
        if prevState.CurrentPage = page then
            prevState, Cmd.none
        else
            init page

let divider =
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]

let render state dispatch =

    let navItem nextPage title =
        let notActive = state.CurrentPage <> nextPage
        let navLinkClass = if notActive then "nav-link" else "nav-link active"
        li [ ClassName "nav-item" ] [
            a [ ClassName navLinkClass
                Href "#"
                OnClick (fun _ -> dispatch (NavigateTo nextPage)) ]
              [ str title ]
        ]

    let pageView =
        let subView = WebPartRegistry.WebParts.TryRender state dispatch
        match subView with
        | Some view -> view
        | None -> div [][ str "empty" ]

    div [ Style [ Padding 20 ] ] [
        h1 [ ] [ str "Lonely Siblings :(" ]
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem Page.Counter "Counter"
            navItem Page.Loader "Loader"
            navItem Page.Settings "Settings"
        ]

        divider
        pageView
    ]


Program.mkProgram (fun () -> init Counter) update render
|> Program.withReact "root"
|> Program.run