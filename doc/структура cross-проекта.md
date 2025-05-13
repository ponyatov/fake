# структура [[em/cross]]-проекта
https://wiki.yandex.ru/33.-programmnye-sredstva/cross/

## [[кросс-компилятор]]

компилятор запускаемый на BUILD/HOST-архитектуре, предназначенный для компиляции для TARGET-архитектуры

- `BUILD`
  - архитектура рабочей станции для разработки и отладки ПО
- `HOST`
  - архитектура на которой запускается компилятор
  - чаще всего совпадает с `BUILD`
    - canadian cross:
      - BUILD=[[x86_64-linux-gnu]] build-сервер используется для сборки системы в целом
      - HOST=[[aarch64-linux-gnu]] компилятор `gcc` будет запускаться на RPi
      - TARGET=[[arm-none-eabi]] чтобы собирать прошивку для модуля pillF103 подключенный в USB
- `TARGET`
  - архитектура целевой аппаратуры
    - `x86_64-linux-gnu`
      - рабочая станция (64-битные i5/i7)
    - `i486-linux-uclibc`
      - специализированная сборка embedded Linux для Vortex86 PC104 (32 bit)
    - `arm-none-eabi[hf]`
      - микроконтроллеры [Cortex-Mx](cortexM)
    - `aarch64-linux-gnu`
      - [Raspberry Pi](rpi)
    - `xtensa-lx106-elf`
      - [ESP8266/ESP32](xtensa)

## файловая структура

### общая структура
```
.vscode
bin/			исполняемые файлы, файлы прошивок, образы дисков
doc/			документация
  html/			Doxygen-документация сгенерированная из исходного кода
lib/			библиотеки, скрипты, файлы инициализации
tmp/			временные файлы, build-каталоги
ref/			копии исходного кода сторонних проектов, используемых как образцы кода
README.md
LICENSE
apt.Debian		список пакетов Debian/Linux
apt.Msys		список пакетов Windows/MinGW/Msys
```

### [GNU Make](make)

```
mk/				include-фрагменты Makefile
Makefile		головной файл GNU make
```

### [CMake](cmake)

```
cmake/				библиотека фрагментов
CMakeLists.txt		головной файл CMake-проекта
CMakePresets.json	описание вариантов сборки (целевая HW переключается в IDE)

```
### группировка исходного кода

- код специфичный для конкретного проекта
```
  inc/				заголовочные header-файлы, специфичные для проекта
    project.hpp
  src/				исходный код C/C++, специфичный для проекта
    project.cpp		головной файл кода проекта
    lexer.lex			синтаксический лексер
    parser.yacc		синтаксический парсер
    str2hex.ragel		функция конверсии hex-строки (на регулярных выражениях)
```
- скрипты и библиотеки
```
lib/				библиотеки, скрипты, файлы инициализации
  modbus/			библиотека MODBUS
    inc/
    src/
  cli/				REPL/CLI командная оболочка: библиотечный компонент
    inc/
    src/
  project.ini		файл инициализаци приложения
  project.f			скрипт исполняемый приложением (на движке CLI)
```
- `HW`: выбор конкретной целевой железки
```
hw/
  iskra/			файлы кода специфичные для IskraJS (как железка в целом: код доступа к внешним чипам)
    iskra.mk
    iskra.cmake
    iskra.odb		скрипт отладчика OpenOCD
    iskra.gdb
  f429disco/		отладка STM32F429-DISCO
  f4disco/			отладка STM32F4-DISCO
  l496disco/
  pillF0/
  pillF1/			модуль на базе STM32F103
  esp8266/
  esp32/
  esp32c6/			с экранчиком на базе RiscV
  pc/				рабочая станция (ноутбук)
  pi800/			клавиатура Orange Pi 800
```
- `CPU`: процессор или SoC
```
cpu/
  stm32f429zig/
  stm32f405rgt/			файлы специфичные для STM32F405
    stm32f405rgt.mk
    stm32f405rgt.cmake
  stmref407vgt/
  lx106/			ESP/xtensa
  i5/				процессор рабочей станции
  bcm2835/			типовой SoC на Raspberry Pi
  allwinnerA10/
  allwinnerA20/
  allwinnerH3/		Orange Pi
```
```
arch/				код специфичный для архитектуры в целом (безх привязки к конкретному CPU)
  cortexM/
  cortexM4/
  	cortexM4.mk		-> `include arch/cortexM/cortexM.mk`
  	cortexM4.cmake	-> `include arch/cortexM/cortexM.cmake`
  xtensa/
```
```
os/
  bare/				железо без ОС
  freertos/			RTOS
  linux/			Linux
  mingw/			Windows (win32/MinGW)
```
