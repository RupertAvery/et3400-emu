# Introduction

The [ET-3400](http://www.oldcomputermuseum.com/heathkit_et3400.html) was a microprocessor trainer kit sold in the 70's and 80's and was designed to be assembled by the end user and was used to teach fundamentals of microprocessors.  

The kit had the following features:

* The popular [Motorola 6800](http://en.wikipedia.org/wiki/Motorola_6800) 8-bit CPU clocked at 100kHz
* Six 7-segment LED displays
* 17 push-button switches arranged as a hex keypad
* 8 LEDs for general purpose visual output
* 1 8-position DIP switch for data input
* 1KB ROM
* 256 bytes RAM (expandable to 512 bytes)
* buffered 8-bit data and 16-bit address ports

The ROM contained a program allowing the user to (tediously, i.e. byte-by-byte) enter, run and monitor programs in RAM using the keypad and the screen.

This project simulates the ET-3400 by emulating the 6800 CPU and memory-mapped access to simulated 7-segment displays and keypad for I/O. The ROM program is included.

# Usage

You can download the trainer's Manual [here](http://archive.org/details/HeathkitManualForTheEt-3400MicroprocessorTrainer). 

The manual mainly details the assembly of the hardware of the kit.

You can click on the hex kepad button images to simulate button presses. However, since the button detection is done in 6800 software, mouse clicks might sometimes be too short to register. It's usually better to just press the corresponding key on the keyboard when the Trainer has focus.'

Here is an index of interesting information related to the software that will be useful for emulation:

| PDF Page | Manual Page | Section          | 
|:--------:|:-----------:|------------------|
| 47       |     45      | Operation        |
| 57       |     55      | Sample Programs  |
| 75       |     74      | Monitor Listing  |
| 89       |     87      | Meomry Map       |
| 91       |     89      | Instruction Set  |

# Debugger

Press `CTRL-D` or click `View` > `Debugger` at the Trainer window to bring up the Debugger.

The Debugger has a raw memory display and a disassembly display. Status registers are displayed at the right side.

At the top of the Debugger is a toolstrip menu. You can control execution with the buttons here. There are also two dropdowns labeled **Memory** and **Disassembly**. Selecting an item will update the memory or disassembly views with the raw or disassembled memory respectively at the preset address of the selected item.

| Label    |    Memory     | Description                                       | 
|:--------:|:-------------:|---------------------------------------------------|
| RAM      |    $0000      | Program RAM and Stack                             |
| Keypad   |    $C003      | Keypad mapped memory                              |
| Display  |    $C110      | 7-segment display mapped memory                   |
| ROM      |    $FC00      | Monitor ROM                                       |

When the disassembly view has focus, you can press `Up` and `Down` arrow keys to move the currently selected address. You can set a breakpoint at the current selected address by pressing `F9`.  The emulator will halt when a breakpoint is hit, allowing you to step through the program. 

You can also halt execution anytime by pressing `F4`. You can step through statements by pressing `F10` when execution is halted. The registers will update as each operation is executed. You may resume execution by pressing `F5`.

Click on `Emulator` > `Enable Breakpoints` to toggle whether execution will stop on breakpoints. It is enabled by default. 

# SREC (.OBJ) file format

The emulator supports SREC (S19) format files. Click `File` > `Load RAM` and select your .OBJ file to load it into memory.  

You can also save RAM (or any memory range) to SREC format by clicking `File` > `Save RAM`. You will be prompted for a memory range, and then prompted for a location to save the file.

## Quick ET-3400 Guide

Here are the keys you can press when the trainer has focus and the corresponding button on the keypad.

| Keypress |    Button     | Action                                            | 
|:--------:|:-------------:|---------------------------------------------------|
|    1     |  **ACCA**     | View contents of Accumulator A Register           |
|    2     |  **ACCB**     | View contents of Accumulator B Register		   |
|    3     |  **PC**       | View contents of Program Counter Register		   |
|    4     |  **INDEX**    | View contents of Index Pointer Register		   |
|    5     |  **CC**       | View contents of Condition Codes Register		   |
|    6     |  **SP**       | View contents of Stack Pointer Register		   |
|    7     |  **RTI**      | Return from Interrupt							   |
|    8     |  **SS**       | Single Step									   |
|    9     |  **BR**       | Break											   |
|    A     |  **AUTO**     | Start entering hex at specified address		   |
|    B     |  **BACK**     | During Examine mode, move address back		       |
|    C     |  **CHAN**     | During Examiine mode, edit hex at specified address. During ACCA/ACCB/PC mode, edit hex in selected register |
|    D     |  **DO**       | Execute RAM at given address                      |
|    E     |  **EXAM**     | Start viewing hex at specified address            |
|    F     |  **FWD**      | During Examine mode, move address forward         |
|   ESC    |  **RESET**    | Reset the CPU         |

# History

This emulator is inspired by a lot of previous works, notably Marat Fayzullin's [How To Write a Computer Emulator](http://fms.komkon.org/EMUL8/HOWTO.html). It was originally written in Visual Basic 6, ported to C++, and C#.

The C# emulator core was originally written from scratch, but after some hard-to-fix bugs, it was rewritten using MAME 6800 emulator source as reference.

# Thanks

I would like to thank Rick Nungester for taking interest in the project and testing the emulator and providing invaluable information for improving the emulator.

