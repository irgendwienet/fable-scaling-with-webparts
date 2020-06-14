module Settings

open Elmish
open Fable.React.Props
open Fable.React

open WebPart
open GlobalTypes

type Model = SelectedCountFactor of int

type Page =
    | Settings

type Msg =
    | ChangeFactor of int

let init (page:Page) (lastModel:Model option) =
    SelectedCountFactor 1, Cmd.none

let update msg model =
    match msg with
    | ChangeFactor nextFactor ->
        let nextModel = SelectedCountFactor nextFactor
        nextModel, Cmd.none


let view model dispatch (navigateTo:AnyPage -> unit) =
    let (SelectedCountFactor currentFactor) = model
    let factorBtn n =
      let buttonClass =
        if n = currentFactor
        then "btn btn-success"
        else "btn btn-default"
      button [ OnClick (fun _ -> dispatch (ChangeFactor n))
               Style [ Margin 10 ]
               ClassName buttonClass ]
             [ ofInt n ]

    div [ ] [
        factorBtn 1
        factorBtn 5
        factorBtn 10
    ]

let WebPart = {
    Init = init
    Update = update
    View = view

    GetGlobalMsg = fun _ -> None

    GetHeader = fun _ -> "Settings"

    BuildUrl = fun _ -> [ "settings" ]
    ParseUrl =
        function
        | [ "settings" ] -> Some Settings
        | _ -> None
}