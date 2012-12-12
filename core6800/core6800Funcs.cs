using System;
using System.Collections.Generic;
using System.Text;

public partial class core6800
{
    //    void Bit(byte A, byte n) { A = (A >> n) & (byte)1; }
    void TestH(int A, int Operand, int Temp) { Flags.H = (((A >> 3) & 1) & ((Operand >> 3) & 1)) | (((Operand >> 3) & 1) & ~((Temp >> 3) & 1)) | (~((Temp >> 3) & 1) & ((A >> 3) & 1)); }
    void TestC(int A, int Operand, int Temp) { Flags.C = (((A >> 7) & 1) & ((Operand >> 7) & 1)) | (((Operand >> 7) & 1) & ~((Temp >> 7) & 1)) | (~((Temp >> 7) & 1) & ((A >> 7) & 1)); }
    void TestV(int A, int Operand, int Temp) { Flags.V = (((A >> 7) & 1) & ~((Operand >> 7) & 1) & ~((Temp >> 7) & 1)) | (~((A >> 7) & 1) & ((Operand >> 7) & 1) & ((Temp >> 7) & 1)); }

    void TestN(int A) { Flags.N = (A >> 7) & 1; }
    void TestZ(int A) { if (A == 0) Flags.Z = 1; else Flags.Z = 0; }

    void Bit7V(int A) { if (A == 0x80) Flags.V = 1; else Flags.V = 0; }
    void TestZC(int A) { if (A == 0) { Flags.Z = 1; Flags.C = 0; } else { Flags.Z = 0; Flags.C = 1; } }

    void Bit1C(int A) { if ((A & 0x01) == 0x01) Flags.C = 1; else Flags.C = 0; }
    void Bit7C(int A) { if ((A & 0x80) == 0x80) Flags.C = 1; else Flags.C = 0; }



    private int CalcNOPs(int FetchCode)
    {
        int NOPS = 2;
        if ((FetchCode == 0x8C) || (FetchCode == 0xCE) || (FetchCode == 0x8E)) NOPS = 3;
        if (AddrMode[FetchCode] == INHERENT) NOPS = 1;
        if (AddrMode[FetchCode] == EXTENDED) NOPS = 3;
        if (FetchCode == 0x01) NOPS = 1;
        return NOPS;
    }

    public bool LoadRam { set { loadram = value; } } 

}
