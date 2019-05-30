using System.Collections;
using System.Diagnostics;

namespace Sharp6800.Debugger
{
    public class Disassembly
    {
        public string Operand { get; set; }
        public string Arguments { get; set; }
        public int Length;
        public byte[] Bytes { get; set; }
    }

    public struct DasmResult
    {
        public bool Illegal { get; set; }
        public string Operand { get; set; }
        public string Instruction { get; set; }
        public int Flags { get; set; }
        public int ByteLength { get; set; }
    }

    public class Disassembler
    {
        private const int inh = 1; /* inherent */
        private const int rel = 2; /* relative */
        private const int imb = 3; /* immediate (byte) */
        private const int imw = 4; /* immediate (word) */
        private const int dir = 5; /* direct address */
        private const int imd = 6; /* HD63701YO: immediate, direct address */
        private const int ext = 7; /* extended address */
        private const int idx = 8; /* x + byte offset */
        private const int imx = 9; /* HD63701YO: immediate, x + byte offset */
        private const int sx1 = 10; /* HD63701YO: undocumented opcodes: byte from (s+1) */

        const int aba = 0;
        const int abx = 1;
        const int adca = 2;
        const int adcb = 3;
        const int adda = 4;
        const int addb = 5;
        const int addd = 6;
        const int aim = 7;
        const int anda = 8;
        const int andb = 9;
        const int asl = 10;
        const int asla = 11;
        const int aslb = 12;
        const int asld = 13;
        const int asr = 14;
        const int asra = 15;
        const int asrb = 16;
        const int bcc = 17;
        const int bcs = 18;
        const int beq = 19;
        const int bge = 20;
        const int bgt = 21;
        const int bhi = 22;
        const int bita = 23;
        const int bitb = 24;
        const int ble = 25;
        const int bls = 26;
        const int blt = 27;
        const int bmi = 28;
        const int bne = 29;
        const int bpl = 30;
        const int bra = 31;
        const int brn = 32;
        const int bsr = 33;
        const int bvc = 34;
        const int bvs = 35;
        const int cba = 36;
        const int clc = 37;
        const int cli = 38;
        const int clr = 39;
        const int clra = 40;
        const int clrb = 41;
        const int clv = 42;
        const int cmpa = 43;
        const int cmpb = 44;
        const int cmpx = 45;
        const int com = 46;
        const int coma = 47;
        const int comb = 48;
        const int daa = 49;
        const int dec = 50;
        const int deca = 51;
        const int decb = 52;
        const int des = 53;
        const int dex = 54;
        const int eim = 55;
        const int eora = 56;
        const int eorb = 57;
        const int ill = 58;
        const int inc = 59;
        const int inca = 60;
        const int incb = 61;
        const int ins = 62;
        const int inx = 63;
        const int jmp = 64;
        const int jsr = 65;
        const int lda = 66;
        const int ldb = 67;
        const int ldd = 68;
        const int lds = 69;
        const int ldx = 70;
        const int lsr = 71;
        const int lsra = 72;
        const int lsrb = 73;
        const int lsrd = 74;
        const int mul = 75;
        const int neg = 76;
        const int nega = 77;
        const int negb = 78;
        const int nop = 79;
        const int oim = 80;
        const int ora = 81;
        const int orb = 82;
        const int psha = 83;
        const int pshb = 84;
        const int pshx = 85;
        const int pula = 86;
        const int pulb = 87;
        const int pulx = 88;
        const int rol = 89;
        const int rola = 90;
        const int rolb = 91;
        const int ror = 92;
        const int rora = 93;
        const int rorb = 94;
        const int rti = 95;
        const int rts = 96;
        const int sba = 97;
        const int sbca = 98;
        const int sbcb = 99;
        const int sec = 100;
        const int sev = 101;
        const int sta = 102;
        const int stb = 103;
        const int _std = 104;
        const int sei = 105;
        const int sts = 106;
        const int stx = 107;
        const int suba = 108;
        const int subb = 109;
        const int subd = 110;
        const int swi = 111;
        const int wai = 112;
        const int tab = 113;
        const int tap = 114;
        const int tba = 115;
        const int tim = 116;
        const int tpa = 117;
        const int tst = 118;
        const int tsta = 119;
        const int tstb = 120;
        const int tsx = 121;
        const int txs = 122;
        const int asx1 = 123;
        const int asx2 = 124;
        const int xgdx = 125;
        const int addx = 126;
        const int adcx = 127;
        const int nba = 128;

