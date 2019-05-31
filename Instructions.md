# Sharp6800 Instruction Manual

## Introduction

Sharp6800 is an emulator written in .NET C#. The device it emulates is the ET-3400 Heathkit Microprocessor Trainer Kit that uses the Motorola 6800 CPU. This instruction manual assumes that the user is familiar with the ET-3400.

The original manual is available [here](http://archive.org/details/HeathkitManualForTheEt-3400MicroprocessorTrainer). 

Here is an index into the PDF of information that may be useful:

| PDF Page | Manual Page | Section          | 
|:--------:|:-----------:|------------------|
| 47       |     45      | Operation        |
| 57       |     55      | Sample Programs  |
| 75       |     74      | Monitor Listing  |
| 89       |     87      | Memory Map       |
| 91       |     89      | Instruction Set  |

The Monitor Listing contains the source code of the program that forms the "operating system" of the trainer kit that is programmed into a ROM. The ROM image is embedded in the emulator, so you do not need to find a copy of it to run the software.  

# Emulator Window

The Emulator Window contains a black screen with the letters H I N Z V C at the bottom. This emulates the 6-digit 7-segment display on a real ET-3400.  Below this is a set of 17 buttons that emulate the keypad on a ET-3400. You can interact with these buttons by clicking on them, however you can also press the corresponding keys (`0-9`, `A-F`) on your keyboard, with `ESC` being mapped to the `Reset` button.

On startup, the emulator will reset the 6800 CPU and execute the ROM, displaying the `CPU UP` message.

At this point you may press buttons on your keyboard or click on the virtual keypad to interact with the running program as you would a real ET-3400.  

## Menu

The top of the application window contains the following menu items.

### File

#### Load RAM

You may select a [Motorola S-record](https://en.wikipedia.org/wiki/SREC_(file_format)) or `SREC` file with the file extension `.OBJ` or `.S19` to load into RAM address space. 

Note that if your data writes to the memory area used by the stack, it may cause the ROM to crash. If this happens, reset the CPU by presses `ESC`. Avoid writing data to the stack starting at `$00EB`.

You may also load any other file by selecting `All files (*.*)` from the file extension filter dropdown. The file will be interpreted as a binary file and will be loaded verbatim into RAM starting at address `$0100`. This is to avoid inadvertently overwriting the stack.

#### Save RAM

You will be prompted for the address range to save. You may enter decimal or hexadecimal addresses. You may enter hex addresses by prefixing `$`.

The file will be saved in `SREC` format.

#### Recent

Clicking this menu item will show the last 10 files you have loaded as a sub menu list. You may clear this list by clicking `Clear Recent`

#### Load ROM

You may select a binary file to load as the ROM. It will be loaded into address space at the end of memory.  That is, if your ROM is 1KB in size (1024 bytes or `$400`) it will be loaded at `$10000` - `$400` = `$FC00`. This will replace the boot ROM and reset the CPU, causing the next cycle to fetch the address located in the last two bytes of memory and begin execution.  

#### Reset ROM

This will reload the default ROM and reset the CPU.

#### Load Map

Memory Maps is a feature that allows you to mark sections of memory as data blocks, or add a comment at as specific address. Marking a section of memory as a data block will inform the Debugger's disassembly view to treat that section as non-code, and avoid disassembling it. 

This will allow later code areas to be disassembled properly, as well as  prevent the view from "jumping around" as the contents of that section change during execution.

If an `.OBJ` or `.S19` file is loaded and a corresponding `.MAP` file exists, it will be loaded automatically.

#### Save Map

#### Exit

This will exit the emulator.

### View

This contains one sub menu, which will show the Debugger. You can press `CTRL-D` while in the Emulator window to bring up the Debugger.

### Emulation

#### Start

#### Stop

#### Step

#### Reset

#### Interrupts

#### Settings

#### Enable Breakpoints

### Help

# Debugger

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

# Appendix

## Keybaord Mapping

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
|   ESC    |  **RESET**    | Reset the CPU                                     |

## SREC File Format

An `SREC` file is a text file with the `.OBJ` or `.S19` looks like this: 

```
S11F00007F0007CE002AA600A72D088C003026F68D11BDFCBCBD005496074C9700
S11F001C07812526E220DDCE60000926FD390808080808083B7E3E050000793300
S11F00387E7E0000305B00003E6700007D153D0000051C15151015080808080800
S10E0054BDFE52000000000000803900
S11F00607F0086C620BDFDBB25F95A26F8C6208D38BDFDBB24F75A26F8BDFD8D00
S11F007C000000000080BDFDF4C686112714222ABDFD8D000000000E7E80CE6000
S11F0098000926FD20DBBDFD8D00003B4F5BA020B796864C9786811026037F0000
S11200B48639BDFD8D373000000000807E009600
```

Each line contains the following information

* `S1` - A line header, specifying that the address field is a 16-bit address
* `1F` - Two hex digits, indicating the number of bytes (hex digit pairs) that follow in the rest of the record (address + data + checksum).
* `0000` - Four hex digits, indicating the location where the following data will be stored. Arranged in big endian format.
* `7F00...` - The data to be stored. Each hex digit pair e.g. `7F` represents a byte.
* `00` - A checksum calculated as the least significant byte of ones' complement of the sum of the values represented by the hex digit pairs for the byte count, address and data fields. Currently, this is ignored.

### MAP File Format