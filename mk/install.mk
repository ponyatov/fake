.PHONY : install update ref gz
install: $(HOST)_install $(RUSTUP) doc ref gz
	$(RUSTUP) component add rustfmt
	$(RUSTUP) target    add $(RTARGET)
	$(CARGO)  install   cargo-binutils
	$(RUSTUP) component add llvm-tools
update : $(HOST)_update  $(RUSTUP) $(PIP)
	$(PIP) install -U -r requirements.txt
ref    : $(RF)
gz     : $(GZ)

Debian_install: Debian_update
# sudo dpkg --add-architecture i386
Debian_update:
	sudo apt update
	sudo apt install -uy `cat apt.$(HOST)` $(APT)

Msys_install: doc ref gz
	pacman -Suy
Msys_update:
	pacman -S $(shell cat apt.$(HOST) | tr '\n' ' ') $(MSYS)