        private static string[] op_name_str_orig =
            {
                "aba",  "abx",  "adca", "adcb", "adda", "addb", "addd", "aim",
                "anda", "andb", "asl",  "asla", "aslb", "asld", "asr",  "asra",
                "asrb", "bcc",  "bcs",  "beq",  "bge",  "bgt",  "bhi",  "bita",
                "bitb", "ble",  "bls",  "blt",  "bmi",  "bne",  "bpl",  "bra",
                "brn",  "bsr",  "bvc",  "bvs",  "cba",  "clc",  "cli",  "clr",
                "clra", "clrb", "clv",  "cmpa", "cmpb", "cmpx", "com",  "coma",
                "comb", "daa",  "dec",  "deca", "decb", "des",  "dex",  "eim",
                "eora", "eorb", "illegal", "inc", "inca", "incb", "ins", "inx",
                "jmp",  "jsr",  "lda",  "ldb",  "ldd",  "lds",  "ldx",  "lsr",
                "lsra", "lsrb", "lsrd", "mul",  "neg",  "nega", "negb", "nop",
                "oim",  "ora",  "orb",  "psha", "pshb", "pshx", "pula", "pulb",
                "pulx", "rol",  "rola", "rolb", "ror",  "rora", "rorb", "rti",
                "rts",  "sba",  "sbca", "sbcb", "sec",  "sev",  "sta",  "stb",
                "std",  "sei",  "sts",  "stx",  "suba", "subb", "subd", "swi",
                "wai",  "tab",  "tap",  "tba",  "tim",  "tpa",  "tst",  "tsta",
                "tstb", "tsx",  "txs",  "asx1", "asx2", "xgdx", "addx", "adcx"
            };

        private static string[] op_name_str =
            {
                "aba",  "abx",  "adca", "adcb", "adda", "addb", "addd", "aim",
                "anda", "andb", "asl",  "asla", "aslb", "asld", "asr",  "asra",
                "asrb", "bcc",  "bcs",  "beq",  "bge",  "bgt",  "bhi",  "bita",
                "bitb", "ble",  "bls",  "blt",  "bmi",  "bne",  "bpl",  "bra",
                "brn",  "bsr",  "bvc",  "bvs",  "cba",  "clc",  "cli",  "clr",
                "clra", "clrb", "clv",  "cmpa", "cmpb", "cpx", "com",  "coma",
                "comb", "daa",  "dec",  "deca", "decb", "des",  "dex",  "eim",
                "eora", "eorb", "illegal", "inc", "inca", "incb", "ins", "inx",
                "jmp",  "jsr",  "ldaa",  "ldab",  "ldd",  "lds",  "ldx",  "lsr",
                "lsra", "lsrb", "lsrd", "mul",  "neg",  "nega", "negb", "nop",
                "oim",  "oraa", "orab", "psha", "pshb", "pshx", "pula", "pulb",
                "pulx", "rol",  "rola", "rolb", "ror",  "rora", "rorb", "rti",
                "rts",  "sba",  "sbca", "sbcb", "sec",  "sev",  "staa",  "stab",
                "std",  "sei",  "sts",  "stx",  "suba", "subb", "subd", "swi",
                "wai",  "tab",  "tap",  "tba",  "tim",  "tpa",  "tst",  "tsta",
                "tstb", "tsx",  "txs",  "asx1", "asx2", "xgdx", "addx", "adcx",
                "nba"
            };


