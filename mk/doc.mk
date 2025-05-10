GZ += doc/C/ansi-iso-9899-1990-1.pdf
doc/C/ansi-iso-9899-1990-1.pdf:
	$(CURL) $@ https://www.yodaiken.com/wp-content/uploads/2021/05/ansi-iso-9899-1990-1.pdf

.PHONY: doxy
doxy: .doxygen doc/DoxygenLayout.xml vscode/logo.png
	rm -rf doc/html ; doxygen $< 1>/dev/null

.PHONY: doc
doc:
