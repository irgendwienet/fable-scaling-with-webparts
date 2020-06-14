module Login

open Elmish
open Fable.React.Props
open Fable.React

open GlobalTypes
open WebPart

type Model = LoginModel

type Page =
    | Login

type Msg =
    | LoginAsAdmin
    | GlobalMsg of GlobalMsg

let init (page:Page) (lastModel:Model option) =
    LoginModel, Cmd.none

let update msg model =
    match msg with
    | LoginAsAdmin ->
        (model, (LoggedIn "Admin") |> GlobalMsg |> Cmd.ofMsg )

    | GlobalMsg _ ->
        (model, Cmd.none) // will not be handled her but in MainDispatch

let view model dispatch (navigateTo:AnyPage -> unit) =
    div [  ] [
        makeButton (fun _ -> dispatch LoginAsAdmin) "Login as Admin"
    ]

let WebPart = {
    Init = init
    Update = update
    View = view

    GetGlobalMsg =
        function
        | GlobalMsg m -> Some m
        | _ -> None

    GetHeader = fun _ -> "Login"

    BuildUrl = fun _ -> [ "login" ]
    ParseUrl =
        function
        | [ "login" ] -> Some Login
        | _ -> None
}