        /*
         * This table defines the opcodes:
         * byte meaning
         * 0    token (menmonic)
         * 1    addressing mode
         * 2    invalid opcode for 1:6800/6802/6808, 2:6801/6803, 4:HD63701
         */

        static int[][] table = new[]
            {
             new int[] {ill, inh,7},new int[] {nop, inh,0},new int[] {ill, inh,7},new int[] {ill, inh,7},/* 00 */
             new int[] {lsrd,inh,1},new int[] {asld,inh,1},new int[] {tap, inh,0},new int[] {tpa, inh,0},
             new int[] {inx, inh,0},new int[] {dex, inh,0},new int[] {clv, inh,0},new int[] {sev, inh,0},
             new int[] {clc, inh,0},new int[] {sec, inh,0},new int[] {cli, inh,0},new int[] {sei, inh,0},
             new int[] {sba, inh,0},new int[] {cba, inh,0},new int[] {asx1,sx1,1},new int[] {asx2,sx1,1},/* 10 */
             new int[] {nba, inh,7},new int[] {ill, inh,7},new int[] {tab, inh,0},new int[] {tba, inh,0},
             new int[] {xgdx,inh,3},new int[] {daa, inh,0},new int[] {ill, inh,7},new int[] {aba, inh,0},
             new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {ill, inh,7},
             new int[] {bra, rel,0},new int[] {brn, rel,1},new int[] {bhi, rel,0},new int[] {bls, rel,0},/* 20 */
             new int[] {bcc, rel,0},new int[] {bcs, rel,0},new int[] {bne, rel,0},new int[] {beq, rel,0},
             new int[] {bvc, rel,0},new int[] {bvs, rel,0},new int[] {bpl, rel,0},new int[] {bmi, rel,0},
             new int[] {bge, rel,0},new int[] {blt, rel,0},new int[] {bgt, rel,0},new int[] {ble, rel,0},
             new int[] {tsx, inh,0},new int[] {ins, inh,0},new int[] {pula,inh,0},new int[] {pulb,inh,0},/* 30 */
             new int[] {des, inh,0},new int[] {txs, inh,0},new int[] {psha,inh,0},new int[] {pshb,inh,0},
             new int[] {pulx,inh,1},new int[] {rts, inh,0},new int[] {abx, inh,1},new int[] {rti, inh,0},
             new int[] {pshx,inh,1},new int[] {mul, inh,1},new int[] {wai, inh,0},new int[] {swi, inh,0},
             new int[] {nega,inh,0},new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {coma,inh,0},/* 40 */
             new int[] {lsra,inh,0},new int[] {ill, inh,7},new int[] {rora,inh,0},new int[] {asra,inh,0},
             new int[] {asla,inh,0},new int[] {rola,inh,0},new int[] {deca,inh,0},new int[] {ill, inh,7},
             new int[] {inca,inh,0},new int[] {tsta,inh,0},new int[] {ill, inh,7},new int[] {clra,inh,0},
             new int[] {negb,inh,0},new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {comb,inh,0},/* 50 */
             new int[] {lsrb,inh,0},new int[] {ill, inh,7},new int[] {rorb,inh,0},new int[] {asrb,inh,0},
             new int[] {aslb,inh,0},new int[] {rolb,inh,0},new int[] {decb,inh,0},new int[] {ill, inh,7},
             new int[] {incb,inh,0},new int[] {tstb,inh,0},new int[] {ill, inh,7},new int[] {clrb,inh,0},
             new int[] {neg, idx,0},new int[] {aim, imx,3},new int[] {oim, imx,3},new int[] {com, idx,0},/* 60 */
             new int[] {lsr, idx,0},new int[] {eim, imx,3},new int[] {ror, idx,0},new int[] {asr, idx,0},
             new int[] {asl, idx,0},new int[] {rol, idx,0},new int[] {dec, idx,0},new int[] {tim, imx,3},
             new int[] {inc, idx,0},new int[] {tst, idx,0},new int[] {jmp, idx,0},new int[] {clr, idx,0},
             new int[] {neg, ext,0},new int[] {aim, imd,3},new int[] {oim, imd,3},new int[] {com, ext,0},/* 70 */
             new int[] {lsr, ext,0},new int[] {eim, imd,3},new int[] {ror, ext,0},new int[] {asr, ext,0},
             new int[] {asl, ext,0},new int[] {rol, ext,0},new int[] {dec, ext,0},new int[] {tim, imd,3},
             new int[] {inc, ext,0},new int[] {tst, ext,0},new int[] {jmp, ext,0},new int[] {clr, ext,0},
             new int[] {suba,imb,0},new int[] {cmpa,imb,0},new int[] {sbca,imb,0},new int[] {subd,imw,1},/* 80 */
             new int[] {anda,imb,0},new int[] {bita,imb,0},new int[] {lda, imb,0},new int[] {sta, imb,7},
             new int[] {eora,imb,0},new int[] {adca,imb,0},new int[] {ora, imb,0},new int[] {adda,imb,0},
             new int[] {cmpx,imw,0},new int[] {bsr, rel,0},new int[] {lds, imw,0},new int[] {sts, imw,7},
             new int[] {suba,dir,0},new int[] {cmpa,dir,0},new int[] {sbca,dir,0},new int[] {subd,dir,1},/* 90 */
             new int[] {anda,dir,0},new int[] {bita,dir,0},new int[] {lda, dir,0},new int[] {sta, dir,0},
             new int[] {eora,dir,0},new int[] {adca,dir,0},new int[] {ora, dir,0},new int[] {adda,dir,0},
             new int[] {cmpx,dir,0},new int[] {jsr, dir,1},new int[] {lds, dir,0},new int[] {sts, dir,0},
             new int[] {suba,idx,0},new int[] {cmpa,idx,0},new int[] {sbca,idx,0},new int[] {subd,idx,1},/* a0 */
             new int[] {anda,idx,0},new int[] {bita,idx,0},new int[] {lda, idx,0},new int[] {sta, idx,0},
             new int[] {eora,idx,0},new int[] {adca,idx,0},new int[] {ora, idx,0},new int[] {adda,idx,0},
             new int[] {cmpx,idx,0},new int[] {jsr, idx,0},new int[] {lds, idx,0},new int[] {sts, idx,0},
             new int[] {suba,ext,0},new int[] {cmpa,ext,0},new int[] {sbca,ext,0},new int[] {subd,ext,1},/* b0 */
             new int[] {anda,ext,0},new int[] {bita,ext,0},new int[] {lda, ext,0},new int[] {sta, ext,0},
             new int[] {eora,ext,0},new int[] {adca,ext,0},new int[] {ora, ext,0},new int[] {adda,ext,0},
             new int[] {cmpx,ext,0},new int[] {jsr, ext,0},new int[] {lds, ext,0},new int[] {sts, ext,0},
             new int[] {subb,imb,0},new int[] {cmpb,imb,0},new int[] {sbcb,imb,0},new int[] {addd,imw,1},/* c0 */
             new int[] {andb,imb,0},new int[] {bitb,imb,0},new int[] {ldb, imb,0},new int[] {stb, imb,7},
             new int[] {eorb,imb,0},new int[] {adcb,imb,0},new int[] {orb, imb,0},new int[] {addb,imb,0},
             new int[] {ldd, imw,1},new int[] {_std,imw,1},new int[] {ldx, imw,0},new int[] {stx, imw,7},
             new int[] {subb,dir,0},new int[] {cmpb,dir,0},new int[] {sbcb,dir,0},new int[] {addd,dir,1},/* d0 */
             new int[] {andb,dir,0},new int[] {bitb,dir,0},new int[] {ldb, dir,0},new int[] {stb, dir,0},
             new int[] {eorb,dir,0},new int[] {adcb,dir,0},new int[] {orb, dir,0},new int[] {addb,dir,0},
             new int[] {ldd, dir,1},new int[] {_std,dir,1},new int[] {ldx, dir,0},new int[] {stx, dir,0},
             new int[] {subb,idx,0},new int[] {cmpb,idx,0},new int[] {sbcb,idx,0},new int[] {addd,idx,1},/* e0 */
             new int[] {andb,idx,0},new int[] {bitb,idx,0},new int[] {ldb, idx,0},new int[] {stb, idx,0},
             new int[] {eorb,idx,0},new int[] {adcb,idx,0},new int[] {orb, idx,0},new int[] {addb,idx,0},
             new int[] {ldd, idx,1},new int[] {_std,idx,1},new int[] {ldx, idx,0},new int[] {stx, idx,0},
             new int[] {subb,ext,0},new int[] {cmpb,ext,0},new int[] {sbcb,ext,0},new int[] {addd,ext,1},/* f0 */
             new int[] {andb,ext,0},new int[] {bitb,ext,0},new int[] {ldb, ext,0},new int[] {stb, ext,0},
             new int[] {eorb,ext,0},new int[] {adcb,ext,0},new int[] {orb, ext,0},new int[] {addb,ext,0},
             new int[] {ldd, ext,1},new int[] {_std,ext,1},new int[] {ldx, ext,0},new int[] {stx, ext,0},

             /* extra instruction $fc for NSC-8105 */
             new int[] {addx,ext,0},
             /* extra instruction $ec for NSC-8105 */
             new int[] {adcx,imb,0}
         };

