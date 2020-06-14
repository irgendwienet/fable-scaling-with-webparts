module GlobalTypes

type AnyPage = obj
type AnyWebPartModel = obj
type AnyWebPartMsg = obj

type Model = {
    Page : AnyPage
    PageModel : AnyWebPartModel

    CurrentUser : string option
}

type GlobalMsg =
    | LoggedIn of string
    | LoggedOut

type Msg =
    | WebPartMsg of AnyWebPartMsg
    | UrlUpdated of AnyPage
    | NavigateTo of AnyPage
    | GlobalMsg of GlobalMsg
