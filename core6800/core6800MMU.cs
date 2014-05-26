using System;

namespace Core6800
{
    public partial class Cpu6800
    {
        public Func<int, int> ReadMem { get; set; }
        public Action<int, int> WriteMem { get; set; }

    }
}