        public bool EnableUndocumentedOpcodes
        {
            get { return _enableUndocumentedOpcodes; }
            set
            {
                _enableUndocumentedOpcodes = value;
                var patchValue = _enableUndocumentedOpcodes ? 0 : 1;
                table[0x14][2] = patchValue;
                table[0x87][2] = patchValue;
                table[0x8f][2] = patchValue;
                table[0xc7][2] = patchValue;
                table[0xcF][2] = patchValue;
            }
        }


        ///* some macros to keep things short */
        //#define OP      oprom[0]
        //#define ARG1    opram[1]
        //#define ARG2    opram[2]
        //#define ARGW    (opram[1]<<8) + opram[2]
        public const int DASMFLAG_SUPPORTED = 0x04;
        public const int DASMFLAG_STEP_OVER = 0x08;
        public const int DASMFLAG_STEP_OUT = 0x10;

        private static int SIGNED(int b)
        {
            return ((int)((b & 0x80) == 0x80 ? b | 0xffffff00 : b));
        }

        public static bool IsSubroutine(int opcode)
        {
            return opcode == bsr || opcode == jsr;
        }

        public static bool IsReturn(int opcode)
        {
            return opcode == rti || opcode == rts;
        }

        public static DasmResult Disassemble(int[] memory, int pc)
        {
            int flags = 0;
            int invalid_mask;
            int code = memory[pc] & 0xff;
            int opcode, args, invalid;

            invalid_mask = 1;

            opcode = table[code][0];
            args = table[code][1];
            invalid = table[code][2];

            if (IsSubroutine(opcode))
                flags = DASMFLAG_STEP_OVER;
            else if (IsReturn(opcode))
                flags = DASMFLAG_STEP_OUT;


            if ((invalid & invalid_mask) == invalid_mask)   /* invalid for this cpu type ? */
            {
                return new DasmResult()
                {
                    Illegal = true,
                    ByteLength = 1,
                    Flags = flags | DASMFLAG_SUPPORTED
                };
            }


            var instruction = string.Format("{0,-4} ", op_name_str[opcode]);

            int byteLength = 0;

            string operand = "";

            switch (args)
            {
                case rel:  /* relative */
                    operand += string.Format("${0:X4}", pc + SIGNED(memory[pc + 1]) + 2);
                    byteLength = 2;
                    break;
                case imb:  /* immediate (byte) */
                    operand += string.Format("#${0:X2}", memory[pc + 1]);
                    byteLength = 2;
                    break;
                case imw:  /* immediate (word) */
                    operand += string.Format("#${0:X4}", (memory[pc + 1] << 8) + memory[pc + 2]);
                    byteLength = 3;
                    break;
                case idx:  /* indexed + byte offset */
                    operand += string.Format("${0:X2},x", memory[pc + 1]);
                    byteLength = 2;
                    break;
                case imx:  /* immediate, indexed + byte offset */
                    operand += string.Format("#${0:X2},(x+${1:X2})", memory[pc + 1], memory[pc + 2]);
                    byteLength = 3;
                    break;
                case dir:  /* direct address */
                    operand += string.Format("${0:X2}", memory[pc + 1]);
                    byteLength = 2;
                    break;
                case imd:  /* immediate, direct address */
                    operand += string.Format("#${0:X2},${1:X2}", memory[pc + 1], memory[pc + 2]);
                    byteLength = 3;
                    break;
                case ext:  /* extended address */
                    operand += string.Format("${0:X4}", (memory[pc + 1] << 8) + memory[pc + 2]);
                    byteLength = 3;
                    break;
                case sx1:  /* byte from address (s + 1) */
                    operand += string.Format("(s+1)");
                    byteLength = 1;
                    break;
                default:
                    byteLength = 1;
                    break;
            }

            return new DasmResult()
            {
                Instruction = instruction,
                Operand = operand,
                ByteLength = byteLength,
                Flags = flags | DASMFLAG_SUPPORTED
            };
        }

