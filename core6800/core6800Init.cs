namespace Core6800
{
    public partial class Cpu6800
    {
        const int IMMEDIATE = 1;
        const int DIRECT = 2;
        const int INDEXED = 3;
        const int EXTENDED = 4;
        const int INHERENT = 5;
        const int RELATIVE = 6;

        public void Initialize()
        {
            NMI = 1;
            IRQ = 1;

            //============ LOGIC OPCODES ==========
            //ADDA
            Cycles[0x8B] = 2; AddrMode[0x8B] = IMMEDIATE;
            Cycles[0x9B] = 3; AddrMode[0x9B] = DIRECT;
            Cycles[0xAB] = 5; AddrMode[0xAB] = INDEXED;
            Cycles[0xBB] = 4; AddrMode[0xBB] = EXTENDED;
            //ADDB
            Cycles[0xCB] = 2; AddrMode[0xCB] = IMMEDIATE;
            Cycles[0xDB] = 3; AddrMode[0xDB] = DIRECT;
            Cycles[0xEB] = 5; AddrMode[0xEB] = INDEXED;
            Cycles[0xFB] = 4; AddrMode[0xFB] = EXTENDED;
            //ABA
            Cycles[0x1B] = 2; AddrMode[0x1B] = INHERENT;
            //ADCA
            Cycles[0x89] = 2; AddrMode[0x89] = IMMEDIATE;
            Cycles[0x99] = 3; AddrMode[0x99] = DIRECT;
            Cycles[0xA9] = 5; AddrMode[0xA9] = INDEXED;
            Cycles[0xB9] = 4; AddrMode[0xB9] = EXTENDED;
            //ADCB
            Cycles[0xC9] = 2; AddrMode[0xC9] = IMMEDIATE;
            Cycles[0xD9] = 3; AddrMode[0xD9] = DIRECT;
            Cycles[0xE9] = 5; AddrMode[0xE9] = INDEXED;
            Cycles[0xF9] = 4; AddrMode[0xF9] = EXTENDED;
            //ANDA
            Cycles[0x84] = 2; AddrMode[0x84] = IMMEDIATE;
            Cycles[0x94] = 3; AddrMode[0x94] = DIRECT;
            Cycles[0xA4] = 5; AddrMode[0xA4] = INDEXED;
            Cycles[0xB4] = 4; AddrMode[0xB4] = EXTENDED;
            //ANDB
            Cycles[0xC4] = 2; AddrMode[0xC4] = IMMEDIATE;
            Cycles[0xD4] = 3; AddrMode[0xD4] = DIRECT;
            Cycles[0xE4] = 5; AddrMode[0xE4] = INDEXED;
            Cycles[0xF4] = 4; AddrMode[0xF4] = EXTENDED;
            //BITA
            Cycles[0x85] = 2; AddrMode[0x85] = IMMEDIATE;
            Cycles[0x95] = 3; AddrMode[0x95] = DIRECT;
            Cycles[0xA5] = 5; AddrMode[0xA5] = INDEXED;
            Cycles[0xB5] = 4; AddrMode[0xB5] = EXTENDED;
            //BITB
            Cycles[0xC5] = 2; AddrMode[0xC5] = IMMEDIATE;
            Cycles[0xD5] = 3; AddrMode[0xD5] = DIRECT;
            Cycles[0xE5] = 5; AddrMode[0xE5] = INDEXED;
            Cycles[0xF5] = 4; AddrMode[0xF5] = EXTENDED;
            //CLR
            Cycles[0x6F] = 7; AddrMode[0x6F] = INDEXED;
            Cycles[0x7F] = 6; AddrMode[0x7F] = EXTENDED;
            //CLRA
            Cycles[0x4F] = 2; AddrMode[0x4F] = INHERENT;
            //CLRB
            Cycles[0x5F] = 2; AddrMode[0x5F] = INHERENT;
            //CMPA
            Cycles[0x81] = 2; AddrMode[0x81] = IMMEDIATE;
            Cycles[0x91] = 3; AddrMode[0x91] = DIRECT;
            Cycles[0xA1] = 5; AddrMode[0xA1] = INDEXED;
            Cycles[0xB1] = 4; AddrMode[0xB1] = EXTENDED;
            //CMPA
            Cycles[0xC1] = 2; AddrMode[0xC1] = IMMEDIATE;
            Cycles[0xD1] = 3; AddrMode[0xD1] = DIRECT;
            Cycles[0xE1] = 5; AddrMode[0xE1] = INDEXED;
            Cycles[0xF1] = 4; AddrMode[0xF1] = EXTENDED;
            //CBA
            Cycles[0x11] = 2; AddrMode[0x11] = INHERENT;
            //COM
            Cycles[0x63] = 7; AddrMode[0x63] = INDEXED;
            Cycles[0x73] = 6; AddrMode[0x73] = EXTENDED;
            //COMA
            Cycles[0x43] = 2; AddrMode[0x43] = INHERENT;
            //COMB
            Cycles[0x53] = 2; AddrMode[0x53] = INHERENT;
            //NEG
            Cycles[0x60] = 7; AddrMode[0x60] = INDEXED;
            Cycles[0x70] = 6; AddrMode[0x70] = EXTENDED;
            //NEGA
            Cycles[0x40] = 2; AddrMode[0x40] = INHERENT;
            //NEGB
            Cycles[0x50] = 2; AddrMode[0x50] = INHERENT;
            //DAA
            Cycles[0x19] = 2; AddrMode[0x19] = INHERENT;
            //DEC
            Cycles[0x6A] = 7; AddrMode[0x6A] = INDEXED;
            Cycles[0x7A] = 6; AddrMode[0x7A] = EXTENDED;
            //DECA
            Cycles[0x4A] = 2; AddrMode[0x4A] = INHERENT;
            //DECB
            Cycles[0x5A] = 2; AddrMode[0x5A] = INHERENT;
            //EORA
            Cycles[0x88] = 2; AddrMode[0x88] = IMMEDIATE;
            Cycles[0x98] = 3; AddrMode[0x98] = DIRECT;
            Cycles[0xA8] = 5; AddrMode[0xA8] = INDEXED;
            Cycles[0xB8] = 4; AddrMode[0xB8] = EXTENDED;
            //EORB
            Cycles[0xC8] = 2; AddrMode[0xC8] = IMMEDIATE;
            Cycles[0xD8] = 3; AddrMode[0xD8] = DIRECT;
            Cycles[0xE8] = 5; AddrMode[0xE8] = INDEXED;
            Cycles[0xF8] = 4; AddrMode[0xF8] = EXTENDED;
            //INC
            Cycles[0x6C] = 7; AddrMode[0x6C] = INDEXED;
            Cycles[0x7C] = 6; AddrMode[0x7C] = EXTENDED;
            //INCA
            Cycles[0x4C] = 2; AddrMode[0x4C] = INHERENT;
            //INCB
            Cycles[0x5C] = 2; AddrMode[0x5C] = INHERENT;
            //LDAA
            Cycles[0x86] = 2; AddrMode[0x86] = IMMEDIATE;
            Cycles[0x96] = 3; AddrMode[0x96] = DIRECT;
            Cycles[0xA6] = 5; AddrMode[0xA6] = INDEXED;
            Cycles[0xB6] = 4; AddrMode[0xB6] = EXTENDED;
            //LDAB
            Cycles[0xC6] = 2; AddrMode[0xC6] = IMMEDIATE;
            Cycles[0xD6] = 3; AddrMode[0xD6] = DIRECT;
            Cycles[0xE6] = 5; AddrMode[0xE6] = INDEXED;
            Cycles[0xF6] = 4; AddrMode[0xF6] = EXTENDED;
            //ORA
            Cycles[0x8A] = 2; AddrMode[0x8A] = IMMEDIATE;
            Cycles[0x9A] = 3; AddrMode[0x9A] = DIRECT;
            Cycles[0xAA] = 5; AddrMode[0xAA] = INDEXED;
            Cycles[0xBA] = 4; AddrMode[0xBA] = EXTENDED;
            //ORB
            Cycles[0xCA] = 2; AddrMode[0xCA] = IMMEDIATE;
            Cycles[0xDA] = 3; AddrMode[0xDA] = DIRECT;
            Cycles[0xEA] = 5; AddrMode[0xEA] = INDEXED;
            Cycles[0xFA] = 4; AddrMode[0xFA] = EXTENDED;
            //PSHA
            Cycles[0x36] = 4; AddrMode[0x36] = INHERENT;
            //PSHB
            Cycles[0x37] = 4; AddrMode[0x37] = INHERENT;
            //PULA
            Cycles[0x32] = 4; AddrMode[0x32] = INHERENT;
            //PULB
            Cycles[0x33] = 4; AddrMode[0x33] = INHERENT;
            //ROL
            Cycles[0x69] = 7; AddrMode[0x69] = INDEXED;
            Cycles[0x79] = 6; AddrMode[0x79] = EXTENDED;
            //ROLA
            Cycles[0x49] = 2; AddrMode[0x49] = INHERENT;
            //ROLB
            Cycles[0x59] = 2; AddrMode[0x59] = INHERENT;
            //ROR
            Cycles[0x66] = 7; AddrMode[0x66] = INDEXED;
            Cycles[0x76] = 6; AddrMode[0x76] = EXTENDED;
            //RORA
            Cycles[0x46] = 2; AddrMode[0x46] = INHERENT;
            //RORB
            Cycles[0x56] = 2; AddrMode[0x56] = INHERENT;
            //ASL
            Cycles[0x68] = 7; AddrMode[0x68] = INDEXED;
            Cycles[0x78] = 6; AddrMode[0x78] = EXTENDED;
            //ASLA
            Cycles[0x48] = 2; AddrMode[0x48] = INHERENT;
            //ASLB
            Cycles[0x58] = 2; AddrMode[0x58] = INHERENT;
            //ASR
            Cycles[0x67] = 7; AddrMode[0x67] = INDEXED;
            Cycles[0x77] = 6; AddrMode[0x77] = EXTENDED;
            //ASRA
            Cycles[0x47] = 2; AddrMode[0x47] = INHERENT;
            //ASRB
            Cycles[0x57] = 2; AddrMode[0x57] = INHERENT;
            //LSR
            Cycles[0x64] = 7; AddrMode[0x64] = INDEXED;
            Cycles[0x74] = 6; AddrMode[0x74] = EXTENDED;
            //LSRA
            Cycles[0x44] = 2; AddrMode[0x44] = INHERENT;
            //LSRB
            Cycles[0x54] = 2; AddrMode[0x54] = INHERENT;
            //STA
            Cycles[0x97] = 4; AddrMode[0x97] = DIRECT;
            Cycles[0xA7] = 6; AddrMode[0xA7] = INDEXED;
            Cycles[0xB7] = 5; AddrMode[0xB7] = EXTENDED;
            //STB
            Cycles[0xD7] = 4; AddrMode[0xD7] = DIRECT;
            Cycles[0xE7] = 6; AddrMode[0xE7] = INDEXED;
            Cycles[0xF7] = 5; AddrMode[0xF7] = EXTENDED;
            //SUBA
            Cycles[0x80] = 2; AddrMode[0x80] = IMMEDIATE;
            Cycles[0x90] = 3; AddrMode[0x90] = DIRECT;
            Cycles[0xA0] = 5; AddrMode[0xA0] = INDEXED;
            Cycles[0xB0] = 4; AddrMode[0xB0] = EXTENDED;
            //SUBB
            Cycles[0xC0] = 2; AddrMode[0xC0] = IMMEDIATE;
            Cycles[0xD0] = 3; AddrMode[0xD0] = DIRECT;
            Cycles[0xE0] = 5; AddrMode[0xE0] = INDEXED;
            Cycles[0xF0] = 4; AddrMode[0xF0] = EXTENDED;
            //SBA
            Cycles[0x10] = 2; AddrMode[0x10] = INHERENT;
            //SBCA
            Cycles[0x82] = 2; AddrMode[0x82] = IMMEDIATE;
            Cycles[0x92] = 3; AddrMode[0x92] = DIRECT;
            Cycles[0xA2] = 5; AddrMode[0xA2] = INDEXED;
            Cycles[0xB2] = 4; AddrMode[0xB2] = EXTENDED;
            //SBCB
            Cycles[0xC2] = 2; AddrMode[0xC2] = IMMEDIATE;
            Cycles[0xD2] = 3; AddrMode[0xD2] = DIRECT;
            Cycles[0xE2] = 5; AddrMode[0xE2] = INDEXED;
            Cycles[0xF2] = 4; AddrMode[0xF2] = EXTENDED;
            //TAB
            Cycles[0x16] = 2; AddrMode[0x16] = INHERENT;
            //TBA
            Cycles[0x17] = 2; AddrMode[0x17] = INHERENT;
            //TST
            Cycles[0x6D] = 7; AddrMode[0x6D] = INDEXED;
            Cycles[0x7D] = 6; AddrMode[0x7D] = EXTENDED;
            //TSTA
            Cycles[0x4D] = 2; AddrMode[0x4D] = INHERENT;
            //TSTB
            Cycles[0x5D] = 2; AddrMode[0x5D] = INHERENT;

            //============ INDEX REGISTER AND STACK OPCODES ==========
            //CPX
            Cycles[0x8C] = 3; AddrMode[0x8C] = IMMEDIATE;
            Cycles[0x9C] = 4; AddrMode[0x9C] = DIRECT;
            Cycles[0xAC] = 6; AddrMode[0xAC] = INDEXED;
            Cycles[0xBC] = 5; AddrMode[0xBC] = EXTENDED;
            //DEX
            Cycles[0x09] = 4; AddrMode[0x09] = INHERENT;
            //DES
            Cycles[0x34] = 4; AddrMode[0x34] = INHERENT;
            //INX
            Cycles[0x08] = 4; AddrMode[0x08] = INHERENT;
            //INS
            Cycles[0x31] = 4; AddrMode[0x31] = INHERENT;
            //LDX
            Cycles[0xCE] = 3; AddrMode[0xCE] = IMMEDIATE;
            Cycles[0xDE] = 4; AddrMode[0xDE] = DIRECT;
            Cycles[0xEE] = 6; AddrMode[0xEE] = INDEXED;
            Cycles[0xFE] = 5; AddrMode[0xFE] = EXTENDED;
            //LDS
            Cycles[0x8E] = 3; AddrMode[0x8E] = IMMEDIATE;
            Cycles[0x9E] = 4; AddrMode[0x9E] = DIRECT;
            Cycles[0xAE] = 6; AddrMode[0xAE] = INDEXED;
            Cycles[0xBE] = 5; AddrMode[0xBE] = EXTENDED;
            //STX
            Cycles[0xDF] = 5; AddrMode[0xDF] = DIRECT;
            Cycles[0xEF] = 7; AddrMode[0xEF] = INDEXED;
            Cycles[0xFF] = 6; AddrMode[0xFF] = EXTENDED;
            //STS
            Cycles[0x9F] = 5; AddrMode[0x9F] = DIRECT;
            Cycles[0xAF] = 7; AddrMode[0xAF] = INDEXED;
            Cycles[0xBF] = 6; AddrMode[0xBF] = EXTENDED;
            //TXS
            Cycles[0x35] = 4; AddrMode[0x35] = INHERENT;
            //TSX
            Cycles[0x30] = 4; AddrMode[0x30] = INHERENT;

            //============ JUMP AND BRANCH OPCODES ==========
            //BRA
            Cycles[0x20] = 4; AddrMode[0x20] = RELATIVE;
            //BCC
            Cycles[0x24] = 4; AddrMode[0x24] = RELATIVE;
            //BCS
            Cycles[0x25] = 4; AddrMode[0x25] = RELATIVE;
            //BEQ
            Cycles[0x27] = 4; AddrMode[0x27] = RELATIVE;
            //BGE
            Cycles[0x2C] = 4; AddrMode[0x2C] = RELATIVE;
            //BGT
            Cycles[0x2E] = 4; AddrMode[0x2E] = RELATIVE;
            //BHI
            Cycles[0x22] = 4; AddrMode[0x22] = RELATIVE;
            //BLE
            Cycles[0x2F] = 4; AddrMode[0x2F] = RELATIVE;
            //BLS
            Cycles[0x23] = 4; AddrMode[0x23] = RELATIVE;
            //BLT
            Cycles[0x2D] = 4; AddrMode[0x2D] = RELATIVE;
            //BMI
            Cycles[0x2B] = 4; AddrMode[0x2B] = RELATIVE;
            //BNE
            Cycles[0x26] = 4; AddrMode[0x26] = RELATIVE;
            //BVC
            Cycles[0x28] = 4; AddrMode[0x28] = RELATIVE;
            //BVS
            Cycles[0x29] = 4; AddrMode[0x29] = RELATIVE;
            //BPL
            Cycles[0x2A] = 4; AddrMode[0x2A] = RELATIVE;
            //BSR
            Cycles[0x8D] = 8; AddrMode[0x8D] = RELATIVE;
            //JMP
            Cycles[0x6E] = 4; AddrMode[0x6E] = INDEXED;
            Cycles[0x7E] = 3; AddrMode[0x7E] = EXTENDED;
            //JSR
            Cycles[0xAD] = 8; AddrMode[0xAD] = INDEXED;
            Cycles[0xBD] = 9; AddrMode[0xBD] = EXTENDED;
            //NOP
            Cycles[0x01] = 2; AddrMode[0x01] = INHERENT;
            //RTI
            Cycles[0x3B] = 10; AddrMode[0x3B] = INHERENT;
            //RTS
            Cycles[0x39] = 5; AddrMode[0x39] = INHERENT;
            //SWI
            Cycles[0x3F] = 12; AddrMode[0x3F] = INHERENT;
            //WAI
            Cycles[0x3E] = 9; AddrMode[0x3E] = INHERENT;

            //============ CONDITIONS CODE REGISTER OPCODES ==========
            //CLC
            Cycles[0xC] = 2; AddrMode[0xC] = INHERENT;
            //CLI
            Cycles[0xE] = 2; AddrMode[0xE] = INHERENT;
            //CLV
            Cycles[0xA] = 2; AddrMode[0xA] = INHERENT;
            //SEC
            Cycles[0xD] = 2; AddrMode[0xD] = INHERENT;
            //SEI
            Cycles[0xF] = 2; AddrMode[0xF] = INHERENT;
            //SEV
            Cycles[0xB] = 2; AddrMode[0xB] = INHERENT;
            //TAP
            Cycles[0x6] = 2; AddrMode[0x6] = INHERENT;
            //TPA
            Cycles[0x7] = 2; AddrMode[0x7] = INHERENT;
        }
    }
}
