APT     += linux-source uclibc-source

LINUX_CFG += $(CWD)/hw/all.linux
LINUX_CFG += $(CWD)/arch/$(ARCH)/$(ARCH).linux
LINUX_CFG += $(CWD)/cpu/$(CPU)/$(CPU).linux
LINUX_CFG += $(CWD)/hw/$(HW)/$(HW).linux
LINUX_CFG += $(CWD)/$(MODULE).linux

LINUX_MAKE  = $(MAKE) -f /usr/src/linux-source-$(LINUX_VER)/Makefile
LINUX_MAKE += ARCH=$(ARCH) CROSS_COMPILE=$(TARGET)-
LINUX_MAKE += INSTALL_PATH=$(BOOT)
LINUX_MAKE += INSTALL_MOD_PATH=$(ROOT)
LINUX_MAKE += INSTALL_HDR_PATH=$(ROOT)/usr
# LINUX_MAKE += INSTALL_DTBS_PATH=$(ROOT)/boot/dtbs

.PHONY: linux
linux: tmp/kernel/.config
	cd tmp/kernel ;\
	$(LINUX_MAKE) menuconfig && $(LINUX_MAKE) -j4 &&\
	$(LINUX_MAKE) install modules_install headers_install &&\
	rm -f $(BOOT)/*.old
# dtbs_install

tmp/kernel/.config: $(LINUX_CFG) mk/cross.mk os/linux/linux.mk
	cd tmp/kernel ; rm .config ; \
	$(LINUX_MAKE) allnoconfig ;\
	cat $(LINUX_CFG) >> .config ;\
	echo 'CONFIG_LOCALVERSION="-$(HW)"'        >> .config ;\
	echo 'CONFIG_DEFAULT_HOSTNAME="$(MODULE)"' >> .config

UCLIBC_MK   = $(CWD)/tmp/uClibc-ng-$(UCLIBC_VER)/Makefile
UCLIBC_MAKE = $(MAKE) -f $(UCLIBC_MK) CROSS=$(TARGET)-
UCLIBC_CFG += $(CWD)/hw/all.uclibc
UCLIBC_CFG += $(CWD)/arch/$(ARCH)/$(ARCH).uclibc
UCLIBC_CFG += $(CWD)/cpu/$(CPU)/$(CPU).uclibc

.PHONY: uclibc
uclibc: $(UCLIBC_MK) $(UCLIBC_CFG) os/linux/linux.mk
	cd $(dir $<) ; rm .config ; $(UCLIBC_MAKE)    allnoconfig ;\
	cat $(UCLIBC_CFG)                              >> .config ;\
	echo 'KERNEL_HEADERS="$(ROOT)/usr/include"'    >> .config ;\
	echo 'RUNTIME_PREFIX="$(ROOT)/uclibc/runtime"' >> .config ;\
	echo 'DEVEL_PREFIX="$(ROOT)/usr"'              >> .config ;\
	echo 'CROSS_COMPILER_PREFIX="$(TARGET)-"'      >> .config ;\
	$(UCLIBC_MAKE) menuconfig &&\
	$(UCLIBC_MAKE) -j4 && $(UCLIBC_MAKE) install

GZ += /usr/src/linux-source-$(LINUX_VER)/README
/usr/src/linux-source-$(LINUX_VER)/README: /usr/src/linux-source-$(LINUX_VER).tar.xz
	cd /usr/src ; xzcat $< | sudo tar x && sudo touch $@
GZ += $(UCLIBC_MK)
$(UCLIBC_MK): /usr/src/uClibc-ng-$(UCLIBC_VER).tar.xz
	cd tmp ; xzcat $< | tar x && touch $@
