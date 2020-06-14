module GlobalTypes

type AnyPage = obj
type AnyWebPartModel = obj
type AnyWebPartMsg = obj

type Model = {
    Page : AnyPage
    PageModel : AnyWebPartModel
}

type GlobalMsg =
    | CounterSettingChanged of int

type Msg =
    | WebPartMsg of AnyWebPartMsg
    | UrlUpdated of AnyPage
    | NavigateTo of AnyPage
    | GlobalMsg of GlobalMsg
