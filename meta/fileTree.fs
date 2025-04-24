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

    File.WriteAllLines( //
        "Makefile",
        [ for m in _mk -> $"include mk/{m}.mk" ],
        Text.Encoding.UTF8
    )

    for m in _mk do
        touch $"mk/{m}.mk"

let doc = //
    for d in [ "doc"; "doc/C"; "doc/F" ] do
        mkdir d

    File.WriteAllText("doc/.gitignore", "html/\n")

let _cmake =
    [ //
      "version"
      "src"
      "syntax"
      "any_toolchain" ]

let _target =
    [ //
      "x86_64-gnu-linux"
      "i686-w64-mingw32.cmake"
      "arm-none-eabi"
      "xtensa-lx106-elf.cmake" ]

let cmake = //

    mkdir "cmake"

    for c in _cmake do
        touch $"cmake/{c}.cmake"

    for t in _target do
        touch $"cmake/{t}.cmake"

    File.WriteAllText( //
        "CMakeLists.txt",
        "cmake_minimum_required(VERSION 3.22)
get_filename_component(CMAKE_PROJECT_NAME ${CMAKE_SOURCE_DIR} NAME_WE)
project(${CMAKE_PROJECT_NAME} LANGUAGES C CXX ASM)
"
    )

    File.WriteAllText( //
        "CMakePresets.json",
        $$"""{
        "version": 6,
        "buildPresets": [
            {
                "name"            :  "linux",
                "configurePreset" :  "linux",
                "targets"         : ["all","install"]
            }
        ],
        "configurePresets": [
        {
            "name"            : "common",
            "hidden"          :  true,
            "binaryDir"       : "${sourceDir}/tmp/${presetName}",
            "generator"       : "Unix Makefiles",
            "cacheVariables"  : {
                "CMAKE_INSTALL_PREFIX"    : "${sourceDir}/bin",
                "CMAKE_MODULE_PATH"       : "${sourceDir}/cmake",
                "CMAKE_COLOR_DIAGNOSTICS" :  false,
                "CMAKE_BUILD_TYPE"        : "Debug",
                "CMAKE_VERBOSE_MAKEFILE"  :  false
            }
        },
        {
            "name"            : "pc",
            "inherits"        : "common",
            "hidden"          :  true,
            "cacheVariables"  : {"HW":"pc", "CPU":"i5", "ARCH":"x86_64"}
        },
        {
            "name"            : "linux",
            "inherits"        : "pc",
            "toolchainFile"   : "${sourceDir}/cmake/x86_64-linux-gnu.cmake",
            "cacheVariables"  : {"OS":"linux"}
        }
        ]
    }
    """
    )

let _hw = [ "pc"; "f429disco"; "esp8266" ]
let _cpu = [ "i5"; "stm32f429zi"; "lx106" ]
let _arch = [ "x86_64"; "cortexM"; "cortexM4"; "xtensa" ]

let cross = //
    for m in [ "hw"; "cpu"; "arch"; "os" ] do
        mkdir m
        mkdir $"{m}/inc"
        mkdir $"{m}/src"

let files =
    for f in _files do
        touch f

    apt
    doc
    mk
    cmake
    cross

[<EntryPoint>]
let main (args: string[]) =
    dirs
    files
    0
