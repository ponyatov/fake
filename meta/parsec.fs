// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/

open System

/// input chars/tokens stream
/// (treat string as a simplest case)
type istream = string

/// parser state shoud be passed across parsec stages
type pstate = (bool * istream)

/// parsec stage: single 'A' char
let parseA (str: istream) : pstate =
    match str with
    | str when String.IsNullOrEmpty(str) -> //
        (false, "")
    | str when str.[0] = 'A' -> //
        let remaining = str.[1..]
        (true, remaining)
    | _ -> //
        (false, str)

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
