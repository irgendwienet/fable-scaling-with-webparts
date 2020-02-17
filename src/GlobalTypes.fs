module GlobalTypes

type Page =
    | Counter
    | Loader
    | Settings

type PageState =
    | CounterState of Counter.State
    | LoaderState of Loader.State
    | SettingsState of Settings.State

type State = {
    CurrentPage : Page
    CurrentPageState : PageState
}

type Message =
    | CounterMsg of Counter.Msg
    | LoaderMsg of Loader.Msg
    | SettingMsg of Settings.Msg
    | NavigateTo of Page


