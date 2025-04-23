# [[LLVM/LLVM|LLVM]] Interop

Target code generation:

- [[Bare C]] = [[Nim/Nim|Nim]]
	- the most portable and human-friendly way
	- source code can be checked by eyes
	- can be compiled
- [[fake/C++|C++]]
	- without any [[Cpp/STL|STL]] and [[libstdc++]]: not available on MCU devices
	- provides [[C++ Interop]] for complex libraries
- [[LLVM/LLVM|LLVM]]
	- using [[LLVMSharp]]
	- the most low-level but still portable way
