module GlobalTypes

type AnyPageState = obj
type AnyPageMsg = obj

type Page =
    | Counter
    | Loader
    | Settings

type State = {
    CurrentPage : Page
    CurrentPageState : AnyPageState
}

type Message =
    | WebPartMsg of AnyPageMsg
    | NavigateTo of Page


