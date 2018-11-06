# Introduction

The [ET-3400](http://www.oldcomputermuseum.com/heathkit_et3400.html) was a microprocessor trainer kit sold in the 70's and 80's and was designed to be assembled by the end user and was used to teach fundamentals of microprocessors.  

The kit had the following features:

* [Motorola 6800](http://en.wikipedia.org/wiki/Motorola_6800) 8-bit CPU clocked at 1MHz
* Six 7-segment LED displays used to display address and data information, and as a visual output device (i.e. screen)
* 17 push-button switches used as a Hex keypad
* 8 LEDs for general purpose visual output
* 1 8-position DIP switch for data input
* 1 KB ROM 
* 2 x 256 bytes RAM
* buffered 8-bit data and 16-bit address ports

The ROM contained a program allowing the user to (tediously, i.e. byte-by-byte) enter, run and monitor programs in RAM using the keypad and the screen.

This project simulates the ET-3400 by emulating the 6800 CPU and memory-mapped access to simulated 7-segment displays and keypad for I/O. The ROM program is included.

The emulator core and 7 segment display are written from scratch in 100% C#, I only got the basic idea of how an emulator should run from Marat Fayzullin's page [How To Write a Computer Emulator](http://fms.komkon.org/EMUL8/HOWTO.html). It was originally implemented in Visual Basic 6, ported to C++, then finally C#.

The emulator core for the 6800 CPU is based on the MAME source code.

# Usage

You can download the trainer's Manual [here](http://archive.org/details/HeathkitManualForTheEt-3400MicroprocessorTrainer). 

## .RAM file format

The repository contains some files with the .ram extension. These are binary files containing the machine code in plain bytes that will be loaded directly into RAM at address $0000.

The file is headered with 8 bytes that contain the ASCII characters "3400" followed by 4 zero-bytes.


## .S19 file format

The emulator supports S19 format files. Click File > Load and select your S19 OBJ file to load it into memory.  Press D on the keypad to bring up the Do prompt. You will be required to enter a 16-bit hex address in RAM. Usually this will be 0000. The ROM will then execute the program.

The manual contains program listings in assembly/machine code and the ROM listing which can be useful for calling functions such as monitoring the keyboard and displaying text on screen.

## Quick ET-3400 Guide

* **ACCA** - View contents of Accumulator A Register
* **ACCB** - View contents of Accumulator B Register
* **PC** - View contents of Program Counter Register
* **INDEX** - View contents of Index Pointer Register
* **CC** - View contents of Condition Codes Register
* **SP** - View contents of Stack Pointer Register
* **RTI** - Return from Interrupt
* **SS** - Single Step
* **BR** - Break
* **AUTO** - Start entering hex at specified address
* **BACK** - During *EXAM*ine mode, move address back
* **CHAN** - During *EXAM*ine mode, edit hex at specified address. During *ACCA/ACCB/PC* mode, edit hex in selected register
* **DO** - Execute RAM at given address
* **EXAM** - Start viewing hex at specified address
* **FWD** - During *EXAM*ine mode, move address forward