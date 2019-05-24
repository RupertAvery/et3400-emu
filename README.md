# Introduction

The [ET-3400](http://www.oldcomputermuseum.com/heathkit_et3400.html) was a microprocessor trainer kit sold in the 70's and 80's and was designed to be assembled by the end user and was used to teach fundamentals of microprocessors.  

The kit had the following features:

* [Motorola 6800](http://en.wikipedia.org/wiki/Motorola_6800) 8-bit CPU clocked at 1MHz
* Six 7-segment LED displays used to display address and data information, and as a visual output device (i.e. screen)
* 17 push-button switches used as a hex keypad
* 8 LEDs for general purpose visual output
* 1 8-position DIP switch for data input
* 1 KB ROM 
* 2 x 256 bytes RAM
* buffered 8-bit data and 16-bit address ports

The ROM contained a program allowing the user to (tediously, i.e. byte-by-byte) enter, run and monitor programs in RAM using the keypad and the screen.

This project simulates the ET-3400 by emulating the 6800 CPU and memory-mapped access to simulated 7-segment displays and keypad for I/O. The ROM program is included.

# History

This emulator is inspired by a lot of previous works, notably Marat Fayzullin's [How To Write a Computer Emulator](http://fms.komkon.org/EMUL8/HOWTO.html). It was originally written in Visual Basic 6, ported to C++, and C#.

The C# emulator core was originally written from scratch, but after some hard-to-fix bugs, it was rewritten using MAME 6800 emulator source as reference.

# Usage

You can download the trainer's Manual [here](http://archive.org/details/HeathkitManualForTheEt-3400MicroprocessorTrainer). 

The manual mainly details the assembly of the hardware of the kit.

Here is an index of interesting information related to the software that will be useful for emulation:

| PDF Page | Manual Page | Section          | 
|:--------:|:-----------:|------------------|
| 47       |     45      | Operation        |
| 57       |     55      | Sample Programs  |
| 75       |     74      | Monitor Listing  |
| 89       |     87      | Meomry Map       |
| 91       |     89      | Instruction Set  |

# Debugger

A rudimentary debugger is implemented.  Press `CTRL-D` or click `View` > `Debugger` to bring up the debugger.

The debugger has a raw memory display and a disassembly display. Status registers are displayed at the bottom.

When the main window has focus, you can halt execution by pressing `F4`. You can step through statements by pressing `F10` when execution is halted. You may resume execution by pressing `F5`.

You can set breakpoints by selecting disassembly statements and pressing `F9`.  The emulator will halt once the breakpoint is hit.  

Click on `Emulator` > `Enable Breakpoints` to toggle whether execution will stop on breakpoints. It is enabled by default. 

`F4`, `F5` and `F10` will also work when the disassembly pane has focus.

# SREC (.OBJ) file format

The emulator supports SREC (S19) format files. Click `File` > `Load RAM` and select your .OBJ file to load it into memory.  

You can also save RAM (or any memory range) to SREC format by clicking `File` > `Save RAM`. You will be prompted for a memory range, and then prompted for a location to save the file.

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

# Thanks

I would like to thank Rick Nungester for taking interest in the project and testing the emulator and providing invaluable information for improving the emulator.

