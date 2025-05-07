// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/

open System

type stream = (bool * string)

let parseA (str: string) : stream =
    match str with
    | str when String.IsNullOrEmpty(str) -> //
        (false, "")
    | str when str.[0] = 'A' -> //
        let remaining = str.[1..]
        (true, remaining)
    | _ -> //
        (false, str)
