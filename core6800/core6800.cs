using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// Summary description for Class1
/// </summary>
public partial class core6800
{

    public core6800()
    {
        Initialize();
        //Breakpoint.Add(0xFC96);
        //Breakpoint.Add(0xFEFC);
    }

    public void Run()
    {
        int cycles = 0;
        running = true;
        //var stopwatch = new Stopwatch();
        //stopwatch.Start();
        while (running)
        {
            if (reset)
            {
                reset = false;
                PC = (ReadMem(0xFFFE) << 8) + ReadMem(0xFFFF);
                //Paused = FALSE;
                Flags.I = 1;
                NMI = 1;
                IRQ = 1;
            }
            cycles += Execute();

            if (cycles%128 == 0)
            {
                if (Interrupt != null)
                    Interrupt();
                if (cycles%1024 == 0)
                {
                    Thread.Sleep(1);
                }
            }
            
            //1,048,576 cycles/sec = 1MHz 
            //17476 = 1MHz / 60 = 60Hz interrupt rate
            if (cycles >= 17476)
            {
                //var tms = stopwatch.Elapsed.TotalMilliseconds;
                //stopwatch.Reset();
                if (Interrupt != null)
                    Interrupt();
                cycles = 0;
                Thread.Sleep(40);
            }
            if (loadram) { PC = 0; loadram = false; }
        }
    }

    public void Step()
    {
        Execute();
        if (Interrupt != null)
            Interrupt();
    }

    public void StepInto()
    {
        Execute();
        if (Interrupt != null)
            Interrupt();
    }

    public void StepOutOf()
    {
        Execute();
        if (Interrupt != null)
            Interrupt();
    }


