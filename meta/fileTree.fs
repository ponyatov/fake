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

let write (name: string, text: string) = //
    File.WriteAllText(name, text)

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

let dirs () =
    for d in _dirs do
        mkdir d

        match d with
        | "bin"
        | "tmp"
        | "ref" -> write ($"{d}/.gitignore", "*\n")
        | "doc" -> write ($"{d}/.gitignore", "html/\n")
        | _ -> ()


let _files =
    [ "Makefile"
      "README.md"
      "LICENSE"
      ".clang-format"
      ".prettierc"
      "apt.Debian" ]

let apt () = //
    write (
        "apt.Debian",
        "\
git make curl
code meld doxygen clang-format
g++ cmake gdb flex bison libreadline-dev ragel lemon
"
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

let patch () = //
    write (
        "mk/patch.mk",
        "\
#!/usr/bin/make -f

PATCH = $(wildcard *.patch               )
FILES = $(patsubst %.patch,%    ,$(PATCH))
FIXES = $(patsubst %.patch,%.fix,$(PATCH))

.PHONY: all
all:
\tdos2unix $(FILES)
\t$(MAKE) -f $(MAKEFILE_LIST) $(FIXES)
%.fix: %
\tpatch -u $< $<.patch && touch $@
"
    )

let mk () =
    mkdir "mk"

    File.WriteAllLines( //
        "Makefile",
        [ for m in _mk -> $"include mk/{m}.mk" ],
        Text.Encoding.UTF8
    )

    for m in _mk do
        touch $"mk/{m}.mk"

    patch ()

let doc () = //
    for d in [ "doc"; "doc/C"; "doc/F"; "doc/compiler" ] do
        mkdir d

    write ("doc/.gitignore", "html/\n")

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

let x86_64_linux_gnu () = //
    write (
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

let arm_none_eabi () =

    write (
        "cmake/arm-none-eabi.cmake", //
        "\
set(CMAKE_SYSTEM_NAME       Generic)
set(CMAKE_SYSTEM_PROCESSOR  arm)
set(TOOLCHAIN_PREFIX        arm-none-eabi)
set(CMAKE_CROSS_COMPILING   true)
set(CMAKE_EXECUTABLE_SUFFIX \".elf\")

include(any_toolchain)

add_compile_definitions(
    CORTEX ${SERIES}
)

add_compile_options(
    -mthumb
    -ffunction-sections -fdata-sections
    $<$<COMPILE_LANGUAGE:CXX>:-nostdinc++>
    $<$<COMPILE_LANGUAGE:CXX>:-fno-rtti>
    $<$<COMPILE_LANGUAGE:CXX>:-fno-exceptions>
    $<$<COMPILE_LANGUAGE:CXX>:-fno-threadsafe-statics>
    $<$<COMPILE_LANGUAGE:ASM>:-x$<SEMICOLON>assembler-with-cpp>
    $<$<COMPILE_LANGUAGE:ASM>:-MMD>
    $<$<COMPILE_LANGUAGE:ASM>:-MP>
)

set(CMAKE_TRY_COMPILE_TARGET_TYPE STATIC_LIBRARY)
add_link_options(
    -mthumb
    -T ${CMAKE_SOURCE_DIR}/hw/${HW}/${CPU_}x_FLASH.ld
    --specs=nano.specs
    -Wl,--start-group -lc -lm -lnosys   -Wl,--end-group
    -Wl,--start-group -lstdc++ -lsupc++ -Wl,--end-group
    -Wl,-Map=${CMAKE_PROJECT_NAME}.map -Wl,--gc-sections
)
"
    )

let any_toolchain () = //
    write (
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

let version () = //

    write (
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

set(BIN_OUTPUT_NAME \"${CMAKE_PROJECT_NAME}_${HW}_${BRANCH}_${REL}_${NOW}\")
"
    )

let linux_cpp () = //
    write (
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

let linux_hpp () = //
    write (
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


let linux () = //
    linux_cpp ()
    linux_hpp ()


let ini () = //
    write (
        "lib/fake.ini",
        "\
#!/usr/bin/env Flang
# line comment
-020 +030 # integer
nop halt  # command
"
    )


let hpp () = //
    write (
        "inc/fake.hpp",
        "\
#pragma once

#ifdef __cplusplus
extern \"C\" {
#endif

extern void setup();
extern void loop();

#ifdef __cplusplus
} // extern \"C\"
#endif
"
    )

let cpp () = //
    write (
        "src/fake.cpp",
        "\
#include \"fake.hpp\"

void setup() {}
void loop() {}
"
    )

let src () = //
    ini ()
    linux ()

    hpp ()
    cpp ()

    write (
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
    # CortexM/CubeMX
    hw/${HW}/Core/Src/*.c*
    # hw/${HW}/Drivers/CMSIS/Device/ST/${SERIES}xx/Source/*.c*
    hw/${HW}/Drivers/${SERIES}xx_HAL_Driver/Src/*.c*
    hw/${HW}/USB_DEVICE/App/*.c* hw/${HW}/USB_DEVICE/Target/*.c*
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Core/Src/*.c*
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Class/CDC/Src/*.c*
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Class/AUDIO/Src/*.c*
)

file(GLOB H
    RELATIVE ${CMAKE_SOURCE_DIR}
    inc/*.h*
    # cross
      hw/inc/*.h*   hw/${HW}/inc/*.h*
     cpu/inc/*.h*  cpu/${CPU}/inc/*.h*
    arch/inc/*.h* arch/${ARCH}/inc/*.h*
      os/inc/*.h*   os/${OS}/inc/*.h*
    # CortexM/CubeMX
    hw/${HW}/Core/Inc/*.h*
    hw/${HW}/Drivers/CMSIS/Include/*.h*
    hw/${HW}/Drivers/CMSIS/Device/ST/${SERIES}xx/Include/*.h*
    hw/${HW}/Drivers/${SERIES}xx_HAL_Driver/Inc/*.h*
    hw/${HW}/USB_DEVICE/App/*.h* hw/${HW}/USB_DEVICE/Target/*.h*
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Core/Inc/*.h*
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Class/CDC/Inc/*.h*
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Class/AUDIO/Inc/*.h*
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
    # CortexM/CubeMX
    hw/${HW}/Core/Inc
    hw/${HW}/Drivers/CMSIS/Include
    hw/${HW}/Drivers/CMSIS/Device/ST/${SERIES}xx/Include
    hw/${HW}/Drivers/${SERIES}xx_HAL_Driver/Inc
    hw/${HW}/USB_DEVICE/App hw/${HW}/USB_DEVICE/Target
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Core/Inc
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Class/CDC/Inc
    hw/${HW}/Middlewares/ST/STM32_USB_Device_Library/Class/AUDIO/Inc
)
include_directories(${INC})
"
    )

let _hw_cortex = [ "pillF030"; "f429disco" ]
let _hw = _hw_cortex @ [ "esp8266"; "pc" ]


let cmake () = //

    mkdir "cmake"

    for c in _cmake do
        touch $"cmake/{c}.cmake"

    for t in _target do
        touch $"cmake/{t}.cmake"
        x86_64_linux_gnu ()
        arm_none_eabi ()
        any_toolchain ()

    version ()
    src ()

    write ( //
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
message(\"-- |    linker: \" \"${LD}\")
message(\"-- |    binary: \" ${CMAKE_INSTALL_PREFIX}/${BIN_OUTPUT_NAME}${CMAKE_EXECUTABLE_SUFFIX})
message(\"-- |\")

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

    let _build hw = //
        $$"""{
            "name"            :  "{{hw}}",
            "configurePreset" :  "{{hw}}",
            "targets"         : ["all","install"]
        }"""

    let builders =
        (String.concat
            ",\n        "
            ([ //
               for hw in _hw do
                   if hw <> "pc" then
                       _build hw ]
             @ [ _build "linux" ]))

    let _config hw inher vars = //
        $$"""{
            "name"            : "{{hw}}",
            "inherits"        : "{{inher}}",
            "cacheVariables"  : {{vars}}
        }"""

    let configs =
        String.concat
            ",\n        "
            [ //
              for hw, inher, vars in
                  [ //
                    ("pillF030", "cortexM0", """{"HW":"pillF030","CPU":"stm32f030f4p","SERIES":"STM32F0"}""")
                    ("f429disco", "cortexM4", """{"HW":"f429disco","CPU":"stm32f429zit","SERIES":"STM32F4"}""")
                    ("esp8266", "xtensa", """{"HW":"esp8266","CPU":"lx106"}""")
                    ("linux", "pc", """{"OS":"linux"}""") ] ->  //
                  (_config hw inher vars) ]

    write ( //
        "CMakePresets.json",
        $$"""{
    "version": 6,
    "buildPresets": [
        {{builders}}
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
            "name"            :  "cortexM",
            "inherits"        :  "common",
            "hidden"          :   true,
            "toolchainFile"   :  "${sourceDir}/cmake/arm-none-eabi.cmake",
            "cacheVariables"  : {"OS":"bare"}
        },
        {
            "name"            :  "cortexM0",
            "inherits"        :  "cortexM",
            "hidden"          :   true,
            "cacheVariables"  : {"ARCH":"cortexM0"}
        },
        {
            "name"            :  "cortexM4",
            "inherits"        :  "cortexM",
            "hidden"          :   true,
            "cacheVariables"  : {"ARCH":"cortexM4"}
        },
        {
            "name"            :  "xtensa",
            "inherits"        :  "common",
            "hidden"          :   true,
            "toolchainFile"   :  "${sourceDir}/cmake/xtensa-lx106-elf.cmake",
            "cacheVariables"  : {"ARCH":"xtensa","OS":"rtos"}
        },
        {
            "name"            :  "pc",
            "inherits"        :  "common",
            "hidden"          :   true,
            "cacheVariables"  : {"HW":"pc", "CPU":"i5", "ARCH":"x86_64"}
        },
        {{configs}}
    ]
}
"""
    )


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


let pillF030 () = //
    write (
        "hw/pillF030/pillF030.ocd",
        "\
gdb_port 12345
source [find interface/stlink-v2.cfg]
adapter   speed  1800
transport select hla_swd
source [find target/stm32f0x.cfg]

gdb_memory_map    enable
gdb_flash_program enable
arm semihosting   enable
"
    )

let hw () = //
    for hw in _hw do
        _cross hw "hw"

    write ("hw/.gitignore", ".mxproject\n")

    pillF030 ()

let stm32f429zit () = //
    write ("cpu/stm32f429zit/stm32f429zit.cmake", "")

let stm32f030f4p () = //
    write (
        "cpu/stm32f030f4p/stm32f030f4p.cmake",
        "\
add_compile_definitions(
    STM32F030x6
)
"
    )

let cpu () = //
    for cpu in [ "i5"; "stm32f429zit"; "stm32f030f4p"; "lx106" ] do
        _cross cpu "cpu"

    stm32f429zit ()
    stm32f030f4p ()

let x86_64 () = //
    ()

let cortexM () = //
    write (
        "arch/cortexM/cortexM.cmake",
        "\
add_compile_options(
    -mthumb
)

add_compile_definitions(
    USE_HAL_DRIVER
)

add_link_options(
)
"
    )

let cortexM0 () = //
    write (
        "arch/cortexM0/cortexM0.cmake",
        "\
include(arch/cortexM/cortexM.cmake)

set(MCPU -march=armv6-m   -mcpu=cortex-m0 )

add_compile_options( ${MCPU} ${MFPU} )

add_compile_definitions(
    # PREFETCH_ENABLE=1
    # INSTRUCTION_CACHE_ENABLE=1
    # DATA_CACHE_ENABLE=1
)

add_link_options   ( ${MCPU} ${MFPU} )
"
    )

let cortexM3 () = //
    ()

let cortexM4 () = //
    write (
        "arch/cortexM4/cortexM4.cmake",
        "\
include(arch/cortexM/cortexM.cmake)

set(MCPU -march=armv7e-m   -mcpu=cortex-m4)
set(FCPU -mfpu=fpv4-sp-d16 -mfloat-abi=hard)

add_compile_options(
    ${MCPU} ${MFPU}
)

add_compile_definitions(
)

add_link_options(
    ${MCPU} ${MFPU}
)
"
    )

let xtensa () = //
    ()

let arch () = //

    for arch in [ "x86_64"; "cortexM"; "cortexM0"; "cortexM3"; "cortexM4"; "xtensa" ] do
        _cross arch "arch"

        if Regex.IsMatch(arch, "cortexM[0-9]") then
            write ( //
                $"arch/{arch}/{arch}.mk",
                "include arch/cortexM.mk\n"
            )

        match arch with
        | "cortexM" ->
            write (
                "arch/cortexM/cortexM.mk",
                "\
TARGET  = arm-none-eabi
APT    += $(target)-gcc gdb-multiarch qemu-system-arm
APT    += openocd stlink-tools dfu-util
QEMU    = qemu-system-arm
"
            )
        | _ -> ()

    x86_64 ()
    cortexM ()
    cortexM0 ()
    cortexM3 ()
    cortexM4 ()
    xtensa ()


let os () = //

    for os in [ "bare"; "linux"; "win32"; "rtos" ] do
        _cross os "os"

let cross () = //

    for m in [ "hw"; "cpu"; "arch"; "os" ] do
        _cross m "."

    hw ()
    cpu ()
    arch ()
    os ()

let extensions () = //
    touch $".vscode/extensions.json"

let settings () = //
    touch $".vscode/settings.json"

let tasks () = //
    write (
        $".vscode/tasks.json",
        $$"""{
    "version": "2.0.0",
    "tasks": [
        {
            "label"          : "project: install",
            "type"           : "shell",
            "command"        : "make install",
            "presentation"   : {"focus": true},
            "problemMatcher" : []
        },
        {
            "label"          : "project: update",
            "type"           : "shell",
            "command"        : "make update",
            "presentation"   : {"focus": true},
            "problemMatcher" : []
        },
        {
            "label"          : "git: whoami",
            "type"           : "shell",
            "command"        : "make `whoami`",
            "problemMatcher" : []
        },
        {
            "label"          : "git: dev",
            "type"           : "shell",
            "command"        : "make dev",
            "problemMatcher" : []
        },
        {
            "label"          : "git: checkout .vscode",
            "type"           : "shell",
            "command"        : "git checkout .vscode/settings.json",
            "problemMatcher" : []
        },
        {
            "type"           : "cmake",
            "label"          : "cmake: build",
            "command"        : "build",
            "targets"        : ["all"],
            "preset"         : "${command:cmake.activeBuildPresetName}",
            "group"          : "build",
            "problemMatcher" : [],
            "promptOnClose"  : false
        },
        {
            "label"          : "openocd: debug",
            "type"           : "shell",
            "group"          : {"kind": "build", "isDefault": true},
            // "dependsOn"      : "CMake: build",
            "command"        : "openocd -f ${workspaceFolder}/hw/${command:cmake.activeConfigurePresetName}/${command:cmake.activeConfigurePresetName}.ocd -c \"program ${command:cmake.launchTargetPath} verify reset\"",
            "problemMatcher" : [],
            "presentation"   : {"showReuseMessage": false, "focus": false, "reveal": "silent", "close": true}
        },
    ]
}
"""
    )

let launch () = //


    let lhdr name =
        $$"""
        {
            "name"         : "{{name}}",
            "type"         : "cppdbg",
            "request"      : "launch",
            "program"      : "${command:cmake.launchTargetPath}",
            // "preLaunchTask": "CMake: build",
            "cwd"          : "${workspaceFolder}",
            "MIMode": "gdb",
            "setupCommands": [
                {"text": "-enable-pretty-printing", "ignoreFailures": true}
            ],"""

    let linux =
        lhdr "linux"
        + """
            "args": ["${workspaceFolder}/lib/${workspaceFolderBasename}.ini"],
            "externalConsole": false,
            "stopAtEntry": true,
        }"""

    let cortex =
        lhdr "cortex"
        + """
            "miDebuggerPath"           : "gdb-multiarch",
            "miDebuggerArgs"           : "--silent",
            "miDebuggerServerAddress"  : "localhost:12345",
            "useExtendedRemote"        : true,
            "stopAtEntry"              : false, // don't enable!
            "postRemoteConnectCommands": [
                {"text": "monitor reset halt"},
                {"text": "monitor arm semihosting enable"},
                {"text": "load"},
                {"text": "set substitute-path /home/pere/src/newlib-salsa ${userHome}/em/ref/newlib-salsa"},
                // {"text": "b Reset_Handler"},
                // {"text": "b DefaultHandler"},
                // {"text": "b SystemInit"},
                // {"text": "b __libc_init_array"},
                // {"text": "b main"},
                {"text": "b setup"},
                {"text": "b loop"},
                {"text": "monitor reset halt"}, // req
            ],
        },"""

    let json =
        "\
{
    \"version\": \"0.2.0\",
    \"configurations\": ["
        + cortex
        + linux
        + "
    ]
}
"

    write (".vscode/launch.json", json)

launch ()

let c_cpp_properties () = //
    touch $".vscode/c_cpp_properties.json"

let vscode () = //
    mkdir ".vscode"
    extensions ()
    settings ()
    tasks ()
    launch ()
    c_cpp_properties ()

let files () =
    for f in _files do
        touch f

    apt ()
    doc ()
    mk ()
    cmake ()
    cross ()
    vscode ()

[<EntryPoint>]
let main (args: string[]) =
    dirs ()
    files ()
    src ()
    linux ()
    0
