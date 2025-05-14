GZ += doc/C/ansi-iso-9899-1990-1.pdf
doc/C/ansi-iso-9899-1990-1.pdf:
	$(CURL) $@ https://www.yodaiken.com/wp-content/uploads/2021/05/ansi-iso-9899-1990-1.pdf

GZ += doc/FORTH/Brodie_Starting_ru.pdf
doc/FORTH/Brodie_Starting_ru.pdf:
	$(CURL) $@ https://nncron.ru/download/sf.pdf

GZ += doc/FORTH/Threaded_interpretive_languages.pdf
doc/FORTH/Threaded_interpretive_languages.pdf:
	$(CURL) $@ https://sinclairql.speccy.org/archivo/docs/books/Threaded_interpretive_languages.pdf

.PHONY: doxy
doxy: .doxygen doc/DoxygenLayout.xml vscode/logo.png
	rm -rf doc/html ; doxygen $< 1>/dev/null

.PHONY: doc
doc:
