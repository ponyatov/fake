APT += linux-source

LINUX_CFG += $(CWD)/hw/all.linux
LINUX_CFG += $(CWD)/arch/$(ARCH)/$(ARCH).linux
LINUX_CFG += $(CWD)/cpu/$(CPU)/$(CPU).linux
LINUX_CFG += $(CWD)/hw/$(HW)/$(HW).linux
LINUX_CFG += $(CWD)/$(MODULE).linux

LINUX_MAKE  = $(MAKE) -f /usr/src/linux-source-$(LINUX_VER)/Makefile
LINUX_MAKE += ARCH=$(ARCH) CROSS_COMPILE=$(TARGET)-

.PHONY: linux
linux: tmp/kernel/.config
	cd tmp/kernel ;\
	$(LINUX_MAKE) menuconfig

tmp/kernel/.config: $(LINUX_CFG) mk/cross.mk os/linux/linux.mk
	cd tmp/kernel ; rm .config ; \
	$(LINUX_MAKE) allnoconfig ;\
	cat $(LINUX_CFG) >> .config ;\
	echo 'CONFIG_LOCALVERSION="-$(HW)"' >> .config ;\
	echo 'CONFIG_DEFAULT_HOSTNAME="$(MODULE)"'    >> .config
