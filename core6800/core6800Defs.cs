using System.Collections.Generic;

namespace Core6800
{
    public class Cpu6800State
    {
        public int A, B, CC;
        public int S, X, EAD;
        public int PC;
        public int NMI;
        public int IRQ;
    }

    public partial class Cpu6800
    {
        // Registers
        public Cpu6800State State;

        // housekeeping
        public List<int> Breakpoint = new List<int>();

    }
}
