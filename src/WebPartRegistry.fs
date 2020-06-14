module WebPartRegistry

open WebPart

let private UserWebParts =
    [ Counter.WebPart.Functions
      Loader.WebPart.Functions ] |> Merge

let private AdminWebParts =
    [ Settings.WebPart.Functions ] |> Merge

let AllParts =
    [ UserWebParts
      AdminWebParts
      Login.WebPart.Functions ] |> Merge
