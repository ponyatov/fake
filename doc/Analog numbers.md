# Analog numbers

dedicated type that combines:
- [[arbitrary width integer]] value
	- represents real value taken from ADC
	- or constructed for DAC/PWM output
- coupled with
	- [[fake/floating point|floating point]] or [[fake/Integer|Integer]] [[fake/Range|Range]] 
		- to let you always know and track minimal..maximal range of some real-world variable
	- and [[Measure]] units
		- specifies measurement units of the real physical world
		- type system blocks your errors with adding and multiplicating kilograms, inches, pounds, watts and mm/sec
