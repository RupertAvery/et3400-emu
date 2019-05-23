;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; 6800 assembly code to convert binary to BCD, by Rick Nungester 1/6/15.
; Input value n is limited to the range [0,255].  The "Double Dabble"
; algorithm is explained at en.wikipedia.org/wiki/Double_dabble.  This
; program is intended to be entered and stepped through on the Heathkit
; ET-3400 Microprocessor Trainer or its "Sharp6800.codeplex.com" emulator.

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ET-3400 constants

USRRAML .equ    $0000   ; first byte of user RAM
USRRAMH .equ    $00C4   ; last byte of user RAM (197 bytes total)

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; Variables

        .org  USRRAMH-5 ; put vars at the end of user RAM

BCDIN   .block  1       ; BIN2BCD input value (0-255)
BCD100S .block  1       ; BIN2BCD 100s output digit (0-2)
BCD10S  .block  1       ; BIN2BCD 10s output digit (0-9)
BCD1S   .block  1       ; BIN2BCD 10s output digit (0-9)
BCD10_1 .block  1       ; BIN2BCD mid-routine 10s (MS nibbble) & 1s digit
BCDIDX  .block  1       ; BIN2BCD loop counter

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; Main program

        .org    USRRAML ; put code at the start of user RAM

MAIN    LDAA    #243    ; Test case (as used in the Wikipedia article)
        BSR     BIN2BCD
        RTS

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; BIN2BCD converts the value in ACC A (0 to 255) to BCD in variables
; BCD100S, BCD10S and BCD1S.  See en.wikipedia.org/wiki/Double_dabble
; for the "Double Dabble" algorithm being used.

BIN2BCD STAA    BCDIN   ; Initialization. BCDIN = binary value to convert.
        CLRA
        STAA    BCD100S ; BCD 100s digit = 0
        STAA    BCD10_1 ; BCD 10s digit (MS nibble) and 1s digits = 0
        LDAA    #8
        STAA    BCDIDX  ; Loop counter = 8 (8-bit unsigned input)

; Top of main loop.  Test each of the 3 BCD digits and if > 4, add 3.
; REVISIT: Avoid the Sharp6800 v1.0.3.0 defect that BLS is mis-implemented.
; Instead of testing for <= 4 (BLS), test for < 5 (BCS).

BIN01   LDAA    BCD100S
        CMPA    #5
        BCS     BIN02   ; branch if BCD 100s digit < 5

        LDAA    #3      ; BCD 100s digit > 4 so add 3
        ADDA    BCD100S
        STAA    BCD100S

BIN02   LDAA    BCD10_1
        ANDA    #$F0    ; ACCA = BCD 10s digit (in MS nibble)
        CMPA    #$50
        BCS     BIN03   ; branch if BCD 10s digit < 5

        LDAA    #$30    ; BCD 10s digit > 4 so add 3
        ADDA    BCD10_1
        STAA    BCD10_1

BIN03   LDAA    BCD10_1
        ANDA    #$0F    ; ACCA = BCD 1s digit (in LS nibble)
        CMPA    #5
        BCS     BIN04   ; branch if BCD 1s digit < 5

        LDAA    #3      ; BCD 1s digit > 4 so add 3
        ADDA    BCD10_1
        STAA    BCD10_1

BIN04   ASL     BCDIN   ; left-shift BCD100S:BCD10_1:BCDIN (3 bytes)
        ROL     BCD10_1
        ROL     BCD100S

        DEC     BCDIDX  ; Done with 8 passes?
        BNE     BIN01   ; If not, branch up and do another

; Split BCD10_1 into separate BCD10S and BCD1S digits.

        LDAA    BCD10_1
        ANDA    #$0F
        STAA    BCD1S   ; BCD1S = final BCD 1s digit
        
        LDAA    BCD10_1 ; REVISIT: ACCA = $43 (good)
        LSRA            ; REVISIT: ACCA = $21 (good)
        LSRA            ; REVISIT: ACCA = $90 (BAD, should be $10)
        LSRA            ; REVISIT: ACCA = $48 (BAD, should be $80)
        LSRA            ; REVISIT: ACCA = $24 (BAD, should be $40)
        STAA    BCD10S  ; BCD10S = final BCD 10s digit
        
        RTS             ; Done

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; Required TASM assembler directive

        .END            ; required TASM assembler directive
