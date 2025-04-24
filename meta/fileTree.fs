// build project file tree

open System
open System.IO

// create empty file
let touch (name: string) = //
    if not (File.Exists name) then
        File.Create(name) |> ignore

// create empty dir with .gitignore marker
let mkdir (name: string) =
    Directory.CreateDirectory(name) |> ignore
    touch ($"{name}/.gitignore")

// generic C/C++ project
let _dirs =
    [ "."
      ".vscode"
      "bin"
      "doc"
      "lib"
      "inc"
      "src"
      "tmp"
      "ref"
      "mk"
      "cmake" ]

let dirs =
    for d in _dirs do
        mkdir d

        match d with
        | "bin"
        | "tmp"
        | "ref" -> File.WriteAllText($"{d}/.gitignore", "*\n")
        | "doc" -> File.WriteAllText($"{d}/.gitignore", "html/\n")
        | _ -> ()


let _files =
    [ "Makefile"
      "README.md"
      "LICENSE"
      ".clang-format"
      ".prettierc"
      "apt.Debian" ]


let apt = //
    File.WriteAllText(
        "apt.Debian",
        "git make curl
code meld doxygen clang-format
g++ cmake gdb flex bison libreadline-dev"
    )

let _mk =
    [ //
      "var"
      "version"
      "dirs"
      "tool"
      "src"
      "fsh"
      "install" ]

let mk =
    mkdir "mk"

    File.WriteAllLines(
        "Makefile",
        [ for m in _mk do
              $"include mk/{m}.mk" ],
        Text.Encoding.UTF8
    )

    for m in _mk do
        touch $"mk/{m}.mk"

let doc = //
    for d in [ "doc"; "doc/C"; "doc/F" ] do
        mkdir d

    File.WriteAllText("doc/.gitignore", "html/\n")

let files =
    for f in _files do
        touch f

    apt
    mk
    doc

[<EntryPoint>]
let main (args: string[]) =
    dirs
    files
    0
