﻿module Template

open FSharp.Core
open Fable.React
open Fable.React.Props
open GlobalTypes

let pageNotFound =
    div [] [ p [] [str "Sorry - not found."] ]

let divider =
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]

let caption model =
    model
    |> WebPartRegistry.AllParts.TryGetHeader
    |> Option.defaultValue "Demo"

let main model dispatch children =

    let navItem nextPage title =
        let notActive = model.Page <> nextPage
        let navLinkClass = if notActive then "nav-link" else "nav-link active"
        li [ ClassName "nav-item" ] [
            a [ ClassName navLinkClass
                OnClick (fun _ -> dispatch (NavigateTo nextPage)) ]
              [ str title ]
        ]

    div [ Style [ Padding 20 ] ] [
        h1 [ ] [ model |> caption |> str ]
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem Counter.Page.Counter "Counter"
            navItem Loader.Page.Loader "Loader"
            navItem Settings.Page.Settings "Settings"
        ]

        divider

        children
    ]