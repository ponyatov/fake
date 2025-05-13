HW ?= qemu386
include hw/$(HW)
include cpu/$(CPU)
include arch/$(ARCH)
include os/$(OS)
