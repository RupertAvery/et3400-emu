# Introduction

The [ET-3400](http://www.oldcomputermuseum.com/heathkit_et3400.html) was a microprocessor trainer kit manufactured and sold by the Heath company in the 70's and 80's under the Heathkit brand. As a kit it was a collection of electronic  components and casing and was designed to be assembled by the user. It was used to teach fundamentals of microprocessors at universities.  

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

This project simulates the ET-3400 by emulating the 6800 CPU, 7-segment displays and keypad. The ROM program is included.

# Usage

Starting the application will have the emulator boot into the ROM (Monitor program), displaying the familiar `CPU UP` message.

![image](https://user-images.githubusercontent.com/1910659/59240453-1e3e4c80-8c37-11e9-978c-06de8b5950de.png)

You can download the trainer's Manual [here](http://archive.org/details/HeathkitManualForTheEt-3400MicroprocessorTrainer). 

You can click on the hex kepad button images, or press the corresponding key on the keyboard to simulate button presses. 

Note that since the button detection is done in 6800 software, mouse clicks might sometimes be too short to register. It's usually better to press the corresponding key on the keyboard when the Trainer has focus.

The manual mainly details the assembly of the kit, however here is an index of interesting information related to the software and CPU.

| PDF Page | Manual Page | Section          | 
|:--------:|:-----------:|------------------|
| 47       |     45      | Operation        |
| 57       |     55      | Sample Programs  |
| 75       |     74      | Monitor Listing  |
| 89       |     87      | Memory Map       |
| 91       |     89      | Instruction Set  |

# Debugger

![image](https://user-images.githubusercontent.com/1910659/59240996-7aee3700-8c38-11e9-89bb-0823b2334343.png)

Press `CTRL-D` or click `View` > `Debugger` at the Trainer window to bring up the Debugger.

The Debugger has a raw memory display panel, a disassembly display panel and a status registers panel.

These panels visibility can be toggled with `CTRL+1`, `CTRL+2` and `CTRL+3` respectively.

At the top of the Debugger is a toolstrip menu. You can control execution with the buttons here. There are also two dropdowns labeled **Memory** and **Disassembly**. Selecting an item will update the memory or disassembly views with the raw or disassembled memory respectively at the preset address of the selected item.

| Label    |    Memory     | Description                                       | 
|:--------:|:-------------:|---------------------------------------------------|
| RAM      |    $0000      | Program RAM and Stack                             |
| Keypad   |    $C003      | Keypad mapped memory                              |
| Display  |    $C110      | 7-segment display mapped memory                   |
| ROM      |    $FC00      | Monitor ROM                                       |
# Breakpoints

When the disassembly view has focus, you can press `Up` and `Down` arrow keys to move the currently selected address. You can add a breakpoint at the current selected address by pressing `F9`. 

Pressing `F9` again when an address is highlighted and has a breakpoint will remove the breakpoint.

Pressing `CTRL+F9` will toggle whether the breakpoint is enabled or disabled.

The emulator will halt when the program counter reaches an address with an enabled breakpoint. 

# Execution Control

You can halt execution anytime by pressing `F4`. You can step through statements by pressing `F10` when execution is halted. 

The current instruction will be highlighted yellow.

The registers will update as each instruction is executed. You may resume execution by pressing `F5`.

# Memory Maps

Code and Data are often intermingled, causing code following the data to be misinterpreted by the disassembler.

You can add data ranges by using the memory maps tab at the bottom of the debugger window, or by right-clicking in the Disassembly panel.

Adding a Data Range will force the disassembler to skip the specified address range and display it as raw bytes.

![image](https://user-images.githubusercontent.com/1910659/59241975-5e53fe00-8c3c-11e9-8dca-070d9ebac68b.png)

Adding a Comment will add some text above the specified address.

Memory maps can be saved and loaded from the `File` menu. If a SREC file is loaded into RAM and a corresponding memory map is found, then the memory map will be loaded automatically.

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

This emulator is inspired by a lot of previous work, notably Marat Fayzullin's [How To Write a Computer Emulator](http://fms.komkon.org/EMUL8/HOWTO.html). It was originally written in Visual Basic 6, ported to C++, and finally C#.

The C# emulator core was originally written from scratch, but after discovering some bugs, it was rewritten using the MAME 6800 emulator source as reference.

# Thanks

I would like to thank Rick Nungester for taking interest in the project, testing the emulator and providing invaluable feedback.

# License

The emulator is made available under the MIT License.

# References

The core interpreter is based on the MAME 6800 CPU core.

https://github.com/mamedev/mame/blob/master/src/devices/cpu/m6800/m6800.cpp

