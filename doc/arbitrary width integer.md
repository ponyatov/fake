# arbitrary width integer

- the same `i/u` signed/unsigned [[fake/Integer|Integer]]
- but with arbitrary bit-width
	- from the `u1` (single bit)
	- to any `u/iN` (bit stream fragment in memory)

The common cases that forces as to include arbitrary integers into the [[F0]] language core layer:
- ADC/DAC values starting from 4..24 bits
	- see [[Analog numbers]] that combines [[arbitrary width integer]] value coupled with [[fake/floating point|floating point]] [[fake/Range|Range]] and [[Measure]] into a dedicated type
- [[bit field]]s in packed structures
	- hardware register elements from one to multiple bits
	- arbitrary binary data formats
