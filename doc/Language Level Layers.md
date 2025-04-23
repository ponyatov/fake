# Language Level Layers

- F0
	- [[AVR8]], [[Cortex-M0]]
	- MCU with minimal ROM/RAM sizes
	- focus on single control function or hardware interface conversion
		- small GPIO capability
		- low-speed interfaces (UART, USB/Serial)
	- optional [[fake/Wireless|Wireless]] via external modem device
	- slave and stand-alone
- F1
	- [[Cortex-M3]], [[Cortex-M4]], [[esp/ESP32|ESP32]]
	- connected MCU can run multiple functions
	- focus on multiple functions on a single device
		- large GPIO capability
		- many data flow & hw control interfaces
		- high-speed interfaces (USB HS, Ethernet)
		- control & [[fake/Signal Processing|Signal Processing]] with hw [[FPU]]
	- slave and stand-alone
- F2
	- [[Cortex-M4]], [[esp/ESP32|ESP32]], [[MIPS]], [[Mobile Devices]]
	- multiple connection interfaces
	- focuses on multiple device interconnect & distributed control
		- [[Ethernet]] or [[fake/Wireless|Wireless]] interconnect
		- distributed control over multiple local devices
		- operational data buffering and edge-level processing
	- optional [[gui/GUI|GUI]] & [[HID]]
	- local master + F3 uplink
- F3
	- full-size [[x86_64]] PC, [[Mobile Devices]], cloud services
	- focuses on
		- large size data storage & processing
		- main user interface (workstation) & [[Data Visualization]]
		- low speed top-level control (no RT)
		- high-speed [[fake/Networking|Networking]] & backend services
	- mains power

![[F0 Core Language Specification]]
![[F1 Language Layer for Slave Devices]]
![[F2 Gateways and Hub Functional]]
![[F3 Data Storage and Processing Semantics]]
