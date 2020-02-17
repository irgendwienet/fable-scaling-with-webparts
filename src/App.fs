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
        let model = { CurrentPage = page; CurrentPageState = (CounterState submodel)}
        let cmd  = Cmd.map CounterMsg submsg
        model, cmd

    | Loader ->
        let (submodel, submsg) = Loader.init()
        let model = { CurrentPage = page; CurrentPageState = (LoaderState submodel)}
        let cmd  = Cmd.map LoaderMsg submsg
        model, cmd

    | Settings ->
        let (submodel, submsg) = Settings.init()
        let model = { CurrentPage = page; CurrentPageState = (SettingsState submodel)}
        let cmd  = Cmd.map SettingMsg submsg
        model, cmd

let update msg prevState =
    match msg, prevState.CurrentPageState with
    | CounterMsg counterMsg, CounterState prevCounterState ->
        let nextCounterState, nextCounterCmd = Counter.update counterMsg prevCounterState
        let nextState = { prevState with CurrentPageState = (CounterState nextCounterState) }
        nextState, Cmd.map CounterMsg nextCounterCmd

    | LoaderMsg loaderMsg, LoaderState prevLoaderState ->
        let nextLoaderState, nextLoaderCmd = Loader.update loaderMsg prevLoaderState
        let nextState = { prevState with CurrentPageState = (LoaderState nextLoaderState) }
        nextState, Cmd.map LoaderMsg nextLoaderCmd

    | SettingMsg settingMsg, SettingsState prevSettingState ->
        let nextSettingState, nextSettingCmd = Settings.update settingMsg prevSettingState
        let nextState = { prevState with CurrentPageState = (SettingsState nextSettingState) }
        nextState, Cmd.map SettingMsg nextSettingCmd

    | NavigateTo page, _ ->
        if prevState.CurrentPage = page then
            prevState, Cmd.none
        else
            init page

    | _, _ ->
        // TODO log
        prevState, Cmd.none

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

    let currentPage =
        match state.CurrentPageState with
        | CounterState s -> Counter.view s (CounterMsg >> dispatch)
        | LoaderState s -> Loader.view s (LoaderMsg >> dispatch)
        | SettingsState s -> Settings.view s (SettingMsg >> dispatch)

    div [ Style [ Padding 20 ] ] [
        h1 [ ] [ str "Lonely Siblings :(" ]
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem Page.Counter "Counter"
            navItem Page.Loader "Loader"
            navItem Page.Settings "Settings"
        ]

        divider
        currentPage
    ]


Program.mkProgram (fun () -> init Counter) update render
|> Program.withReact "root"
|> Program.run