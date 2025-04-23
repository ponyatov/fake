# Language Level Layers

Language specification splitted into multiple layers in dependence with hardware capabilities and functional requirements:

- F0
	- [[AVR8]], [[Cortex-M0]]
	- MCU with minimal ROM/RAM sizes
	- focus on single control function or hardware interface conversion
		- small GPIO capability
		- low-speed interfaces (UART, USB/Serial)
	- optional [[fake/Wireless|Wireless]] via external modem device
	- slave and stand-alone
	- no REPL, pure AOT-only machine code
		- or bytecode for memory compactness
- F1
	- [[Cortex-M3]], [[Cortex-M4]], [[esp/ESP32|ESP32]]
	- connected MCU can run multiple functions
	- focus on multiple functions on a single device
		- large GPIO capability
		- many data flow & hw control interfaces
		- high-speed interfaces (USB HS, Ethernet)
		- control & [[fake/Signal Processing|Signal Processing]] with hw [[FPU]]
	- slave and stand-alone
	- REPL: interactive shell over serial or external modem
		- bytecode or dumb native compiler
		- single-user, no sessions
		- optional file system via [[semihosting]]
- F2
	- [[Cortex-M4]], [[esp/ESP32|ESP32]], [[MIPS]], [[Mobile Devices]]
	- multiple connection interfaces
	- focuses on multiple device interconnect & distributed control
		- [[Ethernet]] or [[fake/Wireless|Wireless]] interconnect
		- distributed control over multiple local devices
		- operational data buffering and edge-level processing
	- optional [[gui/GUI|GUI]] & [[HID]]
	- local master + F3 uplink
	- REPL: interactive shell
		- bytecode with JIT or optimizing native compiler
		- modules and build on file system
			- modules import over network connections & [[semihosting]]
		- interactive debug, multi-user sessions
		- local device groups & data buffering management
- F3
	- full-size [[x86_64]] PC, [[Mobile Devices]], cloud services
	- focuses on
		- large size data storage & processing
		- main user interface (workstation) & [[Data Visualization]]
		- low speed top-level control (no RTOS)
		- high-speed [[fake/Networking|Networking]] & backend services
	- mains power
	- REPL:
		- huge device groups, cluster & database management

![[F0 Core Language Specification]]
![[F1 Language Layer for Slave Devices]]
![[F2 Gateways and Hub Functional]]
![[F3 Data Storage and Processing Semantics]]
