namespace Core6800
{
    public partial class Cpu6800
    {

        int[] OpCode = new int[256];
        int[] Cycles = new int[256];
        int[] AddrMode = new int[256];
        public int[] Memory = new int[65536];

        int ReadMem(int loc)
        {
            //if (loc > 0xFFFF)
            //{
            //    loc = loc;
            //}
            loc = loc & 0xFFFF;
            return Memory[loc];
        }


        void WriteMem(int loc, int value)
        {
            // Readonly access to ROM memory
            if (loc >= 0xFC00)
            {
                return;
            }
            if (loc == 0)
            {
                return;
            }
            Memory[loc] = value;
        }

    }
}