    public int Execute()
    {
        int SaveC;
        int Operand = 0, Temp = 0;
        int FetchCode;
        int NOPS, TempPC;

        if (Breakpoint.Contains(PC) || debug)
        {
            // Do something for a hardware breaskpoint
            // Log stuff
            //if (!((PC >= 0xFE3A) && (PC <= 0xFE4F))) // Ignore OUTCH
            //{
            //    System.Diagnostics.Debug.WriteLine(string.Format("PC:{0,4:X} A:{1,2:X} B:{2,2:X} IX:{3,4:X} SP:{4,4:X}", PC, A, B, IX, SP));
            //}
        }

        FetchCode = ReadMem(PC);
        Clock = Clock + Cycles[FetchCode];

        switch (AddrMode[FetchCode])
        {
            case IMMEDIATE:
                Operand = ReadMem(PC + 1);
                break;
            case DIRECT:
                Operand = ReadMem(ReadMem(PC + 1));
                break;
            case INDEXED:
                Operand = ReadMem(IX + ReadMem(PC + 1));
                break;
            case EXTENDED:
                Operand = ReadMem((ReadMem(PC + 1) << 8) + ReadMem(PC + 2));
                break;
            case INHERENT:
                //
                break;
            case RELATIVE:
                Operand = ReadMem(PC + 1);
                if (Operand > 0x80)
                {
                    Operand = (0 - Operand) & 0xFF;
                    Operand = -Operand;
                }
                break;
        };

        switch (FetchCode)
        {
            //============ LOGIC OPCODES ==========
            //ADDA
            case 0x8B:
            case 0x9B:
            case 0xAB:
            case 0xBB:
                Temp = A;
                Temp += Operand;
                TestH(A, Operand, Temp);
                TestC(A, Operand, Temp);
                TestV(A, Operand, Temp);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //ADDB
            case 0xCB:
            case 0xDB:
            case 0xEB:
            case 0xFB:
                Temp = B;
                Temp += Operand;
                TestH(B, Operand, Temp);
                TestC(B, Operand, Temp);
                TestV(B, Operand, Temp);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                break;

            //ABA
            case 0x1B:
                Temp = A;
                Temp += B;
                TestH(A, B, Temp);
                TestC(A, B, Temp);
                TestV(A, B, Temp);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //ADCA
            case 0x89:
            case 0x99:
            case 0xA9:
            case 0xB9:
                Temp = A;
                Temp += Operand + Flags.C;
                TestH(A, Operand, Temp);
                TestC(A, Operand, Temp);
                TestV(A, Operand, Temp);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //ADCB
            case 0xC9:
            case 0xD9:
            case 0xE9:
            case 0xF9:
                Temp = B;
                Temp += Operand + Flags.C;
                TestH(B, Operand, Temp);
                TestC(B, Operand, Temp);
                TestV(B, Operand, Temp);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                break;

            //ANDA
            case 0x84:
            case 0x94:
            case 0xA4:
            case 0xB4:
                A &= Operand;
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                break;

            //ANDB
            case 0xC4:
            case 0xD4:
            case 0xE4:
            case 0xF4:
                B &= Operand;
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                break;

            //BITA
            case 0x85:
            case 0x95:
            case 0xA5:
            case 0xB5:
                Temp = A & Operand;
                TestN(Temp);
                TestZ(Temp);
                Flags.V = 0;
                break;

            //BITB
            case 0xC5:
            case 0xD5:
            case 0xE5:
            case 0xF5:
                Temp = B & Operand;
                TestN(Temp);
                TestZ(Temp);
                Flags.V = 0;
                break;

            //CLR
            case 0x6F:
                WriteMem(IX + ReadMem(PC + 1), 0);
                Flags.N = 0;
                Flags.Z = 1;
                Flags.V = 0;
                Flags.C = 0;
                break;

            case 0x7F:
                WriteMem((ReadMem(PC + 1) << 8) + ReadMem(PC + 2), 0);
                Flags.N = 0;
                Flags.Z = 1;
                Flags.V = 0;
                Flags.C = 0;
                break;

            //CLRA
            case 0x4F:
                A = 0;
                Flags.N = 0;
                Flags.Z = 1;
                Flags.V = 0;
                Flags.C = 0;
                break;

            //CLRB
            case 0x5F:
                B = 0;
                Flags.N = 0;
                Flags.Z = 1;
                Flags.V = 0;
                Flags.C = 0;
                break;

            //CMPA
            case 0x81:
            case 0x91:
            case 0xA1:
            case 0xB1:
                Temp = A - Operand;
                if (Temp < 0) Flags.C = 1; else Flags.C = 0;
                TestV(A, Operand, Temp);
                TestN(Temp);
                TestZ(Temp);
                break;

            //CMPB
            case 0xC1:
            case 0xD1:
            case 0xE1:
            case 0xF1:
                Temp = B - Operand;
                if (Temp < 0) Flags.C = 1; else Flags.C = 0;
                TestV(B, Operand, Temp);
                TestN(Temp);
                TestZ(Temp);
                break;

            //CBA
            case 0x11:
                Temp = A;
                Temp -= B;
                if (Temp < 0) Flags.C = 1; else Flags.C = 0;
                TestV(A, B, Temp);
                TestN(Temp);
                TestZ(Temp);
                break;

            //COM
            case 0x63:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ~ReadMem(TempPC) & 0xFF;
                WriteMem(TempPC, Temp);
                TestN(Temp);
                TestZ(Temp);
                Flags.V = 0;
                Flags.C = 1;
                break;

            case 0x73:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                Temp = ~ReadMem(TempPC) & 0xFF;
                WriteMem(TempPC, Temp);
                TestN(Temp);
                TestZ(Temp);
                Flags.V = 0;
                Flags.C = 1;
                break;

            //COMA
            case 0x43:
                A = ~A & 0xFF;
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                Flags.C = 1;
                break;

            //COMB
            case 0x53:
                B = ~B & 0xFF;
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                Flags.C = 1;
                break;

            //NEG
            case 0x60:
                TempPC = IX + ReadMem(PC + 1);
                Temp = (0 - ReadMem(TempPC)) & 0xFF;
                WriteMem(TempPC, Temp);
                TestN(Temp);
                Bit7V(Temp);
                TestZC(Temp);
                break;

            case 0x70:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                Temp = (0 - ReadMem(TempPC)) & 0xFF;
                WriteMem(TempPC, Temp);
                TestN(Temp);
                Bit7V(Temp);
                TestZC(Temp);
                break;

            //NEGA
            case 0x40:
                Temp = A;
                Temp = 0 - Temp;
                A = Temp & 0xFF;
                TestN(A);
                Bit7V(A);
                TestZC(A);
                break;

            //NEGB
            case 0x50:
                Temp = B;
                Temp = 0 - Temp;
                B = Temp & 0xFF;
                TestN(B);
                Bit7V(B);
                TestZC(B);
                break;

            //DAA
            case 0x19:
                Temp = A;
                if (((A & 0xF) > 9) | (Flags.H == 1))
                {
                    A += 0x06;
                    TestV(Temp, 0x06, A);
                }
                if ((((A & 0xF0) >> 8) > 9) | (Flags.C == 1))
                {
                    A += 0x60;
                    TestV(Temp, 0x60, A);
                }
                if (((A & 0xF) > 9) & ((A & 0xF0) == 0x90))
                {
                    A += 0x60;
                    TestV(Temp, 0x60, A);
                }
                if (((A & 0xF0) >> 8) > 9)
                {
                    Flags.C = 1;
                }
                TestN(A);
                TestZ(A);
                break;

            //DEC
            case 0x6A:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                Bit7V(Temp);
                WriteMem(TempPC, (Temp - 1) & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));

                break;

            case 0x7A:
                TempPC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                Bit7V(Temp);
                WriteMem(TempPC, (Temp - 1) & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                break;

            //DECA
            case 0x4A:
                Temp = A;
                Bit7V(Temp);
                Temp = Temp - 1;
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //DECB
            case 0x5A:
                Temp = B;
                Bit7V(Temp);
                Temp = Temp - 1;
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                break;

            //EORA
            case 0x88:
            case 0x98:
            case 0xA8:
            case 0xB8:
                A ^= Operand;
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                break;

            //EORB
            case 0xC8:
            case 0xD8:
            case 0xE8:
            case 0xF8:
                B ^= Operand;
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                break;

            //INC
            case 0x6C:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                if (Temp == 0x7F) Flags.V = 1; else Flags.V = 0;
                Temp = Temp + 1;
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                break;

            case 0x7C:
                TempPC = ReadMem(PC + 1);
                TempPC = TempPC * 0x100 + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                if (Temp == 0x7F) Flags.V = 1; else Flags.V = 0;
                Temp++;
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                break;

            //INCA
            case 0x4C:
                Temp = A;
                if (Temp == 0x7F) Flags.V = 1; else Flags.V = 0;
                Temp++;
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //INCB
            case 0x5C:
                Temp = B;
                if (Temp == 0x7F) Flags.V = 1; else Flags.V = 0;
                Temp++;
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                break;

            //LDAA
            case 0x86:
            case 0x96:
            case 0xA6:
            case 0xB6:
                A = Operand;
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                break;

            //LDAB
            case 0xC6:
            case 0xD6:
            case 0xE6:
            case 0xF6:
                B = Operand;
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                break;

            //ORA
            case 0x8A:
            case 0x9A:
            case 0xAA:
            case 0xBA:
                A |= Operand;
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                break;

            //ORB
            case 0xCA:
            case 0xDA:
            case 0xEA:
            case 0xFA:
                B |= Operand;
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                break;

            //PSHA
            case 0x36:
                WriteMem(SP--, A);
                break;

            //PSHB
            case 0x37:
                WriteMem(SP--, B);
                break;

            //PULA
            case 0x32:
                A = ReadMem(++SP);
                break;

            //PULB
            case 0x33:
                B = ReadMem(++SP);
                break;

            //ROL
            case 0x69:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                SaveC = Flags.C;
                Bit7C(Temp);
                Temp = (Temp << 1) + SaveC;
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            case 0x79:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                SaveC = Flags.C;
                Bit7C(Temp);
                Temp = (Temp << 1) + SaveC;
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ROLA
            case 0x49:
                Temp = A;
                SaveC = Flags.C;
                Bit7C(A);
                Temp = (Temp << 1) + SaveC;
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ROLB
            case 0x59:
                Temp = B;
                SaveC = Flags.C;
                Bit7C(B);
                Temp = (Temp << 1) + SaveC;
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ROR
            case 0x66:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                SaveC = Flags.C;
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (SaveC << 7);
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            case 0x76:
                TempPC = ReadMem(PC + 1);
                TempPC = (Temp << 8) + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                SaveC = Flags.C;
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (SaveC << 7);
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            //RORA
            case 0x46:
                Temp = A;
                SaveC = Flags.C;
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (SaveC << 7);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //RORB
            case 0x56:
                Temp = B;
                SaveC = Flags.C;
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (SaveC << 7);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ASL
            case 0x68:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                Flags.C = (Temp >> 7) & 1;
                Temp = (Temp << 1);
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            case 0x78:
                TempPC = ReadMem(PC + 1);
                TempPC = TempPC * 0x100 + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                Flags.C = (Temp >> 7) & 1;
                Temp = (Temp << 1);
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ASLA
            case 0x48:
                Temp = A;
                Flags.C = (Temp >> 7) & 1;
                Temp = (Temp << 1);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ASLB
            case 0x58:
                Temp = B;
                Flags.C = (Temp >> 7) & 1;
                Temp = (Temp << 1);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ASR
            case 0x67:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (Temp & 0x80);
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            case 0x77:
                TempPC = ReadMem(PC + 1);
                TempPC = TempPC * 0x100 + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (Temp & 0x80);
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ASRA
            case 0x47:
                Temp = A;
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (Temp & 0x80);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //ASRB
            case 0x57:
                Temp = B;
                Flags.C = Temp & 1;
                Temp = (Temp >> 1) + (Temp & 0x80);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //LSR
            case 0x64:
                TempPC = IX + ReadMem(PC + 1);
                Temp = ReadMem(TempPC);
                Flags.C = Temp & 1;
                Temp = Temp >> 1;
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            case 0x74:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                Flags.C = Temp & 1;
                Temp = Temp >> 1;
                WriteMem(TempPC, Temp & 0xFF);
                TestN(ReadMem(TempPC));
                TestZ(ReadMem(TempPC));
                Flags.V = Flags.N ^ Flags.C;
                break;

            //LSRA
            case 0x44:
                Temp = A;
                Flags.C = Temp & 1;
                Temp = Temp >> 1;
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //LSRB
            case 0x54:
                Temp = B;
                Flags.C = Temp & 1;
                Temp = Temp >> 1;
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                Flags.V = Flags.N ^ Flags.C;
                break;

            //STAA
            case 0x97:
                WriteMem(Memory[PC + 1], A);
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                break;

            case 0xA7:
                TempPC = IX + ReadMem(PC + 1);
                WriteMem(TempPC, A);
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                //if(((TempPC <= 0xC16F) && (TempPC >= 0xC110)) && ((TempPC & 0xF)== 0)) Render(TempPC / 0x10 & 0xF, A);
                //if((TempPC <= 0xC16F) && (TempPC >= 0xC110)) Render(TempPC / 0x10 & 0xF, A);
                break;

            case 0xB7:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                WriteMem(TempPC, A);
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                //if((TempPC <= 0xC16F) && (TempPC >= 0xC110)) Render(TempPC / 0x10 & 0xF, A);
                break;

            //STAB
            case 0xD7:
                WriteMem(ReadMem(PC + 1), B);
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                break;

            case 0xE7:
                TempPC = IX + ReadMem(PC + 1);
                WriteMem(TempPC, B);
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                //if((TempPC <= 0xC16F) && (TempPC >= 0xC110)) Render(TempPC / 0x10 & 0xF, B);
                break;

            case 0xF7:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                WriteMem(TempPC, B);
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                // if((TempPC <= 0xC16F) && (TempPC >= 0xC110)) Render(TempPC / 0x10 & 0xF, B);
                break;

            //SUBA
            case 0x80:
            case 0x90:
            case 0xA0:
            case 0xB0:
                Temp = A;
                Temp -= Operand;

                if (Operand > A) Flags.C = 1; else Flags.C = 0;
                //TestC(A, Operand);
                TestV(A, Operand, Temp);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //SUBB
            case 0xC0:
            case 0xD0:
            case 0xE0:
            case 0xF0:
                Temp = B;
                Temp = Temp - Operand;

                if (Operand > B) Flags.C = 1; else Flags.C = 0;
                // TestC(B, Operand);


                TestV(B, Operand, Temp);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                break;

            //SBA
            case 0x10:
                Temp = A;
                Temp = Temp - B;
                TestC(A, B, Temp);
                TestV(A, B, Temp);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //SBCA
            case 0x82:
            case 0x92:
            case 0xA2:
            case 0xB2:
                Temp = A;
                Temp -= Operand;
                Temp -= Flags.C;

                if (Operand > A) Flags.C = 1; else Flags.C = 0;
                //TestC(A, Operand);
                TestV(A, Operand, Temp);
                A = Temp & 0xFF;
                TestN(A);
                TestZ(A);
                break;

            //SBCB
            case 0xC2:
            case 0xD2:
            case 0xE2:
            case 0xF2:
                Temp = B;
                Temp = Temp - Operand;
                Temp -= Flags.C;

                if (Operand > B) Flags.C = 1; else Flags.C = 0;
                // TestC(B, Operand);

                TestV(B, Operand, Temp);
                B = Temp & 0xFF;
                TestN(B);
                TestZ(B);
                break;

            //TAB
            case 0x16:
                B = A;
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                break;

            //TBA
            case 0x17:
                A = B;
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                break;

            //TST
            case 0x6D:
                Temp = ReadMem(IX + ReadMem(PC + 1));
                TestN(Temp);
                TestZ(Temp);
                Flags.V = 0;
                Flags.C = 0;
                break;

            case 0x7D:
                TempPC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                TestN(Temp);
                TestZ(Temp);
                Flags.V = 0;
                Flags.C = 0;
                break;

            //TSTA
            case 0x4D:
                TestN(A);
                TestZ(A);
                Flags.V = 0;
                Flags.C = 0;
                break;

            //TSTB
            case 0x5D:
                TestN(B);
                TestZ(B);
                Flags.V = 0;
                Flags.C = 0;
                break;

            //============ INDEX REGISTER AND STACK OPCODES ==========
            //CPX
            case 0x8C:
                Temp = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                Operand = Temp;
                Temp = IX - Temp;
                TestV(IX, Operand, Temp);
                TestN(Temp >> 8);
                TestZ(Temp);
                break;

            case 0x9C:
                Temp = (ReadMem(PC + 1) << 8) + ReadMem(ReadMem(PC + 1) + 1);
                Operand = Temp;
                Temp = IX - Temp;
                TestV(IX, Operand, Temp);
                TestN(Temp >> 8);
                TestZ(Temp);
                break;

            case 0xAC:
                Temp = (ReadMem(IX + ReadMem(PC + 1)) << 8) + ReadMem(IX + ReadMem(PC + 1) + 1);
                Operand = Temp;
                Temp = IX - Temp;
                TestV(IX, Operand, Temp);
                TestN(Temp >> 8);
                TestZ(Temp);
                break;

            case 0xBC:
                TempPC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                Temp = ReadMem(TempPC);
                Temp = (Temp << 8) + ReadMem(TempPC + 1);
                Operand = Temp;
                Temp = IX - Temp;
                TestV(IX, Operand, Temp);
                TestN(Temp >> 8);
                TestZ(Temp);
                break;

            //DEX
            case 0x09:
                IX--;
                TestZ(IX);
                break;

            //DES
            case 0x34:
                SP--;
                break;

            //INX
            case 0x8:
                IX++;
                TestZ(IX);
                break;

            //INS
            case 0x31:
                SP++;
                break;

            //LDX
            case 0xCE:
                //FOROPT
                IX = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;
                break;

            case 0xDE:
                //FOROPT
                IX = (ReadMem(ReadMem(PC + 1)) << 8) + ReadMem(ReadMem(PC + 1) + 1);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;
                break;

            case 0xEE:
                TempPC = IX + ReadMem(PC + 1);
                IX = (ReadMem(TempPC) << 8) + ReadMem(TempPC + 1);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;
                break;

            case 0xFE:
                TempPC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                IX = (ReadMem(TempPC) << 8) + ReadMem(TempPC + 1);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;

                //LDS
                break;
            case 0x8E:
                SP = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            case 0x9E:
                TempPC = ReadMem(PC + 1);
                SP = (ReadMem(TempPC) << 8) + ReadMem(TempPC + 1);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            case 0xAE:
                TempPC = IX + ReadMem(PC + 1);
                SP = ReadMem(TempPC);
                SP = (SP << 8) + ReadMem(TempPC + 1);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            case 0xBE:
                TempPC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                SP = (ReadMem(TempPC) << 8) + ReadMem(TempPC + 1);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            //STX
            case 0xDF:
                TempPC = ReadMem(PC + 1);
                WriteMem(TempPC, (IX >> 8) & 0xFF);
                WriteMem(TempPC + 1, IX & 0xFF);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;
                break;

            case 0xEF:
                TempPC = IX + ReadMem(PC + 1);
                WriteMem(TempPC, (IX >> 8) & 0xFF);
                WriteMem(TempPC + 1, IX & 0xFF);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;
                break;

            case 0xFF:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                WriteMem(TempPC, (IX >> 8) & 0xFF);
                WriteMem(TempPC + 1, IX & 0xFF);
                TestN(IX >> 8);
                TestZ(IX);
                Flags.V = 0;
                break;

            //STS
            case 0x9F:
                TempPC = ReadMem(PC + 1);
                WriteMem(TempPC, (SP / 0x100) & 0xFF);
                WriteMem(TempPC + 1, SP & 0xFF);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            case 0xAF:
                TempPC = IX + ReadMem(PC + 1);
                WriteMem(TempPC, (SP >> 8) & 0xFF);
                WriteMem(TempPC + 1, SP & 0xFF);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            case 0xBF:
                TempPC = ReadMem(PC + 1);
                TempPC = (TempPC << 8) + ReadMem(PC + 2);
                WriteMem(TempPC, (SP >> 8) & 0xFF);
                WriteMem(TempPC + 1, SP & 0xFF);
                TestN(SP >> 8);
                TestZ(SP);
                Flags.V = 0;
                break;

            //TXS
            case 0x35:
                SP = IX - 1;
                break;

            //TSX
            case 0x30:
                IX = SP + 1;
                break;

            //============ JUMP AND BRANCH OPCODES ==========
            //BRA
            case 0x20:
                PC += Operand;
                break;

            //BCC
            case 0x24:
                if (Flags.C == 0) PC += Operand;
                break;

            //BCS
            case 0x25:
                if (Flags.C == 1) PC += Operand;
                break;

            //BEQ
            case 0x27:
                if (Flags.Z == 1) PC += Operand;
                break;

            //BGE
            case 0x2C:
                if ((Flags.N ^ Flags.V) == 0) PC += Operand;
                break;

            //BGT
            case 0x2E:
                if ((Flags.Z | (Flags.N ^ Flags.V)) == 0) PC += Operand;
                break;

            //BHI
            case 0x22:
                if ((Flags.C | Flags.Z) == 0) PC += Operand;
                break;

            //BLE
            case 0x2F:
                if ((Flags.Z | (Flags.N ^ Flags.V)) == 1) PC += Operand;
                break;

            //BLS
            case 0x23:
                if ((Flags.C | Flags.Z) == 1) PC += Operand;
                break;

            //BLT
            case 0x2D:
                if ((Flags.N ^ Flags.V) == 1) PC += Operand;
                break;

            //BMI
            case 0x2B:
                if (Flags.N == 1) PC += Operand;
                break;

            //BNE
            case 0x26:
                if (Flags.Z == 0) PC += Operand;
                break;

            //BVC
            case 0x28:
                if (Flags.V == 0) PC += Operand;
                break;

            //BVS
            case 0x29:
                if (Flags.V == 1) PC += Operand;
                break;

            //BPL
            case 0x2A:
                if (Flags.N == 0) PC += Operand;
                break;

            //BSR
            case 0x8D:
                PC += 2;
                WriteMem(SP--, PC & 0xFF);
                WriteMem(SP--, (PC >> 8) & 0xFF);
                PC += Operand;
                break;

            //JMP
            case 0x6E:
                PC = IX + ReadMem(PC + 1);
                break;

            case 0x7E:
                PC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                break;

            //JSR
            case 0xAD:
                TempPC = PC;
                PC += 3;
                WriteMem(SP--, PC & 0xFF);
                WriteMem(SP--, (PC >> 8) & 0xFF);
                PC = IX + ReadMem(TempPC + 1);
                break;

            case 0xBD:
                TempPC = (ReadMem(PC + 1) << 8) + ReadMem(PC + 2);
                PC += 3;
                WriteMem(SP--, PC & 0xFF);
                WriteMem(SP--, (PC >> 8) & 0xFF);
                PC = TempPC;
                break;

            //NOP
            case 0x1:
                PC = PC + 1;
                break;

            //RTI
            case 0x3B:
                SP++;
                Flags.H = (ReadMem(SP) >> 6) & 1;
                Flags.I = (ReadMem(SP) >> 5) & 1;
                Flags.N = (ReadMem(SP) >> 4) & 1;
                Flags.Z = (ReadMem(SP) >> 3) & 1;
                Flags.V = (ReadMem(SP) >> 2) & 1;
                Flags.C = (ReadMem(SP) >> 1) & 1;
                SP++;
                B = ReadMem(SP);
                SP++;
                A = ReadMem(SP);
                SP += 2;
                IX = (ReadMem(SP - 1) << 8) + ReadMem(SP);
                SP += 2;
                PC = (ReadMem(SP - 1) << 8) + ReadMem(SP);
                break;

            //RTS
            case 0x39:
                SP += 2;
                PC = (ReadMem(SP - 1) << 8) + ReadMem(SP);
                break;

            //SWI
            case 0x3F:
                //erp!
                WriteMem(SP--, PC & 0xFF);
                WriteMem(SP--, (PC >> 8) & 0xFF);
                WriteMem(SP--, IX & 0xFF);
                WriteMem(SP--, (IX >> 8) & 0xFF);
                WriteMem(SP--, A);
                WriteMem(SP--, B);
                WriteMem(SP--, Flags.H * 32 + Flags.I * 16 + Flags.N * 8 + Flags.Z * 4 + Flags.V * 2 + Flags.C);
                Flags.I = 1;
                //SP = SP - 7;
                PC = (ReadMem(0xFFFA) << 8) + ReadMem(0xFFFB);
                break;

            //WAI
            case 0x3E:
                //if Flags.I = 1;
                //	Paused = False
                //Else
                //	Paused = True
                //End if
                break;

            //============ CONDITIONS CODE REGISTER OPCODES ==========
            //CLC
            case 0xC:
                Flags.C = 0;
                break;

            //CLI
            case 0xE:
                Flags.I = 0;
                break;

            //CLV
            case 0xA:
                Flags.V = 0;
                break;

            //SEC
            case 0xD:
                Flags.C = 1;
                break;

            //SEI
            case 0xF:
                Flags.I = 1;
                break;

            //SEV
            case 0xB:
                Flags.Z = 1;
                break;

            //TAP
            case 0x6:
                Flags.H = A / 32 & 1;
                Flags.I = A / 16 & 1;
                Flags.N = A / 8 & 1;
                Flags.Z = A / 4 & 1;
                Flags.V = A / 2 & 1;
                Flags.C = A & 1;
                break;

            //TPA
            case 0x7:
                A += Flags.H * 32;
                A += Flags.I * 16;
                A += Flags.N * 8;
                A += Flags.Z * 4;
                A += Flags.V * 2;
                A += Flags.C;

                break;
            //INVALID
            default:
                break;
        }

        NOPS = CalcNOPs(FetchCode);

        switch (FetchCode)
        {
            case 0x8D:
            case 0x6E:
            case 0x7E:
            case 0xAD:
            case 0xBD:
            case 0x39:
            case 0x3F:
            case 0x3B:
                // PC already calculated
                break;
            default:
                PC += NOPS;
                break;
        }

        //Safety check
        lastpc = PC;

        //Interrupt handlers
        if ((NMI == 0) || ((IRQ == 0) && (Flags.I == 0)))
        {
            WriteMem(SP, PC & 0xFF);
            WriteMem(SP - 1, PC / 0x100 & 0xFF);
            WriteMem(SP - 2, IX & 0xFF);
            WriteMem(SP - 3, IX / 0x100 & 0xFF);
            WriteMem(SP - 4, A);
            WriteMem(SP - 5, B);
            WriteMem(SP - 6, Flags.H * 32 + Flags.I * 16 + Flags.N * 8 + Flags.Z * 4 + Flags.V * 2 + Flags.C);
            SP = SP - 7;
            Flags.I = 1;
            PC = (ReadMem(0xFFFC) << 8) + ReadMem(0xFFFD);
        }

        return Cycles[FetchCode];
    }

    public void Quit()
    {
        running = false;
    }

    public void Reset()
    {
        reset = true;
    }

}