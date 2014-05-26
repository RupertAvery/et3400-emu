namespace Core6800
{
    public partial class Cpu6800
    {

        public Cpu6800()
        {
            Initialize();
        }

        public int Execute()
        {
            //if (Breakpoint.Contains(PC) || debug)
            //{
            //    // Do something for a hardware breaskpoint
            //    // Log stuff

            //}

            //if (!((PC >= 0xFE3A) && (PC <= 0xFE4F))) // Ignore OUTCH
            //{
            //    System.Diagnostics.Debug.WriteLine(string.Format("PC:{0,4:X} A:{1,2:X} B:{2,2:X} IX:{3,4:X} SP:{4,4:X} CC:{5, 6}", PC, A, B, X, S, Convert.ToString(CC, 2)));
            //}

            int fetchCode = ReadMem(State.PC) & 0xff;
            State.PC++;
            InterpretOpCode(fetchCode);

            return cycles[fetchCode];
        }

        public void Reset()
        {
            State.PC = (ReadMem(0xFFFE) << 8) + ReadMem(0xFFFF);
            //Paused = FALSE;
            SEI();
            //Flags.I = 1;
            //NMI = 1;
            //IRQ = 1;
        }


        public void ClearFlags()
        {
            State.CC = 0x00;
        }

    }
}