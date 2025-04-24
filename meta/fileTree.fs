// build project file tree

open System
open System.IO

// create empty file
let touch (name: string) = //
    File.WriteAllText(name, "")

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

let files =
    for f in _files do
        touch f

    apt

[<EntryPoint>]
let main (args: string[]) =
    dirs
    files
    0
