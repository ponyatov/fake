.PHONY : install update ref gz
install: $(HOST)_install doc ref gz dotnet
update : $(HOST)_update
ref    : $(RF)
gz     : $(GZ)

Debian_install: Debian_update
Debian_update:
	sudo apt update
	sudo apt install -uy `cat apt.$(HOST)` $(APT)

Msys_install: doc ref gz
	pacman -Suy
Msys_update:
	pacman -S $(shell cat apt.$(HOST) | tr '\n' ' ') $(MSYS)

.PHONY: rust
rust:
	rustup self update
	rustup update
	rustup target add thumbv7em-none-eabihf
