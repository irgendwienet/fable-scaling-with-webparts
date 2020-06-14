module App

open FSharp.Core
open Elmish
open Elmish.React
open Browser
open MainDispatch

let urlSubscription _ : Cmd<_> =
    [ fun dispatch ->
        let onChange _ =
            match Urls.parseUrl window.location.hash with
            | None -> ()
            | Some page -> dispatch (GlobalTypes.Msg.UrlUpdated page)

        // listen to manual hash changes or page refresh
        window.addEventListener ("hashchange", unbox onChange)

        // listen to custom navigation events published by `Urls.navigate [ . . .  ]`
        window.addEventListener (Urls.navigationEvent, unbox onChange) ]

Program.mkProgram appStart update view
|> Program.withSubscription urlSubscription
|> Program.withReactSynchronous "elmish-app"
|> Program.run