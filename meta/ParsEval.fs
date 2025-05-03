open System
open FSharp.Text.Lexing
open Lexer
open Parser

let evaluate (input: string) =
    let lexbuf = LexBuffer<char>.FromString input
    let output = Parser.syntax Lexer.tokenize lexbuf
    string output

[<EntryPoint>]
let main argv =

    printfn "Press Ctrl+c to Exit"

    while true do
        printf "fake> "
        let input = Console.ReadLine()

        try
            let result = evaluate input
            printfn "%s" result
        with ex ->
            printfn "%s" (ex.ToString())

    0 // return an integer exit code