        public static int Disassemble(int[] memory, int pc, ref string buf)
        {
            int flags = 0;
            int invalid_mask;
            int code = memory[pc] & 0xff;
            int opcode, args, invalid;

            invalid_mask = 1;

            opcode = table[code][0];
            args = table[code][1];
            invalid = table[code][2];

            if (IsSubroutine(opcode))
                flags = DASMFLAG_STEP_OVER;
            else if (IsReturn(opcode))
                flags = DASMFLAG_STEP_OUT;

            if ((invalid & invalid_mask) == invalid_mask)   /* invalid for this cpu type ? */
            {
                buf += "illegal";
                return 1 | flags | DASMFLAG_SUPPORTED;
            }

            buf += string.Format("{0,-4} ", op_name_str[opcode]);

            switch (args)
            {
                case rel:  /* relative */
                    buf += string.Format("${0:X4}", pc + SIGNED(memory[pc + 1]) + 2);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imb:  /* immediate (byte) */
                    buf += string.Format("#${0:X2}", memory[pc + 1]);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imw:  /* immediate (word) */
                    buf += string.Format("#${0:X4}", (memory[pc + 1] << 8) + memory[pc + 2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case idx:  /* indexed + byte offset */
                    buf += string.Format("${0:X2},x", memory[pc + 1]);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imx:  /* immediate, indexed + byte offset */
                    buf += string.Format("#${0:X2},(x+${1:X2})", memory[pc + 1], memory[pc + 2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case dir:  /* direct address */
                    buf += string.Format("${0:X2}", memory[pc + 1]);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imd:  /* immediate, direct address */
                    buf += string.Format("#${0:X2},${1:X2}", memory[pc + 1], memory[pc + 2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case ext:  /* extended address */
                    buf += string.Format("${0:X4}", (memory[pc + 1] << 8) + memory[pc + 2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case sx1:  /* byte from address (s + 1) */
                    buf += string.Format("(s+1)");
                    return 1 | flags | DASMFLAG_SUPPORTED;
                default:
                    return 1 | flags | DASMFLAG_SUPPORTED;
            }
        }

        public static void SelfTest()
        {
            for (int i = 0; i < table.Length; i++)
            {
                var entry = table[i];
                //if ((entry[2] & 1) == entry[2])
                //{
                if (!((IList)valid6800opcodes).Contains(i))
                {
                    // this is an invalid opcode
                    Debug.WriteLine("{0:X2}: {1}", i, entry[2]);
                }
                //}
            }
        }

        // List of 197 valid opcodes
        private static int[] valid6800opcodes =
            {
                0x01,
                0x06,
                0x07,
                0x08,
                0x09,
                0x0A,
                0x0B,
                0x0C,
                0x0D,
                0x0E,
                0x0F,
                0x10,
                0x11,
                0x16,
                0x17,
                0x19,
                0x1B,
                0x20,
                0x22,
                0x23,
                0x24,
                0x25,
                0x26,
                0x27,
                0x28,
                0x29,
                0x2A,
                0x2B,
                0x2C,
                0x2D,
                0x2E,
                0x2F,
                0x30,
                0x31,
                0x32,
                0x33,
                0x34,
                0x35,
                0x36,
                0x37,
                0x39,
                0x3B,
                0x3E,
                0x3F,
                0x40,
                0x43,
                0x44,
                0x46,
                0x47,
                0x48,
                0x49,
                0x4A,
                0x4C,
                0x4D,
                0x4F,
                0x50,
                0x53,
                0x54,
                0x56,
                0x57,
                0x58,
                0x59,
                0x5A,
                0x5C,
                0x5D,
                0x5F,
                0x60,
                0x63,
                0x64,
                0x66,
                0x67,
                0x68,
                0x69,
                0x6A,
                0x6C,
                0x6D,
                0x6E,
                0x6F,
                0x70,
                0x73,
                0x74,
                0x76,
                0x77,
                0x78,
                0x79,
                0x7A,
                0x7C,
                0x7D,
                0x7E,
                0x7F,
                0x80,
                0x81,
                0x82,
                0x84,
                0x85,
                0x86,
                0x88,
                0x89,
                0x8A,
                0x8B,
                0x8C,
                0x8D,
                0x8E,
                0x90,
                0x91,
                0x92,
                0x94,
                0x95,
                0x96,
                0x97,
                0x98,
                0x99,
                0x9A,
                0x9B,
                0x9C,
                0x9E,
                0x9F,
                0xA0,
                0xA1,
                0xA2,
                0xA4,
                0xA5,
                0xA6,
                0xA7,
                0xA8,
                0xA9,
                0xAA,
                0xAB,
                0xAC,
                0xAD,
                0xAE,
                0xAF,
                0xB0,
                0xB1,
                0xB2,
                0xB4,
                0xB5,
                0xB6,
                0xB7,
                0xB8,
                0xB9,
                0xBA,
                0xBB,
                0xBC,
                0xBD,
                0xBE,
                0xBF,
                0xC0,
                0xC1,
                0xC2,
                0xC4,
                0xC5,
                0xC6,
                0xC8,
                0xC9,
                0xCA,
                0xCB,
                0xCE,
                0xD0,
                0xD1,
                0xD2,
                0xD4,
                0xD5,
                0xD6,
                0xD7,
                0xD8,
                0xD9,
                0xDA,
                0xDB,
                0xDE,
                0xDF,
                0xE0,
                0xE1,
                0xE2,
                0xE4,
                0xE5,
                0xE6,
                0xE7,
                0xE8,
                0xE9,
                0xEA,
                0xEB,
                0xEE,
                0xEF,
                0xF0,
                0xF1,
                0xF2,
                0xF4,
                0xF5,
                0xF6,
                0xF7,
                0xF8,
                0xF9,
                0xFA,
                0xFB,
                0xFE,
                0xFF
            };

        private bool _enableUndocumentedOpcodes;
    }
}