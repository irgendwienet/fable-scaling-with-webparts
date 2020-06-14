[<RequireQualifiedAccess>]
module Urls

open System
open Browser

[<Literal>]
let navigationEvent = "NavigationEvent"

let combine xs = List.fold (sprintf "%s/%s") "" xs
let hashPrefix (s:string) = sprintf "#%s" (s.TrimStart('/'))

let newUrl (newUrl : string) : Elmish.Cmd<_> =
    [ fun _ ->
        history.pushState ((), "", newUrl)

        let ev = CustomEvent.Create (navigationEvent)
        ev.initEvent (navigationEvent, true, true)
        window.dispatchEvent ev |> ignore ]

let parseUrl (urlHash:string) =
    let segments =
        if urlHash.StartsWith "#" then urlHash.Substring(1, urlHash.Length - 1) // remove the hash sign
        else urlHash
        |> fun hash -> hash.Split '/' // split the url segments
        |> List.ofArray
        |> List.filter (String.IsNullOrWhiteSpace >> not)

    WebPartRegistry.AllParts.TryParseUrl segments

