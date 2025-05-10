// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/

open System

// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/#parsing-a-hard-coded-character

/// parsec stage: single 'A' char
let parseA (str:string) (bool*string)=
    if String.IsNullOrEmpty(str) then
        (false, "")
    else if str.[0] = 'A' then
        let remaining = str.[1..]
        (true, remaining)
    else
        (false, str)
// parseA "" -> (false, "")
// parseA "ASD" -> (true, "SD")
// parseA "SD" -> (false, "SD")

// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/#parsing-a-specified-character

let pchar (charToMatch,str) =
  if String.IsNullOrEmpty(str) then
    let msg = "No more input"
    (msg,"")
  else
    let first = str.[0]
    if first = charToMatch then
      let remaining = str.[1..]
      let msg = sprintf "Found %c" charToMatch
      (msg,remaining)
    else
      let msg = sprintf "Expecting '%c'. Got '%c'" charToMatch first
      (msg,str)

// pchar('A',"ABC") -> ("Found A", "BC")
// pchar('A', "BC") -> ("Expecting 'A'. Got 'B'", "BC")

// https://fsharpforfunandprofit.com/posts/understanding-parser-combinators/#returning-a-successfailure

type ParseResult<'a> =
  | Success of 'a
  | Failure of string
