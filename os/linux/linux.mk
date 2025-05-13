APT += linux-source

LINUX_CFG += $(CWD)/hw/all.linux
LINUX_CFG += $(CWD)/arch/$(ARCH)/$(ARCH).linux
LINUX_CFG += $(CWD)/cpu/$(CPU)/$(CPU).linux
LINUX_CFG += $(CWD)/hw/$(HW)/$(HW).linux
LINUX_CFG += $(CWD)/$(MODULE).linux

LINUX_MAKE  = $(MAKE) -f /usr/src/linux-source-$(LINUX_VER)/Makefile
LINUX_MAKE += ARCH=$(ARCH) CROSS_COMPILE=$(TARGET)-
LINUX_MAKE += INSTALL_PATH=$(CWD)/root/boot
LINUX_MAKE += INSTALL_MOD_PATH=$(CWD)/root
LINUX_MAKE += INSTALL_HDR_PATH=$(CWD)/root/usr/include
LINUX_MAKE += INSTALL_DTBS_PATH=$(CWD)/root/boot/dtbs

.PHONY: linux
linux: tmp/kernel/.config
	cd tmp/kernel ;\
	$(LINUX_MAKE) menuconfig && $(LINUX_MAKE) -j4 &&\
	$(LINUX_MAKE) install modules_install headers_install dtbs_install

tmp/kernel/.config: $(LINUX_CFG) mk/cross.mk os/linux/linux.mk
	cd tmp/kernel ; rm .config ; \
	$(LINUX_MAKE) allnoconfig ;\
	cat $(LINUX_CFG) >> .config ;\
	echo 'CONFIG_LOCALVERSION="-$(HW)"'        >> .config ;\
	echo 'CONFIG_DEFAULT_HOSTNAME="$(MODULE)"' >> .config
