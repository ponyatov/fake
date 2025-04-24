// build project file tree

open System
open System.IO
open System.Text.RegularExpressions

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
      "x86_64-linux-gnu"
      "i686-w64-mingw32.cmake"
      "arm-none-eabi"
      "xtensa-lx106-elf.cmake" ]

let x86_64_linux_gnu = //
    File.WriteAllText(
        "cmake/x86_64-linux-gnu.cmake",
        "\
set(CMAKE_SYSTEM_NAME       Linux)
set(CMAKE_SYSTEM_PROCESSOR  x86_64)
set(TOOLCHAIN_PREFIX        ${ARCH}-${OS}-gnu)
set(CMAKE_EXECUTABLE_SUFFIX \"\")

include(any_toolchain)

add_compile_definitions()
add_compile_options()
add_link_options()
"
    )

let any_toolchain = //
    File.WriteAllText(
        "cmake/any_toolchain.cmake",
        "\
set(CMAKE_C_STANDARD   17)
set(CMAKE_CXX_STANDARD 17)

set(CMAKE_C_COMPILER_FORCED   TRUE)
set(CMAKE_CXX_COMPILER_FORCED TRUE)
set(CMAKE_C_COMPILER_ID       GNU)
set(CMAKE_CXX_COMPILER_ID     GNU)

set(CMAKE_C_COMPILER   ${TOOLCHAIN_PREFIX}-gcc)
set(CMAKE_ASM_COMPILER ${CMAKE_C_COMPILER})
set(CMAKE_CXX_COMPILER ${TOOLCHAIN_PREFIX}-g++)
set(CMAKE_LINKER       ${CMAKE_C_COMPILER})
set(CMAKE_OBJCOPY      ${TOOLCHAIN_PREFIX}-objcopy)
set(CMAKE_SIZE         ${TOOLCHAIN_PREFIX}-size)
set(CMAKE_RC_COMPILER  ${TOOLCHAIN_PREFIX}-windres)

include(  os/${OS}/${OS}.cmake    )
include(arch/${ARCH}/${ARCH}.cmake)
include( cpu/${CPU}/${CPU}.cmake  )
include(  hw/${HW}/${HW}.cmake    )

string(TOUPPER ${HW}   HW_  )
string(TOUPPER ${CPU}  CPU_ )
string(TOUPPER ${ARCH} ARCH_)
string(TOUPPER ${OS}   OS_  )

add_compile_options(
    $<$<CONFIG:Debug>:-DDEBUG>
)

add_compile_definitions(
    ${HW_} ${CPU_} ${ARCH_} ${OS_}
)
add_link_options(
    -Wl,--print-memory-usage
)

if(CMAKE_BUILD_TYPE MATCHES Debug)
    add_compile_options(-O0 -g3)
endif()
if(CMAKE_BUILD_TYPE MATCHES Release)
    add_compile_options(-Os -g0)
endif()

set(CMAKE_EXECUTABLE_SUFFIX_ASM ${CMAKE_EXECUTABLE_SUFFIX})
set(CMAKE_EXECUTABLE_SUFFIX_C   ${CMAKE_EXECUTABLE_SUFFIX})
set(CMAKE_EXECUTABLE_SUFFIX_CXX ${CMAKE_EXECUTABLE_SUFFIX})
"
    )

let version = //

    File.WriteAllText(
        "cmake/version.cmake",
        "\
execute_process(
    OUTPUT_VARIABLE REL
    COMMAND git rev-parse --short=4 HEAD
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
    OUTPUT_STRIP_TRAILING_WHITESPACE
)

execute_process(
    OUTPUT_VARIABLE BRANCH
    COMMAND git rev-parse --abbrev-ref HEAD
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
    OUTPUT_STRIP_TRAILING_WHITESPACE
)

execute_process(
    OUTPUT_VARIABLE NOW
    COMMAND date +%y%m%d # _%H%M
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
    OUTPUT_STRIP_TRAILING_WHITESPACE
)

set(BIN_OUTPUT_NAME \"${CMAKE_PROJECT_NAME}_${HW}_${BRANCH}_${REL}_${NOW}${CMAKE_EXECUTABLE_SUFFIX_C}\")
"
    )

let linux_cpp = //
    File.WriteAllText(
        "os/linux/src/linux.cpp",
        "\
#include \"os.hpp\"
#include \"linux.hpp\"

int main(int argc, char *argv[]) {  //
    arg(0, argv[0]);
    for (int i = 1; i < argc; i++) {  //
        arg(i, argv[i]);
        yyfile = argv[i];
        assert(yyin = fopen(yyfile, \"r\"));
        fclose(yyin);
        yyfile = nullptr;
    }
    return 0;
}

void arg(int argc, char *argv) {  //
    fprintf(stderr, \"arg[%i] = <%s>\\n\", argc, argv);
}

char *yyfile = nullptr;
FILE *yyin = nullptr;
"
    )


let linux = //
    linux_cpp


    File.WriteAllText(
        "os/linux/inc/linux.hpp",
        "\
/// @defgroup linux linux
/// @ingroup os

/// @defgroup libc libc
/// @{
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
/// @}

/// @defgroup main main
/// @{
extern int main(int argc, char *argv[]);
extern void arg(int argc, char *argv);
/// @}

/// @defgroup skelex skelex
/// @{
extern char *yyfile;
extern FILE *yyin;
/// @}
"
    )


let src = //
    linux

    File.WriteAllText(
        "cmake/src.cmake",
        "\
file(GLOB LD
    RELATIVE ${CMAKE_SOURCE_DIR}
    hw/${HW}/*.ld
)

file(GLOB S
    RELATIVE ${CMAKE_SOURCE_DIR}
    hw/${HW}/*.s
)

file(GLOB C
    RELATIVE ${CMAKE_SOURCE_DIR}
    src/*.c*
    # cross
      hw/src/*.c*   hw/${HW}/src/*.c*
     cpu/src/*.c*  cpu/${CPU}/src/*.c*
    arch/src/*.c* arch/${ARCH}/src/*.c*
      os/src/*.c*   os/${OS}/src/*.c*
)

file(GLOB H
    RELATIVE ${CMAKE_SOURCE_DIR}
    inc/*.h*
    # cross
      hw/inc/*.h*   hw/${HW}/inc/*.h*
     cpu/inc/*.h*  cpu/${CPU}/inc/*.h*
    arch/inc/*.h* arch/${ARCH}/inc/*.h*
      os/inc/*.h*   os/${OS}/inc/*.h*
)

file(GLOB INC
    RELATIVE ${CMAKE_SOURCE_DIR}
    ${CMAKE_BINARY_DIR}
    inc
    # cross
      hw/inc   hw/${HW}/inc
     cpu/inc  cpu/${CPU}/inc
    arch/inc arch/${ARCH}/inc
      os/inc   os/${OS}/inc
)
include_directories(${INC})
"
    )


let cmake = //

    mkdir "cmake"

    for c in _cmake do
        touch $"cmake/{c}.cmake"

    for t in _target do
        touch $"cmake/{t}.cmake"
        x86_64_linux_gnu
        any_toolchain

    version
    src

    File.WriteAllText( //
        "CMakeLists.txt",
        "\
cmake_minimum_required(VERSION 3.22)
get_filename_component(CMAKE_PROJECT_NAME ${CMAKE_SOURCE_DIR} NAME_WE)
project(${CMAKE_PROJECT_NAME} LANGUAGES C CXX ASM)

include(version)
include(src)
include(syntax)

message(\"-- |\")
message(\"-- | toolchain: \" ${CMAKE_CXX_COMPILER} \" @ \" ${CMAKE_TOOLCHAIN_FILE})
message(\"-- |      host: \" ${CMAKE_HOST_SYSTEM_NAME}-${CMAKE_HOST_SYSTEM_VERSION})
message(\"-- |    target: \" \"hw:\" ${HW} \" cpu:\" ${CPU} \" arch:\" ${ARCH} \" os:\" ${OS})
message(\"-- |   startup: \" \"${S}\")
message(\"-- |    binary: \" ${CMAKE_INSTALL_PREFIX}/${BIN_OUTPUT_NAME}${CMAKE_EXECUTABLE_SUFFIX})
message(\"-- |\")

message(\"-- LD: ${LD}\")
message(\"--  S: ${S} \")
message(\"--  C: ${C} \")
message(\"--  H: ${H} \")

add_executable(${CMAKE_PROJECT_NAME}
    ${C}  ${H}  # C/C++ source
    ${S}  ${LD} # embedded/lowlevel
    ${CP} ${CP} # parsers
)

# target_link_libraries(${CMAKE_PROJECT_NAME} -static)

# target install
set_target_properties(${CMAKE_PROJECT_NAME}
    PROPERTIES OUTPUT_NAME ${BIN_OUTPUT_NAME}${CMAKE_EXECUTABLE_SUFFIX})
install(TARGETS ${CMAKE_PROJECT_NAME}
    DESTINATION ${CMAKE_INSTALL_PREFIX})
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


let _cross m g =
    mkdir $"{g}/{m}"

    match g with
    | "." -> ()
    | _ ->
        touch $"{g}/{m}/{m}.mk"
        touch $"{g}/{m}/{m}.cmake"

    mkdir $"{g}/{m}/inc"
    mkdir $"{g}/{m}/src"

    let d =
        match g with
        | "." -> "cross"
        | _ -> g

    File.WriteAllText( //
        $"{g}/{m}/inc/{m}.hpp",
        $"/// @defgroup {m} {m}\n/// @ingroup {d}\n"
    )

    File.WriteAllText( //
        $"{g}/{m}/src/{m}.cpp",
        $"#include \"{m}.hpp\"\n"
    )

let hw = //
    for hw in [ "pc"; "f429disco"; "esp8266" ] do
        _cross hw "hw"

let cpu = //

    for cpu in [ "i5"; "stm32f429zi"; "lx106" ] do
        _cross cpu "cpu"

let arch = //

    for arch in [ "x86_64"; "cortexM"; "cortexM0"; "cortexM3"; "cortexM4"; "xtensa" ] do
        _cross arch "arch"

        if Regex.IsMatch(arch, "cortexM[0-9]") then
            File.WriteAllText( //
                $"arch/{arch}/{arch}.mk",
                "include arch/cortexM.mk\n"
            )

let os = //

    for os in [ "bare"; "linux"; "win32"; "rtos" ] do
        _cross os "os"

let cross = //

    for m in [ "hw"; "cpu"; "arch"; "os" ] do
        _cross m "."

    hw
    cpu
    arch
    os

let extensions = //
    touch $".vscode/extensions.json"

let settings = //
    touch $".vscode/settings.json"

let tasks = //
    touch $".vscode/tasks.json"

let launch = //
    touch $".vscode/launch.json"

let c_cpp_properties = //
    touch $".vscode/c_cpp_properties.json"

let vscode = //
    mkdir ".vscode"
    extensions
    settings
    tasks
    launch
    c_cpp_properties

let files =
    for f in _files do
        touch f

    apt
    doc
    mk
    cmake
    cross
    vscode

[<EntryPoint>]
let main (args: string[]) =
    dirs
    files
    0
