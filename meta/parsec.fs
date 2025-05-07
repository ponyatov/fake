// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/

open System

let parseA (str: string) =
    if String.IsNullOrEmpty(str) then
        (false, "")
    else if str.[0] = 'A' then
        let remaining = str.[1..]
        (true, remaining)
    else
        (false, str)
