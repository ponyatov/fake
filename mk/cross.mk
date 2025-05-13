HW ?= qemu386
-include   hw/$(HW)/$(HW).mk
-include  cpu/$(CPU)/$(CPU).mk
-include arch/$(ARCH)/$(ARCH).mk
-include   os/$(OS)/$(OS).mk

.PHONY: cross
cross:
	@echo $@: hw:$(HW) cpu:$(CPU) arch:$(ARCH) os:$(OS)
