namespace Core6800
{
    public partial class Cpu6800
    {
        void TestH(int Acc, int Operand, int Temp)
        {
            Flags.H = (((Acc >> 3) & 1) & ((Operand >> 3) & 1)) | (((Operand >> 3) & 1) & ~((Temp >> 3) & 1)) | (~((Temp >> 3) & 1) & ((Acc >> 3) & 1));
        }

        void TestC(int Acc, int Operand, int Temp)
        {
            Flags.C = (((Acc >> 7) & 1) & ((Operand >> 7) & 1)) | (((Operand >> 7) & 1) & ~((Temp >> 7) & 1)) | (~((Temp >> 7) & 1) & ((Acc >> 7) & 1));
        }

        void TestV(int Acc, int Operand, int Temp)
        {
            Flags.V = (((Acc >> 7) & 1) & ~((Operand >> 7) & 1) & ~((Temp >> 7) & 1)) | (~((Acc >> 7) & 1) & ((Operand >> 7) & 1) & ((Temp >> 7) & 1));
        }

        void TestN(int Acc)
        {
            Flags.N = (Acc >> 7) & 1;
        }
        
        void TestZ(int Acc)
        {
            Flags.Z = Acc == 0 ? 1 : 0;
        }

        void Bit7V(int Acc)
        {
            Flags.V = Acc == 0x80 ? 1 : 0;
        }

        void TestZC(int Acc)
        {
            if (Acc == 0)
            {
                Flags.Z = 1;
                Flags.C = 0;
            }
            else
            {
                Flags.Z = 0; 
                Flags.C = 1;
            }
        }

        void Bit1C(int Acc)
        {
            Flags.C = (Acc & 0x01) == 0x01 ? 1 : 0;
        }

        void Bit7C(int Acc)
        {
            Flags.C = (Acc & 0x80) == 0x80 ? 1 : 0;
        }

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
}
