HW ?= qemu386
-include   hw/$(HW)/$(HW).mk
-include  cpu/$(CPU)/$(CPU).mk
-include arch/$(ARCH)/$(ARCH).mk
-include   os/$(OS)/$(OS).mk

.PHONY: cross
cross:
	@echo $@: hw:$(HW) cpu:$(CPU) arch:$(ARCH) os:$(OS)
	$(MAKE) $(OS)

.PHONY: iso $(CWD)/bin/$(MODULE).i386.iso
iso: $(CWD)/bin/$(MODULE).i386.iso
$(CWD)/bin/$(MODULE).i386.iso:
	xorriso -as mkisofs -o $@ -r root -J -isohybrid-mbr \
		-isohybrid-mbr  /usr/lib/ISOLINUX/isohdpfx.bin \
		-b isolinux.bin \
		-c boot.cat -no-emul-boot -boot-load-size 4 -boot-info-table -J -R \
		-V "$(MODULE)@$(HW)"
