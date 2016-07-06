namespace Core6800
{
    public class Cpu6800State
    {
        public Cpu6800State()
        {
            Halt = 1;
            Reset = 1;
            NonMaskableInterrupt = 1;
            InterruptRequest = 1;
        }

        public int A, B, CC;
        public int S, X, EAD;
        public int PC;

        public bool SWI;
        public bool WAI;
         
        public int NonMaskableInterrupt { get; set; }
        public int InterruptRequest { get; set; }
        public int Reset { get; set; }

        public int Halt { get; set; }
        public int ThreeStateControl { get; set; }
        public int DataBusEnable { get; set; }
        
        public int BusAvailable { get; set; }
        public int ValidMemoryAddress { get; set; }
        public int ReadWrite { get; set; }

    }
}