module fileTreeLib

// open System
open System.IO
// open System.Text.RegularExpressions

// create empty file
let touch (name: string) = //
    if not (File.Exists name) then
        File.Create(name) |> ignore

// create empty dir with .gitignore marker
let mkdir (name: string) =
    Directory.CreateDirectory(name) |> ignore
    touch ($"{name}/.gitignore")

let write (name: string, text: string) = //
    File.WriteAllText(name, text)
