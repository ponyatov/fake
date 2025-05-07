module Torben.REPL

open System
open FSharp.Text.Lexing
open Torben.Lexer
open Torben.Parser

let parse (input: string) =
    let lexbuf = LexBuffer<char>.FromString input
    let output = Torben.Parser.syntax Torben.Lexer.tokenize lexbuf
    string output

let repl =
    while true do
        printf "fake> "
        let input = Console.ReadLine()

        try
            let result = parse input
            printfn "%s" result
        with ex ->
            printfn "%s" (ex.ToString())

    0

[<EntryPoint>]
let main argv =

    printfn "Press Ctrl+c to Exit"
    repl
