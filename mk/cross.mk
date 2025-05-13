HW ?= qemu386
-include   hw/$(HW)/$(HW).mk
-include  cpu/$(CPU)/$(CPU).mk
-include arch/$(ARCH)/$(ARCH).mk
-include   os/$(OS)/$(OS).mk

.PHONY: cross
cross:
	@echo $@: hw:$(HW) cpu:$(CPU) arch:$(ARCH) os:$(OS)
	$(MAKE) $(OS)

ISOLINUX += $(CWD)/root/isolinux/isolinux.cfg
ISOLINUX  = $(CWD)/root/isolinux/isolinux.bin
ISOLINUX += $(CWD)/root/isolinux/ldlinux.c32
ISOLINUX += $(CWD)/root/isolinux/poweroff.c32
ISOLINUX += $(CWD)/root/isolinux/reboot.c32
ISOLINUX += $(CWD)/root/isolinux/libcom32.c32

.PHONY: iso $(CWD)/bin/$(MODULE).$(HW).iso
iso: $(CWD)/bin/$(MODULE).$(HW).iso
$(CWD)/bin/$(MODULE).$(HW).iso: $(ISOLINUX) mk/cross.mk os/linux/linux.mk
	xorriso -as mkisofs \
		-isohybrid-mbr /usr/lib/ISOLINUX/isohdpfx.bin \
		-b isolinux/isolinux.bin \
		-c boot/catalog -no-emul-boot -boot-load-size 4 -boot-info-table -J -R \
		-V $(MODULE)@$(HW) -m "*.gitignore" \
		-o $@ $(CWD)/root

$(CWD)/root/isolinux/%.c32: /usr/lib/syslinux/modules/bios/%.c32
	cp $< $@

.PHONY: qemu
qemu: $(CWD)/bin/$(MODULE).$(HW).iso
	qemu-system-i386 -cdrom $< -boot d
