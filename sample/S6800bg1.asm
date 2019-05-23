; This 6800 assembly code demonstrates a defect in the Sharp6800
; emulator (v1.0.3.0) of the Healthkit ET-3400 Microprocessor
; Trainer.  Load the 6 bytes into the emulator (AUTO 0000
; 01 39 01 8D FB 39), RESET, then follow the 3 steps below.
; After Step 3, the real ET-3400 correctly shows "0000 01",
; having Single-Stepped to label SUB1.  The emulator shows
; "0100 00".

        .org    $0000

SUB1    NOP             ; Step 3: Single-Step (SS) to here.
        RTS

MAIN    NOP             ; Step 2: Execute here (DO 0002, see "0003 8D")
        BSR     SUB1    ; Step 1: Set a breakpoint here (BR 0003)
        RTS

        .END
