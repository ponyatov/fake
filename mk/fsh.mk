MS_URL  = http://packages.microsoft.com
APT    += dotnet-runtime-$(DOTNET_VER) dotnet-sdk-$(DOTNET_VER)

.PHONY: dotnet
dotnet: \
	/etc/apt/trusted.gpg.d/microsoft.asc \
	/etc/apt/sources.list.d/microsoft.list
	$(MAKE) update .config/dotnet-tools.json
	dotnet tool install --global fantomas
	dotnet tool install --global Microsoft.dotnet-interactive
/etc/apt/trusted.gpg.d/microsoft.asc:
	sudo $(CURL) $@ $(MS_URL)/keys/microsoft.asc
/etc/apt/sources.list.d/microsoft.list:
	sudo $(CURL) $@ $(MS_URL)/config/debian/12/prod.list
.config/dotnet-tools.json:
	dotnet new tool-manifest
