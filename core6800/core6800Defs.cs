using System.Collections.Generic;

namespace Core6800
{
    public partial class Cpu6800
    {
        public struct FlagType
        {
            public int H;
            public int I;
            public int N;
            public int Z;
            public int V;
            public int C;
        }

        // Registers
        public int A, B;
        public int PC, SP, IX;
        public FlagType Flags;

        // housekeeping
        int Clock;
        int NMI;
        int IRQ;
        public List<int> Breakpoint = new List<int>();

        public delegate void InterruptHandler();

        public InterruptHandler Interrupt;
        int lastpc;
        bool running;
        bool debug;
        bool reset;
        bool loadram = false;
    }
}
