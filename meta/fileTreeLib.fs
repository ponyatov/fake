module fileTreeLib

open cross

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

let _cross m g =
    mkdir $"{g}/{m}"

    if g <> "." then
        touch $"{g}/{m}/{m}.mk"
        touch $"{g}/{m}/{m}.cmake"

    if g = "hw" then
        touch $"hw/{m}/{m}.gdb"
        touch $"hw/{m}/{m}.ocd"

        if List.contains m _hw_cortex then
            File.CreateSymbolicLink( //
                $"hw/{m}/patch.mk",
                "../../mk/patch.mk"
            )
            |> ignore

    mkdir $"{g}/{m}/inc"
    mkdir $"{g}/{m}/src"

    let d =
        match g with
        | "." -> "cross"
        | _ -> g

    write ( //
        $"{g}/{m}/inc/{m}.hpp",
        $"/// @defgroup {m} {m}\n/// @ingroup {d}\n"
    )

    write ( //
        $"{g}/{m}/src/{m}.cpp",
        $"#include \"{m}.hpp\"\n"
    )
