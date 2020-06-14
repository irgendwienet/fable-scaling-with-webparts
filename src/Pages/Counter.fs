module Counter

open Elmish
open Fable.React.Props
open Fable.React

open GlobalTypes
open WebPart

type Model = {
    Count : int
    Factor : int
}

type Page =
    | Counter

type Msg =
    | Increment
    | Decrement
    | IncrementDelayed

let init (page:Page) (lastModel:Model option) =
    { Count = 0; Factor = 1 }, Cmd.none

let update msg model =
    match msg with
    | Increment -> { model with Count = model.Count + model.Factor }, Cmd.none
    | Decrement -> { model with Count = model.Count - model.Factor }, Cmd.none
    | IncrementDelayed ->
        let nextCmd = Cmd.afterTimeout 1000 Increment
        model, nextCmd

let view model dispatch (navigateTo:AnyPage -> unit) =
    div [  ] [
        makeButton (fun _ -> dispatch Increment) "Increment"
        makeButton (fun _ -> dispatch Decrement) "Decrement"
        makeButton (fun _ -> dispatch IncrementDelayed) "Increment Delayed"

        br [ ]
        h1 [ Style [ MarginLeft 150 ] ] [ ofInt model.Count ]
    ]

let WebPart = {
    Init = init
    Update = update
    View = view

    GetGlobalMsg = fun _ -> None

    GetHeader = fun _ -> "Counter"

    BuildUrl = fun _ -> [ "counter" ]
    ParseUrl =
        function
        | [ "counter" ] -> Some Counter
        | _ -> None
}