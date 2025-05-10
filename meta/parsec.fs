// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/

open System

/// parsec stage: single 'A' char
let parseA (str:string) (bool*string)=
    if String.IsNullOrEmpty(str) then
        (false, "")
    else if str.[0] = 'A' then
        let remaining = str.[1..]
        (true, remaining)
    else
        (false, str)
// parseA ""

type ParseResult<'a> =
    | Success of 'a
    | Failure of string

/// parse given char
let pchar (c: char) (str: string) =
    if String.IsNullOrEmpty(str) then
        Failure "No more input"
    else
        let first = str.[0]

        if first = c then
            let remaining = str.[1..]
            Success(c, remaining)
        else
            let msg = sprintf "Expecting '%c'. Got '%c'" c first
            Failure msg
