using System;
using System.Diagnostics;

namespace Sharp6800
{
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

        private static string[] op_name_str =
            {
                "aba", "abx", "adca", "adcb", "adda", "addb", "addd", "aim",
                "anda", "andb", "asl", "asla", "aslb", "asld", "asr", "asra",
                "asrb", "bcc", "bcs", "beq", "bge", "bgt", "bhi", "bita",
                "bitb", "ble", "bls", "blt", "bmi", "bne", "bpl", "bra",
                "brn", "bsr", "bvc", "bvs", "cba", "clc", "cli", "clr",
                "clra", "clrb", "clv", "cmpa", "cmpb", "cmpx", "com", "coma",
                "comb", "daa", "dec", "deca", "decb", "des", "dex", "eim",
                "eora", "eorb", "illegal", "inc", "inca", "incb", "ins", "inx",
                "jmp", "jsr", "lda", "ldb", "ldd", "lds", "ldx", "lsr",
                "lsra", "lsrb", "lsrd", "mul", "neg", "nega", "negb", "nop",
                "oim", "ora", "orb", "psha", "pshb", "pshx", "pula", "pulb",
                "pulx", "rol", "rola", "rolb", "ror", "rora", "rorb", "rti",
                "rts", "sba", "sbca", "sbcb", "sec", "sev", "sta", "stb",
                "std", "sei", "sts", "stx", "suba", "subb", "subd", "swi",
                "wai", "tab", "tap", "tba", "tim", "tpa", "tst", "tsta",
                "tstb", "tsx", "txs", "asx1", "asx2", "xgdx", "addx", "adcx"
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
             new int[] {sba, inh,0},new int[] {cba, inh,0},new int[] {asx1,sx1,0},new int[] {asx2,sx1,0},/* 10 */
             new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {tab, inh,0},new int[] {tba, inh,0},
             new int[] {xgdx,inh,3},new int[] {daa, inh,0},new int[] {ill, inh,7},new int[] {aba, inh,0},
             new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {ill, inh,7},new int[] {ill, inh,7},
             new int[] {bra, rel,0},new int[] {brn, rel,0},new int[] {bhi, rel,0},new int[] {bls, rel,0},/* 20 */
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
             new int[] {anda,imb,0},new int[] {bita,imb,0},new int[] {lda, imb,0},new int[] {sta, imb,0},
             new int[] {eora,imb,0},new int[] {adca,imb,0},new int[] {ora, imb,0},new int[] {adda,imb,0},
             new int[] {cmpx,imw,0},new int[] {bsr, rel,0},new int[] {lds, imw,0},new int[] {sts, imw,0},
             new int[] {suba,dir,0},new int[] {cmpa,dir,0},new int[] {sbca,dir,0},new int[] {subd,dir,1},/* 90 */
             new int[] {anda,dir,0},new int[] {bita,dir,0},new int[] {lda, dir,0},new int[] {sta, dir,0},
             new int[] {eora,dir,0},new int[] {adca,dir,0},new int[] {ora, dir,0},new int[] {adda,dir,0},
             new int[] {cmpx,dir,0},new int[] {jsr, dir,0},new int[] {lds, dir,0},new int[] {sts, dir,0},
             new int[] {suba,idx,0},new int[] {cmpa,idx,0},new int[] {sbca,idx,0},new int[] {subd,idx,1},/* a0 */
             new int[] {anda,idx,0},new int[] {bita,idx,0},new int[] {lda, idx,0},new int[] {sta, idx,0},
             new int[] {eora,idx,0},new int[] {adca,idx,0},new int[] {ora, idx,0},new int[] {adda,idx,0},
             new int[] {cmpx,idx,0},new int[] {jsr, idx,0},new int[] {lds, idx,0},new int[] {sts, idx,0},
             new int[] {suba,ext,0},new int[] {cmpa,ext,0},new int[] {sbca,ext,0},new int[] {subd,ext,1},/* b0 */
             new int[] {anda,ext,0},new int[] {bita,ext,0},new int[] {lda, ext,0},new int[] {sta, ext,0},
             new int[] {eora,ext,0},new int[] {adca,ext,0},new int[] {ora, ext,0},new int[] {adda,ext,0},
             new int[] {cmpx,ext,0},new int[] {jsr, ext,0},new int[] {lds, ext,0},new int[] {sts, ext,0},
             new int[] {subb,imb,0},new int[] {cmpb,imb,0},new int[] {sbcb,imb,0},new int[] {addd,imw,1},/* c0 */
             new int[] {andb,imb,0},new int[] {bitb,imb,0},new int[] {ldb, imb,0},new int[] {stb, imb,0},
             new int[] {eorb,imb,0},new int[] {adcb,imb,0},new int[] {orb, imb,0},new int[] {addb,imb,0},
             new int[] {ldd, imw,1},new int[] {_std,imw,1},new int[] {ldx, imw,0},new int[] {stx, imw,0},
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

        ///* some macros to keep things short */
        //#define OP      oprom[0]
        //#define ARG1    opram[1]
        //#define ARG2    opram[2]
        //#define ARGW    (opram[1]<<8) + opram[2]
        public const int DASMFLAG_SUPPORTED = 0x04;
        public const int DASMFLAG_STEP_OVER = 0x08;
        public const int DASMFLAG_STEP_OUT = 0x10;

        public static int Disassemble(int pc, ref string buf, int[] oprom, int[] opram)
        {
            int flags = 0;
            int invalid_mask;
            int code = oprom[0];
            int opcode, args, invalid;

            invalid_mask = 1;

            opcode = table[code][0];
            args = table[code][1];
            invalid = table[code][2];

            if (opcode == bsr || opcode == jsr)
                flags = DASMFLAG_STEP_OVER;
            else if (opcode == rti || opcode == rts)
                flags = DASMFLAG_STEP_OUT;

            //if (invalid & invalid_mask)   /* invalid for this cpu type ? */
            //{
            //    buf = "illegal";
            //    return 1 | flags | DASMFLAG_SUPPORTED;
            //}

            buf += string.Format("{0} ", op_name_str[opcode]);

            switch (args)
            {
                case rel:  /* relative */
                    buf += string.Format("${0:X4}", pc + opram[1] + 2);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imb:  /* immediate (byte) */
                    buf += string.Format("#${0:X2}", opram[1]);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imw:  /* immediate (word) */
                    buf += string.Format("#${0:X4}", (opram[1] << 8) + opram[2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case idx:  /* indexed + byte offset */
                    buf += string.Format("(x+${0:X2})", opram[1]);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imx:  /* immediate, indexed + byte offset */
                    buf += string.Format("#${0:X2},(x+${1:X2})", opram[1], opram[2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case dir:  /* direct address */
                    buf += string.Format("${0:X2}", opram[1]);
                    return 2 | flags | DASMFLAG_SUPPORTED;
                case imd:  /* immediate, direct address */
                    buf += string.Format("#${0:X2},${1:X2}", opram[1], opram[2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case ext:  /* extended address */
                    buf += string.Format("${0:X4}", (opram[1] << 8) + opram[2]);
                    return 3 | flags | DASMFLAG_SUPPORTED;
                case sx1:  /* byte from address (s + 1) */
                    buf += string.Format("(s+1)");
                    return 1 | flags | DASMFLAG_SUPPORTED;
                default:
                    return 1 | flags | DASMFLAG_SUPPORTED;
            }
        }
    }
}