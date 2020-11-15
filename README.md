# Introduction

The [ET-3400](http://www.oldcomputermuseum.com/heathkit_et3400.html) was a microprocessor trainer kit manufactured and sold by the Heath company in the 70's and 80's under the Heathkit brand. As a kit it was a collection of electronic  components, integrated circuits and a case and was designed to be assembled by the user. It was used to teach fundamentals of microprocessors at universities.  

The kit had the following features:

* It used the popular [Motorola 6800](http://en.wikipedia.org/wiki/Motorola_6800) 8-bit CPU clocked at 100kHz
* Six 7-segment LED displays
* 17 push-button switches arranged as a hex keypad
* 8 LEDs for general purpose visual output
* 1 8-position DIP switch for data input
* 1KB ROM
* 256 bytes RAM (expandable to 512 bytes)
* buffered 8-bit data and 16-bit address ports
* Breadboard for placing additional ICs and components

The ROM contained a program that controlled display and keyboard, and allowed the user to enter programs into RAM (one hex byte at a time) and execute them. The ROM also had the capability to add up to 4 breakpoints, step through the program and view register contents using the 7-segment LEDs.

This project simulates the ET-3400 by emulating the 6800 CPU, 7-segment displays and keypad. The ROM program is included.

The project was previously named sharp6800, as it was built around a 6800 CPU interpreter core written in C#. 

# Usage

Upon starting the application, the emulator boot into the ROM (Monitor program), displaying the familiar `CPU UP` message.

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

The emulator comes with a fully-featured debugger.

Press `CTRL-D` or click `View` > `Debugger` at the Trainer window to bring up the Debugger.

![image](https://user-images.githubusercontent.com/1910659/59240996-7aee3700-8c38-11e9-89bb-0823b2334343.png)

The Debugger has a Memory display panel, a Disassembly display panel and a Status registers panel. These panels visibility can be toggled with `CTRL+1`, `CTRL+2` and `CTRL+3` respectively.

At the top of the Debugger is a toolstrip menu. You can control execution with the **Start**, **Stop**, **Step** and **Reset** buttons here. 

Next to the execution control buttons there are the **Memory** and **Disassembly shorcuts**.

Selecting an item from these dropdowns will update memory or disassembly views with the raw or disassembled memory starting at the preset address of the selected item.

The displays will update in real time as the CPU executes instructions.

The preset addresses are as follows:

| Name    |    Address     | Description                                       | 
|:--------:|:-------------:|---------------------------------------------------|
| RAM      |    $0000      | Program RAM and Stack                             |
| Keypad   |    $C003      | Keypad mapped memory                              |
| Display  |    $C110      | 7-segment display mapped memory                   |
| ROM      |    $FC00      | Monitor ROM                                       |

Selecting Keypad you can see how 3 bytes are mapped to the 16 keys in the hex keypad as they are pressed.

Selecting Display, you can see that the ROM writes bit-shifted data to 8 addresses for each digit in the display, even though only one byte is used for the actual contents of each digit.

## Memory Panel

The Memory Panel shows raw memory in rows of 8 bytes from the currently selected memory start address. Click within the panel to give focus to it.

While it has focus, you can use the **Up**, **Down**, **Page Up** and **Page Down** keys to navigate and set the currently selected address.

You can press **CTRL-C** to copy the row of memory at the currently selected address.

Pressing **F2** will halt execution and allow you to modify memory starting at the currently selected address.


## Disassembly Panel

The Disassembly Panel shows disassembled memory starting from the currently selected disassembly start address. Click within the panel to give focus to it.

While it has focus, you can use the **Up**, **Down**, **Page Up** and **Page Down** keys to navigate and set the currently selected address.

You can press **CTRL-C** to copy the disassembly at the current selected address.

Pressing **F2** will halt execution and allow you to modify memory starting at the currently selected address.

**NOTE**: If you modify ROM and make the CPU crash or otherwise unusable, you can reset the ROM by selecting **File** > **Reset ROM**.

### Breakpoints

You can add a breakpoint at the currently selected address by pressing `F9`. 

Pressing `F9` again when an address is highlighted and has a breakpoint will remove the breakpoint.

Pressing `CTRL+F9` will toggle whether the breakpoint is enabled or disabled.

The emulator will halt when the program counter reaches an address with an enabled breakpoint. 

### Execution Control

You can halt execution anytime by pressing `F4`. You can step through statements by pressing `F10` when execution is halted. 

The current instruction will be highlighted yellow.

The registers will update as each instruction is executed. You may resume execution by pressing `F5`.


### Memory Maps

When writing in machine language or assembly, code and data are often intermingled. The disassembler cannot know which is code and which is data. As a result data immediately following code will be treated as instructions and operands to be disassembled. 

As disassembly depends on the context and arbitrary bytes change the context,there is a possibility that any legitimate code following the data may  be incorrectly disassembled.

**Memory maps** allow you to specify which parts of memory are data.

You can add memory maps by using the Memory Maps tab at the bottom of the debugger window, or by right-clicking in the Disassembly panel.

### Data Ranges

Adding a Data Range will force the disassembler to skip the specified address range and display it as raw bytes. In the image below a Data Range was added starting from address `$0011`.

Note that breakpoints cannot be placed within addresses mapped to Data Ranges.

![image](https://user-images.githubusercontent.com/1910659/59241975-5e53fe00-8c3c-11e9-8dca-070d9ebac68b.png)

### Comments

Adding a Comment will add some text above the specified address without affecting the disassembly of the instructions at the address. 

Memory maps can be saved and loaded from the `File` menu. Memory Maps are plain text files with comma separated values and have the `.MAP` extension

 If a SREC file is loaded into RAM and a corresponding memory map is found the memory map will be loaded automatically.

# SREC (.OBJ) file format

The emulator supports Motorola SREC/S19 format files with the `.OBJ` extension. Select `File` > `Load RAM` from the menu and select your `.OBJ` file to load it into memory.

You can also save RAM (or any memory range) to SREC format by clicking `File` > `Save RAM`. You will be prompted for a memory range, and then prompted for a location to save the file.

# Quick ET-3400 Guide

## Boot Process

When the RESET pin on a 6800 is pulled low, the CPU fetches the address at the last two bytes of memory and begins execution from there. The ROM (the program listing/source code of which can be found in the manual) is 1024 bytes and mapped to `$FC00`, i.e. logic gates on the ET-3400 board are set so that the CPU is selected or active when the value on the address lines are `$FC00`-`$FFFF`, or when the 6 most significant bits of the address bus are set to 1.

```
1111 11XX XXXX XXXX
```

The last two bytes in the ROM are `FC` `00` and point to the beginning of mapped ROM memory

The first thing that the ROM does is load the stack pointer register with a 16-bit value, `$00EB`.

```
$FC00:  8E 00 EB    lds #$00EB
```

It then jumps to subroutine `$FD8D`, the Monitor's main loop.

```
$FC03:  BD FD 8D    jst #$FD8D
```

You can trace the boot process by setting a breakpoint at `$FC00` and pressing `ESC` or otherwise resetting the CPU.

# Key mapping


| PC |    ET3400     | Action                                            | 
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
|    C     |  **CHAN**     | During Examine mode, edit hex value at specified address. <br/> During ACCA/ACCB/PC mode, edit hex value in selected register |
|    D     |  **DO**       | Execute RAM at given address                      |
|    E     |  **EXAM**     | Start viewing hex at specified address            |
|    F     |  **FWD**      | During Examine mode, move address forward         |
|   ESC    |  **RESET**    | Reset the CPU         |

# Debugger Keys

| Key       | Action                                  |
|-----------|-----------------------------------------|
| F4        |  Halt execution                         |
| F5        |  Resume execution                       |
| F9        |  Toggle Breakpoint ON/OFF               |
| CTRL+F9   |  Toggle Breakpoint Enabled/Disabled     |
| F10       |  Step into next instruction             |

# History

This emulator is inspired by a lot of previous work, notably Marat Fayzullin's [How To Write a Computer Emulator](http://fms.komkon.org/EMUL8/HOWTO.html). It was originally written in Visual Basic 6, ported to C++, and finally C#.

The C# emulator core was originally written from scratch, but after discovering some bugs, it was rewritten using the MAME 6800 emulator source as reference.

# Thanks

I would like to thank Rick Nungester for taking interest in the project, testing the emulator and providing invaluable feedback.

# License

The emulator is made available under the BSD 2-Clause.

# References

The core interpreter is based on the MAME 6800 CPU core.

https://github.com/mamedev/mame/blob/master/src/devices/cpu/m6800/m6800.cpp

