The accompanying files contain the program listings printed in the HeathKit ET-3400 User Manual, 
compiled and converted to S19 format by Rick Nungester (wa6ndr) on the ET-3400 Yahoo Group

https://groups.yahoo.com/neo/groups/ET-3400/files/ET-3400%20Emulator%20and%20Files/

There are two versions for each sample file. The *a versions contain the original code, while the *b 
versions are modified to run about 13x faster for emulators that cannot run the programs at the 
"correct" speed.

These files can be loaded into the Sharp6800 through the File > Load RAM menu.

To execute a program load the corresponding file (or if it is already loaded, press RESET), press D to 
start the DO prompt and enter the RAM Start address for the corresponding program.

    +-----------------+----------------+-----------+-------------------------------------------------+
    |    Filename     |  Program Name  | RAM Start |                  Description                    |
    +-----------------+----------------+-----------+-------------------------------------------------+
    | samp123         | SAMPLE 1       | $0000     | Turn on and off each segment in the display     |
    |                 |                |           | beginning at H                                  |
    |                 +----------------+-----------+-------------------------------------------------+
    |                 | SAMPLE 2       | $0030     | Turns all displays on and off                   |
    |                 +----------------+-----------+-------------------------------------------------+
    |                 | SAMPLE 3       | $0060     | Outputs message by displaying up to six         | 
    |                 |                |           | character word one word at a time               |
    +-----------------+----------------+-----------+-------------------------------------------------+
    | samp45          | SAMPLE 4       | $0000     | Outputs same message as program 3 in ticker     |
    |                 |                |           | tape fashion                                    |
    |                 +----------------+-----------+-------------------------------------------------+
    |                 | SAMPLE 5       | $0060     | This program continuously changes the hex value |
    |                 |                |           | stored at KEY+1 until any hex key is depressed. |
    |                 |                |           | The right DP is lit to indicate a value has     |
    |                 |                |           | set. The user then depresses the various hex    |
    |                 |                |           | keys to look for the selected value. The        |
    |                 |                |           | relationship of depressed to correct key is     |
    |                 |                |           | momentarily displayed as HI or LO. DP again     |
    |                 |                |           | lights indicating try again. Depressing the     |
    |                 |                |           | correct key displays YES! which remains until   |
    |                 |                |           | any key id depressed setting a new value to     |
    |                 |                |           | find                                            |
    |-----------------+----------------+-----------+-------------------------------------------------+
    | samp6           | SAMPLE 6       | $0004     | This is a twelve hour clock program. The        |
    |                 |                |           | accuracy is dependent upon the MPU clock        |
    |                 |                |           | frequency and the timing loop at start.         |
    |                 |                |           | Changing the value at 0005/6 by hex 100         |
    |                 |                |           | changes the accuracy approximately 1 sec/min.   |
    |                 |                |           | Hours,minute,second RMB 0001/2/3 are loaded     |
    |                 |                |           | with the starting time. The first display is    |
    |                 |                |           | one second after start of the program.          |
    |                 |                |           | Seconds will be content of second rmb +1.       |
    |                 |                |           | Note:start the program at 0004.                 |
    +-----------------+----------------+-----------+-------------------------------------------------+
