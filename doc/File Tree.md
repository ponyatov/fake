# File Tree

code generator: `meta/fileTree.fs`

```
.vscode/            VSCode settings
doc/                this manual
    C/              C/C++ specs & manuals
    F/              F#            manuals
```
- `Flang` compiler prototype:
```
meta/               compiler prototype /F#/
lib/                .f* modules & init files
.config/            \ .NET project
fake.fsproj         /
```
- Generated files:
```
bin/                target firmware & executables
inc/                .hpp files
src/                .cpp files
```
- Build & support scripts:
```
mk/                 GNU Make scripts
Makefile
cmake/              CMake cross-compiler scripts
CMakeLists.txt            project
CMakePresets.json         target selector
```
- temp dirs:
```
tmp/
ref/
obj/                F#/.NET build directory
```
