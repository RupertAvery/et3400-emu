/*
 *    NOTICE - This code contains work derived from the MAME project (http://mamedev.org/source/src/emu/cpu/m6800/index.html)
 */
namespace Core6800
{
    public partial class Cpu6800
    {
        public void InterpretOpCode(int opCode)
        {
            switch (opCode)
            {
                /* 0x00 ILLEGAL */
                /* 0x01 NOP */
                case 0x01:
                    {
                    }
                    break;
                /* 0x02 ILLEGAL */
                /* 0x03 ILLEGAL */
                /* 0x04 LSRD inherent -0*-* */
                //case  0x04 :
                //  {
                //      int t;
                //      CLR_NZC(); t = D; State.CC|=(t&0x0001);
                //      t>>=1; SET_Z16(t); D=t;
                //  }

                /* 0x05 ASLD inherent ?**** */
                //  break;
                //case  asld :
                //  {
                //      int r;
                //      int t;
                //      t = D; r=t<<1;
                //      CLR_NZVC(); SET_FLAGS16(t,t,r);
                //      D=r;
                //  }

                //  /* 0x06 TAP inherent ##### */
                //  break;
                case 0x06:
                    {
                        State.CC = State.A;
                        //ONE_MORE_INSN();
                        //CHECK_IRQ_LINES(); /* HJB 990417 */
                    }

                    /* 0x07 TPA inherent ----- */
                    break;
                case 0x07:
                    {
                        State.A = State.CC;
                    }

                    /* 0x08 INX inherent --*-- */
                    break;
                case 0x08:
                    {
                        ++State.X;
                        CLR_Z(); SET_Z16(State.X);
                    }

                    /* 0x09 DEX inherent --*-- */
                    break;
                case 0x09:
                    {
                        --State.X;
                        CLR_Z(); SET_Z16(State.X);
                    }

                    /* 0x0a CLV */
                    break;
                case 0x0a:
                    {
                        CLV();
                    }

                    /* 0x0b SEV */
                    break;
                case 0x0b:
                    {
                        SEV();
                    }

                    /* 0x0c CLC */
                    break;
                case 0x0c:
                    {
                        CLC();
                    }

                    /* 0x0d SEC */
                    break;
                case 0x0d:
                    {
                        SEC();
                    }

                    /* 0x0e CLI */
                    break;
                case 0x0e:
                    {
                        CLI();
                        ONE_MORE_INSN();
                        CHECK_IRQ_LINES(); /* HJB 990417 */
                    }

                    /* 0x0f SEI */
                    break;
                case 0x0f:
                    {
                        SEI();
                        ONE_MORE_INSN();
                        CHECK_IRQ_LINES(); /* HJB 990417 */
                    }

                    /* 0x10 SBA inherent -**** */
                    break;
                case 0x10:
                    {
                        int t = State.A - State.B;
                        CLR_NZVC(); SET_FLAGS8(State.A, State.B, t);
                        State.A = t;
                    }

                    /* 0x11 CBA inherent -**** */
                    break;
                case 0x11:
                    {
                        var t = State.A - State.B;
                        CLR_NZVC(); SET_FLAGS8(State.A, State.B, t);
                    }

                    /* 0x12 ILLEGAL */
                    //  break;
                    //case  0x12 :
                    //  {
                    //      State.X += RM( State.S + 1 );
                    //  }

                    /* 0x13 ILLEGAL */
                    //  break;
                    //case  0x13 :
                    //  {
                    //      State.X += RM( State.S + 1 );
                    //  }


                    /* 0x14 ILLEGAL */

                    /* 0x15 ILLEGAL */

                    /* 0x16 TAB inherent -**0- */
                    break;
                case 0x16:
                    {
                        State.B = State.A;
                        CLR_NZV(); SET_NZ8(State.B);
                    }

                    /* 0x17 TBA inherent -**0- */
                    break;
                case 0x17:
                    {
                        State.A = State.B;
                        CLR_NZV(); SET_NZ8(State.A);
                    }

                    /* 0x18 XGDX inherent ----- */
                    /* HD63701YO only */
                    break;
                //case  xgdx :
                //  {
                //      int t = State.X;
                //      State.X = D;
                //      D=t;
                //  }

                /* 0x19 DAA inherent (State.A) -**0* */
                //  break;
                case 0x19:
                    {
                        var cf = 0;
                        var msn = State.A & 0xf0; var lsn = State.A & 0x0f;
                        if (lsn > 0x09 || ((State.CC & 0x20) == 0x20)) cf |= 0x06;
                        if (msn > 0x80 && lsn > 0x09) cf |= 0x60;
                        if (msn > 0x90 || ((State.CC & 0x01) == 0x01)) cf |= 0x60;
                        var t = cf + State.A;
                        CLR_NZV(); /* keep carry from previous operation */
                        SET_NZ8(t); SET_C8(t);
                        State.A = t;
                    }

                    //  /* 0x1a ILLEGAL */

                    //  /* 0x1a SLP */ /* HD63701YO only */
                    break;
                //case  slp :
                //  {
                //      /* wait for next IRQ (same as waiting of wai) */
                //      cpustate->wai_state |= M6800_SLP;
                //      EAT_CYCLES;
                //  }

                /* 0x1b ABA inherent ***** */
                //  break;
                case 0x1b:
                    {
                        int t;
                        t = State.A + State.B;
                        CLR_HNZVC(); SET_FLAGS8(State.A, State.B, t); SET_H(State.A, State.B, t);
                        State.A = t;
                    }

                    /* 0x1c ILLEGAL */

                    /* 0x1d ILLEGAL */

                    /* 0x1e ILLEGAL */

                    /* 0x1f ILLEGAL */

                    /* 0x20 BRA relative ----- */
                    break;
                case 0x20:
                    {
                        int t;
                        t = IMMBYTE();
                        State.PC += SIGNED(t);
                    }

                    /* 0x21 BRN relative ----- */
                    break;
                case 0x21:
                    {
                        int m6800_brn_t; // hack around GCC 4.6 error because we need the side effects of IMMBYTE
                        m6800_brn_t = IMMBYTE();
                    }

                    /* 0x22 BHI relative ----- */
                    break;
                case 0x22:
                    {
                        int t;
                        var condition = (State.CC & 0x04 >> 2 | State.CC & 0x01) == 0x00;
                        // (State.CC & 0x05) != 0x05;
                        BRANCH(condition);
                    }

                    /* 0x23 BLS relative ----- */
                    break;
                case 0x23:
                    {
                        int t;
                        BRANCH((State.CC & 0x05) == 0x05);
                    }

                    /* 0x24 BCC relative ----- */
                    break;
                case 0x24:
                    {
                        int t;
                        BRANCH((State.CC & 0x01) != 0x01);
                    }

                    /* 0x25 BCS relative ----- */
                    break;
                case 0x25:
                    {
                        int t;
                        BRANCH((State.CC & 0x01) == 0x01);
                    }

                    /* 0x26 BNE relative ----- */
                    break;
                case 0x26:
                    {
                        int t;
                        BRANCH((State.CC & 0x04) != 0x04);
                    }

                    /* 0x27 BEQ relative ----- */
                    break;
                case 0x27:
                    {
                        int t;
                        BRANCH((State.CC & 0x04) == 0x04);
                    }

                    /* 0x28 BVC relative ----- */
                    break;
                case 0x28:
                    {
                        int t;
                        BRANCH((State.CC & 0x02) != 0x02);
                    }

                    /* 0x29 BVS relative ----- */
                    break;
                case 0x29:
                    {
                        int t;
                        BRANCH((State.CC & 0x02) == 0x02);
                    }

                    /* 0x2a BPL relative ----- */
                    break;
                case 0x2a:
                    {
                        int t;
                        BRANCH((State.CC & 0x08) != 0x08);
                    }

                    /* 0x2b BMI relative ----- */
                    break;
                case 0x2b:
                    {
                        int t;
                        BRANCH((State.CC & 0x08) == 0x08);
                    }

                    /* 0x2c BGE relative ----- */
                    break;
                case 0x2c:
                    {
                        int t;
                        BRANCH(!NXORV());
                    }

                    /* 0x2d BLT relative ----- */
                    break;
                case 0x2d:
                    {
                        int t;
                        BRANCH(NXORV());
                    }

                    /* 0x2e BGT relative ----- */
                    break;
                case 0x2e:
                    {
                        int t;
                        BRANCH(!(NXORV() || (State.CC & 0x04) == 0x04));
                    }

                    /* 0x2f BLE relative ----- */
                    break;
                case 0x2f:
                    {
                        int t;
                        BRANCH(NXORV() || (State.CC & 0x04) == 0x04);
                    }

                    /* 0x30 TSX inherent ----- */
                    break;
                case 0x30:
                    {
                        State.X = (State.S + 1);
                    }

                    /* 0x31 INS inherent ----- */
                    break;
                case 0x31:
                    {
                        ++State.S;
                    }

                    /* 0x32 PULA inherent ----- */
                    break;
                case 0x32:
                    {
                        State.A = PULLBYTE();
                    }

                    /* 0x33 PULB inherent ----- */
                    break;
                case 0x33:
                    {
                        State.B = PULLBYTE();
                    }

                    /* 0x34 DES inherent ----- */
                    break;
                case 0x34:
                    {
                        --State.S;
                    }

                    /* 0x35 TXS inherent ----- */
                    break;
                case 0x35:
                    {
                        State.S = (State.X - 1);
                    }

                    /* 0x36 PSHA inherent ----- */
                    break;
                case 0x36:
                    {
                        PUSHBYTE(State.A);
                    }

                    /* 0x37 PSHB inherent ----- */
                    break;
                case 0x37:
                    {
                        PUSHBYTE(State.B);
                    }

                    /* 0x38 PULX inherent ----- */
                    break;
                case 0x38:
                    {
                        State.X = PULLWORD();
                    }

                    /* 0x39 RTS inherent ----- */
                    break;
                case 0x39:
                    {
                        State.PC = PULLWORD();
                    }

                    /* 0x3a ABX inherent ----- */
                    break;
                case 0x3a:
                    {
                        State.X += State.B;
                    }

                    /* 0x3b RTI inherent ##### */
                    break;
                case 0x3b:
                    {
                        State.CC = PULLBYTE();
                        State.B = PULLBYTE();
                        State.A = PULLBYTE();
                        State.X = PULLWORD();
                        State.PC = PULLWORD();
                        CHECK_IRQ_LINES(); /* HJB 990417 */
                    }

                    /* 0x3c PSHX inherent ----- */
                    break;
                case 0x3c:
                    {
                        PUSHWORD(State.X);
                    }

                    /* 0x3d MUL inherent --*-@ */
                    break;
                    //case 0x3d:
                    //  {
                    //      int t;
                    //      t=State.A*State.B;
                    //      CLR_C();
                    //      if((t&0x80) == 0x80) SEC();
                    //      D=t;
                    //  }

                    /* 0x3e WAI inherent ----- */
                    break;
                case 0x3e:
                    {
                        /*
                         * WAI stacks the entire machine state on the
                         * hardware stack, then waits for an interrupt.
                         */
                        //cpustate->wai_state |= M6800_WAI;
                        PUSHWORD(State.PC);
                        PUSHWORD(State.X);
                        PUSHBYTE(State.A);
                        PUSHBYTE(State.B);
                        PUSHBYTE(State.CC);
                        CHECK_IRQ_LINES();
                        //if (cpustate->wai_state & M6800_WAI) EAT_CYCLES;
                    }

                    /* 0x3f SWI absolute indirect ----- */
                    break;
                case 0x3f:
                    {
                        PUSHWORD(State.PC);
                        PUSHWORD(State.X);
                        PUSHBYTE(State.A);
                        PUSHBYTE(State.B);
                        PUSHBYTE(State.CC);
                        SEI();
                        State.PC = RM16(0xfffa);
                    }

                    /* 0x40 NEGA inherent ?**** */
                    break;
                case 0x40:
                    {
                        int r;
                        r = -State.A;
                        CLR_NZVC(); SET_FLAGS8(0, State.A, r);
                        State.A = r & 0xff;
                    }

                    /* 0x41 ILLEGAL */

                    /* 0x42 ILLEGAL */

                    /* 0x43 COMA inherent -**01 */
                    break;
                case 0x43:
                    {
                        State.A = ~State.A;
                        CLR_NZV(); SET_NZ8(State.A); SEC();
                    }

                    /* 0x44 LSRA inherent -0*-* */
                    break;
                case 0x44:
                    {
                        CLR_NZC(); State.CC |= (State.A & 0x01);
                        State.A >>= 1; SET_Z8(State.A);
                    }

                    /* 0x45 ILLEGAL */

                    /* 0x46 RORA inherent -**-* */
                    break;
                case 0x46:
                    {
                        int r;
                        r = (State.CC & 0x01) << 7;
                        CLR_NZC(); State.CC |= (State.A & 0x01);
                        r |= State.A >> 1; SET_NZ8(r);
                        State.A = r & 0xff;
                    }

                    /* 0x47 ASRA inherent ?**-* */
                    break;
                case 0x47:
                    {
                        CLR_NZC(); State.CC |= (State.A & 0x01);
                        State.A >>= 1; State.A |= ((State.A & 0x40) << 1);
                        SET_NZ8(State.A);
                    }

                    /* 0x48 ASLA inherent ?**** */
                    break;
                case 0x48:
                    {
                        int r;
                        r = State.A << 1;
                        CLR_NZVC(); SET_FLAGS8(State.A, State.A, r);
                        State.A = r & 0xff;
                    }

                    /* 0x49 ROLA inherent -**** */
                    break;
                case 0x49:
                    {
                        int t, r;
                        t = State.A; r = State.CC & 0x01; r |= t << 1;
                        CLR_NZVC(); SET_FLAGS8(t, t, r);
                        State.A = r & 0xff;
                    }

                    /* 0x4a DECA inherent -***- */
                    break;
                case 0x4a:
                    {
                        --State.A;
                        CLR_NZV(); SET_FLAGS8D(State.A);
                    }

                    /* 0x4b ILLEGAL */

                    /* 0x4c INCA inherent -***- */
                    break;
                case 0x4c:
                    {
                        ++State.A;
                        CLR_NZV(); SET_FLAGS8I(State.A);
                    }

                    /* 0x4d TSTA inherent -**0- */
                    break;
                case 0x4d:
                    {
                        CLR_NZVC(); SET_NZ8(State.A);
                    }

                    /* 0x4e ILLEGAL */

                    /* 0x4f CLRA inherent -0100 */
                    break;
                case 0x4f:
                    {
                        State.A = 0;
                        CLR_NZVC(); SEZ();
                    }

                    /* 0x50 NEGB inherent ?**** */
                    break;
                case 0x50:
                    {
                        int r;
                        r = -State.B;
                        CLR_NZVC(); SET_FLAGS8(0, State.B, r);
                        State.B = r & 0xff;
                    }

                    /* 0x51 ILLEGAL */

                    /* 0x52 ILLEGAL */

                    /* 0x53 COMB inherent -**01 */
                    break;
                case 0x53:
                    {
                        State.B = ~State.B;
                        CLR_NZV(); SET_NZ8(State.B); SEC();
                    }

                    /* 0x54 LSRB inherent -0*-* */
                    break;
                case 0x54:
                    {
                        CLR_NZC(); State.CC |= (State.B & 0x01);
                        State.B >>= 1; SET_Z8(State.B);
                    }

                    /* 0x55 ILLEGAL */

                    /* 0x56 RORB inherent -**-* */
                    break;
                case 0x56:
                    {
                        int r;
                        r = (State.CC & 0x01) << 7;
                        CLR_NZC(); State.CC |= (State.B & 0x01);
                        r |= State.B >> 1; SET_NZ8(r);
                        State.B = r & 0xff;
                    }

                    /* 0x57 ASRB inherent ?**-* */
                    break;
                case 0x57:
                    {
                        CLR_NZC(); State.CC |= (State.B & 0x01);
                        State.B >>= 1; State.B |= ((State.B & 0x40) << 1);
                        SET_NZ8(State.B);
                    }

                    /* 0x58 ASLB inherent ?**** */
                    break;
                case 0x58:
                    {
                        int r;
                        r = State.B << 1;
                        CLR_NZVC(); SET_FLAGS8(State.B, State.B, r);
                        State.B = r & 0xff;
                    }

                    /* 0x59 ROLB inherent -**** */
                    break;
                case 0x59:
                    {
                        int t, r;
                        t = State.B; r = State.CC & 0x01; r |= t << 1;
                        CLR_NZVC(); SET_FLAGS8(t, t, r);
                        State.B = r & 0xff;
                    }

                    /* 0x5a DECB inherent -***- */
                    break;
                case 0x5a:
                    {
                        --State.B;
                        CLR_NZV(); SET_FLAGS8D(State.B);
                    }

                    /* 0x5b ILLEGAL */

                    /* 0x5c INCB inherent -***- */
                    break;
                case 0x5c:
                    {
                        ++State.B;
                        CLR_NZV(); SET_FLAGS8I(State.B);
                    }

                    /* 0x5d TSTB inherent -**0- */
                    break;
                case 0x5d:
                    {
                        CLR_NZVC(); SET_NZ8(State.B);
                    }

                    /* 0x5e ILLEGAL */

                    /* 0x5f CLRB inherent -0100 */
                    break;
                case 0x5f:
                    {
                        State.B = 0;
                        CLR_NZVC(); SEZ();
                    }

                    /* 0x60 NEG indexed ?**** */
                    break;
                case 0x60:
                    {
                        int r, t;
                        t = IDXBYTE(); r = -t;
                        CLR_NZVC(); SET_FLAGS8(0, t, r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x61 AIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case aim_ix:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        r = IDXBYTE();
                    //        r &= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //        WriteMem(State.EAD, r);
                    //    }

                    /* 0x62 OIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case oim_ix:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        r = IDXBYTE();
                    //        r |= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //        WriteMem(State.EAD, r);
                    //    }

                    /* 0x63 COM indexed -**01 */
                    break;
                case 0x63:
                    {
                        int t;
                        t = IDXBYTE(); t = ~t;
                        CLR_NZV(); SET_NZ8(t); SEC();
                        WriteMem(State.EAD, t);
                    }

                    /* 0x64 LSR indexed -0*-* */
                    break;
                case 0x64:
                    {
                        int t;
                        t = IDXBYTE(); CLR_NZC(); State.CC |= (t & 0x01);
                        t >>= 1; SET_Z8(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x65 EIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case eim_ix:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        r = IDXBYTE();
                    //        r ^= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //        WriteMem(State.EAD, r);
                    //    }

                    /* 0x66 ROR indexed -**-* */
                    break;
                case 0x66:
                    {
                        int t, r;
                        t = IDXBYTE(); r = (State.CC & 0x01) << 7;
                        CLR_NZC(); State.CC |= (t & 0x01);
                        r |= t >> 1; SET_NZ8(r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x67 ASR indexed ?**-* */
                    break;
                case 0x67:
                    {
                        int t;
                        t = IDXBYTE(); CLR_NZC(); State.CC |= (t & 0x01);
                        t >>= 1; t |= ((t & 0x40) << 1);
                        SET_NZ8(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x68 ASL indexed ?**** */
                    break;
                case 0x68:
                    {
                        int t, r;
                        t = IDXBYTE(); r = t << 1;
                        CLR_NZVC(); SET_FLAGS8(t, t, r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x69 ROL indexed -**** */
                    break;
                case 0x69:
                    {
                        int t, r;
                        t = IDXBYTE(); r = State.CC & 0x01; r |= t << 1;
                        CLR_NZVC(); SET_FLAGS8(t, t, r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x6a DEC indexed -***- */
                    break;
                case 0x6a:
                    {
                        int t;
                        t = IDXBYTE(); --t;
                        CLR_NZV(); SET_FLAGS8D(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x6b TIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case tim_ix:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        r = IDXBYTE();
                    //        r &= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //    }

                    /* 0x6c INC indexed -***- */
                    break;
                case 0x6c:
                    {
                        int t;
                        t = IDXBYTE(); ++t;
                        CLR_NZV(); SET_FLAGS8I(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x6d TST indexed -**0- */
                    break;
                case 0x6d:
                    {
                        int t;
                        t = IDXBYTE(); CLR_NZVC(); SET_NZ8(t);
                    }

                    /* 0x6e JMP indexed ----- */
                    break;
                case 0x6e:
                    {
                        INDEXED(); State.PC = State.EAD;
                    }

                    /* 0x6f CLR indexed -0100 */
                    break;
                case 0x6f:
                    {
                        INDEXED(); WriteMem(State.EAD, 0);
                        CLR_NZVC(); SEZ();
                    }

                    /* 0x70 NEG extended ?**** */
                    break;
                case 0x70:
                    {
                        int r, t;
                        t = EXTBYTE(); r = -t;
                        CLR_NZVC(); SET_FLAGS8(0, t, r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x71 AIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case aim_di:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        DIRBYTE(r);
                    //        r &= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //        WriteMem(State.EAD, r);
                    //    }

                    /* 0x72 OIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case oim_di:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        DIRBYTE(r);
                    //        r |= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //        WriteMem(State.EAD, r);
                    //    }

                    /* 0x73 COM extended -**01 */
                    break;
                case 0x73:
                    {
                        int t;
                        t = EXTBYTE(); t = ~t;
                        CLR_NZV(); SET_NZ8(t); SEC();
                        WriteMem(State.EAD, t);
                    }

                    /* 0x74 LSR extended -0*-* */
                    break;
                case 0x74:
                    {
                        int t;
                        t = EXTBYTE();
                        CLR_NZC();
                        State.CC |= (t & 0x01);
                        t >>= 1;
                        SET_Z8(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x75 EIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case eim_di:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        DIRBYTE(r);
                    //        r ^= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //        WriteMem(State.EAD, r);
                    //    }

                    /* 0x76 ROR extended -**-* */
                    break;
                case 0x76:
                    {
                        int t, r;
                        t = EXTBYTE(); r = (State.CC & 0x01) << 7;
                        CLR_NZC(); State.CC |= (t & 0x01);
                        r |= t >> 1; SET_NZ8(r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x77 ASR extended ?**-* */
                    break;
                case 0x77:
                    {
                        int t;
                        t = EXTBYTE(); CLR_NZC(); State.CC |= (t & 0x01);
                        t >>= 1; t |= ((t & 0x40) << 1);
                        SET_NZ8(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x78 ASL extended ?**** */
                    break;
                case 0x78:
                    {
                        int t, r;
                        t = EXTBYTE(); r = t << 1;
                        CLR_NZVC(); SET_FLAGS8(t, t, r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x79 ROL extended -**** */
                    break;
                case 0x79:
                    {
                        int t, r;
                        t = EXTBYTE(); r = State.CC & 0x01; r |= t << 1;
                        CLR_NZVC(); SET_FLAGS8(t, t, r);
                        WriteMem(State.EAD, r);
                    }

                    /* 0x7a DEC extended -***- */
                    break;
                case 0x7a:
                    {
                        int t;
                        t = EXTBYTE(); --t;
                        CLR_NZV(); SET_FLAGS8D(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x7b TIM --**0- */
                    /* HD63701YO only */
                    break;
                    //case tim_di:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        DIRBYTE(r);
                    //        r &= t;
                    //        CLR_NZV(); SET_NZ8(r);
                    //    }

                    /* 0x7c INC extended -***- */
                    break;
                case 0x7c:
                    {
                        int t;
                        t = EXTBYTE(); ++t;
                        CLR_NZV(); SET_FLAGS8I(t);
                        WriteMem(State.EAD, t);
                    }

                    /* 0x7d TST extended -**0- */
                    break;
                case 0x7d:
                    {
                        int t;
                        t = EXTBYTE(); CLR_NZVC(); SET_NZ8(t);
                    }

                    /* 0x7e JMP extended ----- */
                    break;
                case 0x7e:
                    {
                        EXTENDED(); State.PC = State.EAD;
                    }

                    /* 0x7f CLR extended -0100 */
                    break;
                case 0x7f:
                    {
                        EXTENDED(); WriteMem(State.EAD, 0);
                        CLR_NZVC(); SEZ();
                    }

                    /* 0x80 SUBA immediate ?**** */
                    break;
                case 0x80:
                    {
                        int t, r;
                        t = IMMBYTE(); r = State.A - t;
                        CLR_NZVC(); SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0x81 CMPA immediate ?**** */
                    break;
                case 0x81:
                    {
                        int t, r;
                        t = IMMBYTE(); r = State.A - t;
                        CLR_NZVC(); SET_FLAGS8(State.A, t, r);
                    }

                    /* 0x82 SBCA immediate ?**** */
                    break;
                case 0x82:
                    {
                        int t, r;
                        t = IMMBYTE(); r = State.A - t - (State.CC & 0x01);
                        CLR_NZVC(); SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0x83 SUBD immediate -**** */
                    break;
                    //case 0x83:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = IMMWORD();
                    //        d = D;
                    //        r = d - State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //        D = r;
                    //    }

                    /* 0x84 ANDA immediate -**0- */
                    break;
                case 0x84:
                    {
                        int t;
                        t = IMMBYTE(); State.A &= t;
                        CLR_NZV(); SET_NZ8(State.A);
                    }

                    /* 0x85 BITA immediate -**0- */
                    break;
                case 0x85:
                    {
                        int t, r;
                        t = IMMBYTE(); r = State.A & t;
                        CLR_NZV(); SET_NZ8(r);
                    }

                    /* 0x86 LDA immediate -**0- */
                    break;
                case 0x86:
                    {
                        State.A = IMMBYTE();
                        CLR_NZV(); SET_NZ8(State.A);
                    }

                    /* is this State.A legal instruction? */
                    /* 0x87 STA immediate -**0- */
                    break;
                    //case 0x87:
                    //    {
                    //        CLR_NZV(); SET_NZ8(State.A);
                    //        IMM8(); WriteMem(State.EAD, State.A);
                    //    }

                    /* 0x88 EORA immediate -**0- */
                    break;
                case 0x88:
                    {
                        int t;
                        t = IMMBYTE(); State.A ^= t;
                        CLR_NZV(); SET_NZ8(State.A);
                    }

                    /* 0x89 ADCA immediate ***** */
                    break;
                case 0x89:
                    {
                        int t, r;
                        t = IMMBYTE(); r = State.A + t + (State.CC & 0x01);
                        CLR_HNZVC(); SET_FLAGS8(State.A, t, r); SET_H(State.A, t, r);
                        State.A = r;
                    }

                    /* 0x8a ORA immediate -**0- */
                    break;
                case 0x8a:
                    {
                        int t;
                        t = IMMBYTE(); State.A |= t;
                        CLR_NZV(); SET_NZ8(State.A);
                    }

                    /* 0x8b ADDA immediate ***** */
                    break;
                case 0x8b:
                    {
                        int t, r;
                        t = IMMBYTE(); r = State.A + t;
                        CLR_HNZVC(); SET_FLAGS8(State.A, t, r); SET_H(State.A, t, r);
                        State.A = r & 0xff;
                    }

                    /* 0x8c CMPX immediate -***- */
                    break;
                case 0x8c:
                    {
                        int r, d;
                        int b;
                        b = IMMWORD();
                        d = State.X;
                        r = d - b;
                        CLR_NZV();
                        SET_NZ16(r); SET_V16(d, b, r);
                    }

                    /* 0x8c CPX immediate -**** (6803) */
                    break;
                    //case 0x8c:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = IMMWORD();
                    //        d = State.X;
                    //        r = d - State.B;
                    //        CLR_NZVC(); SET_FLAGS16(d, State.B, r);
                    //    }


                    /* 0x8d BSR ----- */
                    break;
                case 0x8d:
                    {
                        int t;
                        t = IMMBYTE();
                        PUSHWORD(State.PC);
                        State.PC += SIGNED(t);
                    }

                    /* 0x8e LDS immediate -**0- */
                    break;
                case 0x8e:
                    {
                        State.S = IMMWORD();
                        CLR_NZV();
                        SET_NZ16(State.S);
                    }

                    /* 0x8f STS immediate -**0- */
                    break;
                case 0x8f:
                    {
                        CLR_NZV();
                        SET_NZ16(State.S);
                        IMM16();
                        WM16(State.EAD, State.S);
                    }

                    /* 0x90 SUBA direct ?**** */
                    break;
                case 0x90:
                    {
                        int t, r;
                        t = DIRBYTE(); r = State.A - t;
                        CLR_NZVC(); SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0x91 CMPA direct ?**** */
                    break;
                case 0x91:
                    {
                        int t, r;
                        t = DIRBYTE(); r = State.A - t;
                        CLR_NZVC(); SET_FLAGS8(State.A, t, r);
                    }

                    /* 0x92 SBCA direct ?**** */
                    break;
                case 0x92:
                    {
                        int t, r;
                        t = DIRBYTE(); r = State.A - t - (State.CC & 0x01);
                        CLR_NZVC(); SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0x93 SUBD direct -**** */
                    break;
                    //case 0x93:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = DIRWORD();
                    //        d = D;
                    //        r = d - State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //        D = r;
                    //    }

                    /* 0x94 ANDA direct -**0- */
                    break;
                case 0x94:
                    {
                        int t;
                        t = DIRBYTE(); State.A &= t;
                        CLR_NZV(); SET_NZ8(State.A);
                    }

                    /* 0x95 BITA direct -**0- */
                    break;
                case 0x95:
                    {
                        int t, r;
                        t = DIRBYTE(); r = State.A & t;
                        CLR_NZV(); SET_NZ8(r);
                    }

                    /* 0x96 LDA direct -**0- */
                    break;
                case 0x96:
                    {
                        State.A = DIRBYTE();
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0x97 STA direct -**0- */
                    break;
                case 0x97:
                    {
                        CLR_NZV();
                        SET_NZ8(State.A);
                        DIRECT();
                        WriteMem(State.EAD, State.A);
                    }

                    /* 0x98 EORA direct -**0- */
                    break;
                case 0x98:
                    {
                        int t;
                        t = DIRBYTE();
                        State.A ^= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0x99 ADCA direct ***** */
                    break;
                case 0x99:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.A + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.A, t, r);
                        SET_H(State.A, t, r);
                        State.A = r;
                    }

                    /* 0x9a ORA direct -**0- */
                    break;
                case 0x9a:
                    {
                        int t;
                        t = DIRBYTE();
                        State.A |= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0x9b ADDA direct ***** */
                    break;
                case 0x9b:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.A + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.A, t, r);
                        SET_H(State.A, t, r);
                        State.A = r & 0xff;
                    }

                    /* 0x9c CMPX direct -***- */
                    break;
                case 0x9c:
                    {
                        int r, d;
                        int b;
                        b = DIRWORD();
                        d = State.X;
                        r = d - b;
                        CLR_NZV();
                        SET_NZ16(r);
                        SET_V16(d, b, r);
                    }

                    /* 0x9c CPX direct -**** (6803) */
                    break;
                    //case 0x9c:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = DIRWORD();
                    //        d = State.X;
                    //        r = d - State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //    }

                    /* 0x9d JSR direct ----- */
                    break;
                case 0x9d:
                    {
                        DIRECT();
                        PUSHWORD(State.PC);
                        State.PC = State.EAD;
                    }

                    /* 0x9e LDS direct -**0- */
                    break;
                case 0x9e:
                    {
                        State.S = DIRWORD();
                        CLR_NZV();
                        SET_NZ16(State.S);
                    }

                    /* 0x9f STS direct -**0- */
                    break;
                case 0x9f:
                    {
                        CLR_NZV();
                        SET_NZ16(State.S);
                        DIRECT();
                        WM16(State.EAD, State.S);
                    }

                    /* 0xa0 SUBA indexed ?**** */
                    break;
                case 0xa0:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.A - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0xa1 CMPA indexed ?**** */
                    break;
                case 0xa1:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.A - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.A, t, r);
                    }

                    /* 0xa2 SBCA indexed ?**** */
                    break;
                case 0xa2:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.A - t - (State.CC & 0x01);
                        CLR_NZVC();
                        SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0xa3 SUBD indexed -**** */
                    break;
                //case 0xa3:
                //    {
                //        int r, d;
                //        int State.B;
                //        State.B = IDXWORD();
                //        d = D;
                //        r = d - State.B;
                //        CLR_NZVC();
                //        SET_FLAGS16(d, State.B, r);
                //        D = r;
                //    }

                //    /* 0xa4 ANDA indexed -**0- */
                //    break;
                case 0xa4:
                    {
                        int t;
                        t = IDXBYTE(); State.A &= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xa5 BITA indexed -**0- */
                    break;
                case 0xa5:
                    {
                        int t, r;
                        t = IDXBYTE(); r = State.A & t;
                        CLR_NZV();
                        SET_NZ8(r);
                    }

                    /* 0xa6 LDA indexed -**0- */
                    break;
                case 0xa6:
                    {
                        State.A = IDXBYTE();
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xa7 STA indexed -**0- */
                    break;
                case 0xa7:
                    {
                        CLR_NZV();
                        SET_NZ8(State.A);
                        INDEXED();
                        WriteMem(State.EAD, State.A);
                    }

                    /* 0xa8 EORA indexed -**0- */
                    break;
                case 0xa8:
                    {
                        int t;
                        t = IDXBYTE();
                        State.A ^= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xa9 ADCA indexed ***** */
                    break;
                case 0xa9:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.A + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.A, t, r);
                        SET_H(State.A, t, r);
                        State.A = r;
                    }

                    /* 0xaa ORA indexed -**0- */
                    break;
                case 0xaa:
                    {
                        int t;
                        t = IDXBYTE();
                        State.A |= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xab ADDA indexed ***** */
                    break;
                case 0xab:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.A + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.A, t, r);
                        SET_H(State.A, t, r);
                        State.A = r & 0xff;
                    }

                    /* 0xac CMPX indexed -***- */
                    break;
                case 0xac:
                    {
                        int r, d;
                        int b;
                        b = IDXWORD();
                        d = State.X;
                        r = d -b;
                        CLR_NZV();
                        SET_NZ16(r);
                        SET_V16(d, b, r);
                    }

                    /* 0xac CPX indexed -**** (6803)*/
                    break;
                    //case 0xac:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = IDXWORD();
                    //        d = State.X;
                    //        r = d - State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //    }

                    /* 0xad JSR indexed ----- */
                    break;
                case 0xad:
                    {
                        INDEXED();
                        PUSHWORD(State.PC);
                        State.PC = State.EAD;
                    }

                    /* 0xae LDS indexed -**0- */
                    break;
                case 0xae:
                    {
                        State.S = IDXWORD();
                        CLR_NZV();
                        SET_NZ16(State.S);
                    }

                    /* 0xaf STS indexed -**0- */
                    break;
                case 0xaf:
                    {
                        CLR_NZV();
                        SET_NZ16(State.S);
                        INDEXED();
                        WM16(State.EAD, State.S);
                    }

                    /* 0xb0 SUBA extended ?**** */
                    break;
                case 0xb0:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.A - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0xb1 CMPA extended ?**** */
                    break;
                case 0xb1:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.A - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.A, t, r);
                    }

                    /* 0xb2 SBCA extended ?**** */
                    break;
                case 0xb2:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.A - t - (State.CC & 0x01);
                        CLR_NZVC();
                        SET_FLAGS8(State.A, t, r);
                        State.A = r;
                    }

                    /* 0xb3 SUBD extended -**** */
                    break;
                //case 0xb3:
                //    {
                //        int r, d;
                //        int State.B;
                //        State.B = EXTWORD();
                //        d = D;
                //        r = d - State.B;
                //        CLR_NZVC();
                //        SET_FLAGS16(d, State.B, r);
                //        D = r;
                //    }

                //    /* 0xb4 ANDA extended -**0- */
                //    break;
                case 0xb4:
                    {
                        int t;
                        t = EXTBYTE();
                        State.A &= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xb5 BITA extended -**0- */
                    break;
                case 0xb5:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.A & t;
                        CLR_NZV();
                        SET_NZ8(r);
                    }

                    /* 0xb6 LDA extended -**0- */
                    break;
                case 0xb6:
                    {
                        State.A = EXTBYTE();
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xb7 STA extended -**0- */
                    break;
                case 0xb7:
                    {
                        CLR_NZV();
                        SET_NZ8(State.A);
                        EXTENDED();
                        WriteMem(State.EAD, State.A);
                    }

                    /* 0xb8 EORA extended -**0- */
                    break;
                case 0xb8:
                    {
                        int t;
                        t = EXTBYTE();
                        State.A ^= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xb9 ADCA extended ***** */
                    break;
                case 0xb9:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.A + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.A, t, r);
                        SET_H(State.A, t, r);
                        State.A = r;
                    }

                    /* 0xba ORA extended -**0- */
                    break;
                case 0xba:
                    {
                        int t;
                        t = EXTBYTE();
                        State.A |= t;
                        CLR_NZV();
                        SET_NZ8(State.A);
                    }

                    /* 0xbb ADDA extended ***** */
                    break;
                case 0xbb:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.A + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.A, t, r);
                        SET_H(State.A, t, r);
                        State.A = r & 0xff;
                    }

                    /* 0xbc CMPX extended -***- */
                    break;
                case 0xbc:
                    {
                        int r, d;
                        int b;
                        b = EXTWORD();
                        d = State.X;
                        r = d - b;
                        CLR_NZV();
                        SET_NZ16(r);
                        SET_V16(d, b, r);
                    }

                    /* 0xbc CPX extended -**** (6803) */
                    break;
                    //case 0xbc:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = EXTWORD();
                    //        d = State.X;
                    //        r = d - State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //    }

                    /* 0xbd JSR extended ----- */
                    break;
                case 0xbd:
                    {
                        EXTENDED();
                        PUSHWORD(State.PC);
                        State.PC = State.EAD;
                    }

                    /* 0xbe LDS extended -**0- */
                    break;
                case 0xbe:
                    {
                        State.S = EXTWORD();
                        CLR_NZV();
                        SET_NZ16(State.S);
                    }

                    /* 0xbf STS extended -**0- */
                    break;
                case 0xbf:
                    {
                        CLR_NZV();
                        SET_NZ16(State.S);
                        EXTENDED();
                        WM16(State.EAD, State.S);
                    }

                    /* 0xc0 SUBB immediate ?**** */
                    break;
                case 0xc0:
                    {
                        int t, r;
                        t = IMMBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xc1 CMPB immediate ?**** */
                    break;
                case 0xc1:
                    {
                        int t, r;
                        t = IMMBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                    }

                    /* 0xc2 SBCB immediate ?**** */
                    break;
                case 0xc2:
                    {
                        int t, r;
                        t = IMMBYTE();
                        r = State.B - t - (State.CC & 0x01);
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r & 0xff;
                    }

                    /* 0xc3 ADDD immediate -**** */
                    break;
                    //case 0xc3:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = IMMWORD();
                    //        d = D;
                    //        r = d + State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //        D = r;
                    //    }

                    /* 0xc4 ANDB immediate -**0- */
                    break;
                case 0xc4:
                    {
                        int t;
                        t = IMMBYTE();
                        State.B &= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xc5 BITB immediate -**0- */
                    break;
                case 0xc5:
                    {
                        int t, r;
                        t = IMMBYTE();
                        r = State.B & t;
                        CLR_NZV();
                        SET_NZ8(r);
                    }

                    /* 0xc6 LDB immediate -**0- */
                    break;
                case 0xc6:
                    {
                        State.B = IMMBYTE();
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* is this State.A legal instruction? */
                    /* 0xc7 STB immediate -**0- */
                    break;
                case 0xc7:
                    {
                        CLR_NZV();
                        SET_NZ8(State.B);
                        IMM8();
                        WriteMem(State.EAD, State.B);
                    }

                    /* 0xc8 EORB immediate -**0- */
                    break;
                case 0xc8:
                    {
                        int t;
                        t = IMMBYTE();
                        State.B ^= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xc9 ADCB immediate ***** */
                    break;
                case 0xc9:
                    {
                        int t, r;
                        t = IMMBYTE();
                        r = State.B + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xca ORB immediate -**0- */
                    break;
                case 0xca:
                    {
                        int t;
                        t = IMMBYTE();
                        State.B |= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xcb ADDB immediate ***** */
                    break;
                case 0xcb:
                    {
                        int t, r;
                        t = IMMBYTE();
                        r = State.B + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xCC LDD immediate -**0- */
                    break;
                    //case 0xcc:
                    //    {
                    //        D = IMMWORD();
                    //        CLR_NZV();
                    //        SET_NZ16(D);
                    //    }

                    /* is this State.A legal instruction? */
                    /* 0xcd STD immediate -**0- */
                    break;
                //case 0xcd:
                //    {
                //        IMM16();
                //        CLR_NZV();
                //        SET_NZ16(D);
                //        WM16(cpustate, State.EAD, &cpustate->d);
                //    }

                //    /* 0xce LDX immediate -**0- */
                //    break;
                case 0xce:
                    {
                        State.X = IMMWORD();
                        CLR_NZV();
                        SET_NZ16(State.X);
                    }

                    /* 0xcf STX immediate -**0- */
                    break;
                case 0xcf:
                    {
                        CLR_NZV();
                        SET_NZ16(State.X);
                        IMM16();
                        WM16(State.EAD, State.X);
                    }

                    /* 0xd0 SUBB direct ?**** */
                    break;
                case 0xd0:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xd1 CMPB direct ?**** */
                    break;
                case 0xd1:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                    }

                    /* 0xd2 SBCB direct ?**** */
                    break;
                case 0xd2:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.B - t - (State.CC & 0x01);
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xd3 ADDD direct -**** */
                    break;
                //case addd_di:
                //    {
                //        int r, d;
                //        int State.B;
                //        DIRWORD(State.B);
                //        d = D;
                //        r = d + State.B;
                //        CLR_NZVC();
                //        SET_FLAGS16(d, State.B, r);
                //        D = r;
                //    }

                //    /* 0xd4 ANDB direct -**0- */
                //    break;
                case 0xd4:
                    {
                        int t;
                        t = DIRBYTE();
                        State.B &= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xd5 BITB direct -**0- */
                    break;
                case 0xd5:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.B & t;
                        CLR_NZV();
                        SET_NZ8(r);
                    }

                    /* 0xd6 LDB direct -**0- */
                    break;
                case 0xd6:
                    {
                        State.B = DIRBYTE();
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xd7 STB direct -**0- */
                    break;
                case 0xd7:
                    {
                        CLR_NZV();
                        SET_NZ8(State.B);
                        DIRECT();
                        WriteMem(State.EAD, State.B);
                    }

                    /* 0xd8 EORB direct -**0- */
                    break;
                case 0xd8:
                    {
                        int t;
                        t = DIRBYTE();
                        State.B ^= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xd9 ADCB direct ***** */
                    break;
                case 0xd9:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.B + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xda ORB direct -**0- */
                    break;
                case 0xda:
                    {
                        int t;
                        t = DIRBYTE();
                        State.B |= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xdb ADDB direct ***** */
                    break;
                case 0xdb:
                    {
                        int t, r;
                        t = DIRBYTE();
                        r = State.B + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xdc LDD direct -**0- */
                    break;
                //case 0xdc:
                //    {
                //        D = DIRWORD();
                //        CLR_NZV();
                //        SET_NZ16(D);
                //    }

                //    /* 0xdd STD direct -**0- */
                //    break;
                //case 0xdd:
                //    {
                //        DIRECT;
                //        CLR_NZV();
                //        SET_NZ16(D);
                //        WM16(cpustate, State.EAD, &cpustate->d);
                //    }

                //    /* 0xde LDX direct -**0- */
                //    break;
                case 0xde:
                    {
                        State.X = DIRWORD();
                        CLR_NZV();
                        SET_NZ16(State.X);
                    }

                    /* 0xdF STX direct -**0- */
                    break;
                case 0xdF:
                    {
                        CLR_NZV();
                        SET_NZ16(State.X);
                        DIRECT();
                        WM16(State.EAD, State.X);
                    }

                    /* 0xe0 SUBB indexed ?**** */
                    break;
                case 0xe0:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xe1 CMPB indexed ?**** */
                    break;
                case 0xe1:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                    }

                    /* 0xe2 SBCB indexed ?**** */
                    break;
                case 0xe2:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.B - t - (State.CC & 0x01);
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xe3 ADDD indexed -**** */
                    break;
                    //case 0xe3:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = IDXWORD();
                    //        d = D;
                    //        r = d + State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //        D = r;
                    //    }

                    /* 0xe4 ANDB indexed -**0- */
                    break;
                case 0xe4:
                    {
                        int t;
                        t = IDXBYTE();
                        State.B &= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xe5 BITB indexed -**0- */
                    break;
                case 0xe5:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.B & t;
                        CLR_NZV();
                        SET_NZ8(r);
                    }

                    /* 0xe6 LDB indexed -**0- */
                    break;
                case 0xe6:
                    {
                        State.B = IDXBYTE();
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xe7 STB indexed -**0- */
                    break;
                case 0xe7:
                    {
                        CLR_NZV();
                        SET_NZ8(State.B);
                        INDEXED();
                        WriteMem(State.EAD, State.B);
                    }

                    /* 0xe8 EORB indexed -**0- */
                    break;
                case 0xe8:
                    {
                        int t;
                        t = IDXBYTE();
                        State.B ^= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xe9 ADCB indexed ***** */
                    break;
                case 0xe9:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.B + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xea ORB indexed -**0- */
                    break;
                case 0xea:
                    {
                        int t;
                        t = IDXBYTE();
                        State.B |= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xeb ADDB indexed ***** */
                    break;
                case 0xeb:
                    {
                        int t, r;
                        t = IDXBYTE();
                        r = State.B + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xec LDD indexed -**0- */
                    break;
                    //case 0xec:
                    //    {
                    //        D = IDXWORD();
                    //        CLR_NZV();
                    //        SET_NZ16(D);
                    //    }

                    /* 0xec ADCX immediate -****    NSC8105 only.  Flags are State.A guess - copied from addb_im() */
                    //    break;
                    //case 0xec:
                    //    {
                    //        int t, r;
                    //        t = IMMBYTE();
                    //        r = State.X + t + (State.CC & 0x01);
                    //        CLR_HNZVC();
                    //        SET_FLAGS8(State.X, t, r);
                    //        SET_H(State.X, t, r);
                    //        State.X = r;
                    //    }

                    /* 0xed STD indexed -**0- */
                    break;
                //case 0xed:
                //    {
                //        INDEXED();
                //        CLR_NZV();
                //        SET_NZ16(D);
                //        WM16(cpustate, State.EAD, &cpustate->d);
                //    }

                //    /* 0xee LDX indexed -**0- */
                //    break;
                case 0xee:
                    {
                        State.X = IDXWORD();
                        CLR_NZV();
                        SET_NZ16(State.X);
                    }

                    /* 0xef STX indexed -**0- */
                    break;
                case 0xef:
                    {
                        CLR_NZV();
                        SET_NZ16(State.X);
                        INDEXED();
                        WM16(State.EAD, State.X);
                    }

                    /* 0xf0 SUBB extended ?**** */
                    break;
                case 0xf0:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xf1 CMPB extended ?**** */
                    break;
                case 0xf1:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.B - t;
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                    }

                    /* 0xf2 SBCB extended ?**** */
                    break;
                case 0xf2:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.B - t - (State.CC & 0x01);
                        CLR_NZVC();
                        SET_FLAGS8(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xf3 ADDD extended -**** */
                    break;
                    //case 0xf3:
                    //    {
                    //        int r, d;
                    //        int State.B;
                    //        State.B = EXTWORD();
                    //        d = D;
                    //        r = d + State.B;
                    //        CLR_NZVC();
                    //        SET_FLAGS16(d, State.B, r);
                    //        D = r;
                    //    }

                    /* 0xf4 ANDB extended -**0- */
                    break;
                case 0xf4:
                    {
                        int t;
                        t = EXTBYTE();
                        State.B &= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xf5 BITB extended -**0- */
                    break;
                case 0xf5:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.B & t;
                        CLR_NZV();
                        SET_NZ8(r);
                    }

                    /* 0xf6 LDB extended -**0- */
                    break;
                case 0xf6:
                    {
                        State.B = EXTBYTE();
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xf7 STB extended -**0- */
                    break;
                case 0xf7:
                    {
                        CLR_NZV();
                        SET_NZ8(State.B);
                        EXTENDED();
                        WriteMem(State.EAD, State.B);
                    }

                    /* 0xf8 EORB extended -**0- */
                    break;
                case 0xf8:
                    {
                        int t;
                        t = EXTBYTE();
                        State.B ^= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xf9 ADCB extended ***** */
                    break;
                case 0xf9:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.B + t + (State.CC & 0x01);
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xfa ORB extended -**0- */
                    break;
                case 0xfa:
                    {
                        int t;
                        t = EXTBYTE();
                        State.B |= t;
                        CLR_NZV();
                        SET_NZ8(State.B);
                    }

                    /* 0xfb ADDB extended ***** */
                    break;
                case 0xfb:
                    {
                        int t, r;
                        t = EXTBYTE();
                        r = State.B + t;
                        CLR_HNZVC();
                        SET_FLAGS8(State.B, t, r);
                        SET_H(State.B, t, r);
                        State.B = r;
                    }

                    /* 0xfc LDD extended -**0- */
                    break;
                //case 0xfc:
                //    {
                //        D = EXTWORD();
                //        CLR_NZV();
                //        SET_NZ16(D);
                //    }

                //    /* 0xfc ADDX extended -****    NSC8105 only.  Flags are State.A guess */
                //    break;
                case 0xfc:
                    {
                        int r, d;
                        int b;
                        b= EXTWORD();
                        d = State.X;
                        r = d + b;
                        CLR_NZVC();
                        SET_FLAGS16(d, b, r);
                        State.X = r;
                    }

                    /* 0xfd STD extended -**0- */
                    break;
                //case 0xfd:
                //    {
                //        EXTENDED();
                //        CLR_NZV();
                //        SET_NZ16(D);
                //        WM16(State.EAD, D);
                //    }

                /* 0xfe LDX extended -**0- */
                //break;
                case 0xfe:
                    {
                        State.X = EXTWORD();
                        CLR_NZV();
                        SET_NZ16(State.X);
                    }

                    /* 0xff STX extended -**0- */
                    break;
                case 0xff:
                    {
                        CLR_NZV();
                        SET_NZ16(State.X);
                        EXTENDED();
                        WM16(State.EAD, State.X);
                    }
                    break;
                default:

                    break;
            }
        }
    }
}
