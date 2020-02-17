module WebPartRegistry

open WebPart

let private AllWebParts =
    [ Counter.WebPart.Calls
      Loader.WebPart.Calls
      Settings.WebPart.Calls ]

let WebParts =
    {
        TryUpdate = fun msg state -> AllWebParts |> List.tryPick (fun x -> x.TryUpdate msg state)
        TryRender = fun state dispatch -> AllWebParts |> List.tryPick (fun x -> x.TryRender state dispatch)
    }