module Loader

open Elmish
open Fable.React.Props
open Fable.React

open GlobalTypes
open WebPart

type Model =
    | Initial
    | Loading
    | Loaded of string

type Page =
    | Loader

type Msg =
    | StartLoading
    | LoadedData  of string
    | Reset

let init (page:Page) (lastModel:Model option) =
    Model.Initial, Cmd.none

let update msg model =
    match msg with
    | StartLoading ->
        let nextModel = Model.Loading
        let nextCmd = Cmd.afterTimeout 1000 (LoadedData  "Data is loaded")
        nextModel, nextCmd

    | LoadedData data ->
        let nextModel = Model.Loaded data
        nextModel, Cmd.none

    | Reset ->
        Model.Initial, Cmd.none

let spinner =
    div [  ] [
        span [ ] [
            i [ ClassName "fa fa-circle-notch fa-spin fa-2x" ] [ ]
        ]
    ]

let view model dispatch (navigateTo:AnyPage -> unit) =
    match model with
    | Model.Initial ->
        makeButton (fun _ -> dispatch StartLoading) "Start Loading"
    | Model.Loading ->
        spinner
    | Model.Loaded data ->
        div [ ]
            [ h3 [ ] [ str data ]
              makeButton (fun _ -> dispatch Reset) "Reset" ]

let WebPart = {
    Init = init
    Update = update
    View = view

    GetGlobalMsg = fun _ -> None

    GetHeader = fun _ -> "Loading Data"

    BuildUrl = fun _ -> [ "loader" ]
    ParseUrl =
        function
        | [ "loader" ] -> Some Loader
        | _ -> None
}