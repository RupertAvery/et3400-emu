namespace Core6800
{
    public partial class Cpu6800
    {
        private bool _isHalted = false;

        public Cpu6800()
        {
            Initialize();
        }

        public int Execute()
        {
            if (State.Reset == 0)
            {
                State.PC = RM16(0xFFFE);
                State.CC = 0xC0;
                SEI();
                State.WAI = false;
                State.NonMaskableInterrupt = 1;
                State.InterruptRequest = 1;
                State.ValidMemoryAddress = 0;
                State.BusAvailable = 0;
                State.ReadWrite = 1;
                State.Reset = 1;
            }

            if (_isHalted)
            {
                if (State.Halt == 1)
                {
                    _isHalted = false;
                    State.BusAvailable = 1;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (State.Halt == 0)
                {
                    _isHalted = true;
                    State.BusAvailable = 0;
                }
            }

            CheckInterrupts();


            // State.ITemp 

            //if (Breakpoint.Contains(State.PC))
            //{
            //    var x = 1;
            //    // Do something for a hardware breaskpoint
            //    // Log stuff

            //}

            //if (State.PC == 0xFDC2 && State.A == 0xF7)
            //{
            //    var x = 1;
            //    // Do something for a hardware breaskpoint
            //    // Log stuff

            //}

            //if (!((PC >= 0xFE3A) && (PC <= 0xFE4F))) // Ignore OUTCH
            //{
            //    System.Diagnostics.Debug.WriteLine(string.Format("PC:{0,4:X} A:{1,2:X} B:{2,2:X} IX:{3,4:X} SP:{4,4:X} CC:{5, 6}", PC, A, B, X, S, Convert.ToString(CC, 2)));
            //}

            if (State.WAI)
            {
                return 5;
            }
            else
            {
                int fetchCode = ReadMem(State.PC) & 0xff;
                State.PC++;
                InterpretOpCode(fetchCode);

                return cycles[fetchCode];
            }
        }

        private void CheckInterrupts()
        {
            if (State.NonMaskableInterrupt == 0)
            {
                PUSHWORD(State.PC);
                PUSHWORD(State.X);
                PUSHBYTE(State.A);
                PUSHBYTE(State.B);
                PUSHBYTE(State.CC);

                State.BusAvailable = 0;
                State.NonMaskableInterrupt = 1;
                State.WAI = false;
                SEI();
                State.PC = RM16(0xFFFC);
            }
            else if (State.SWI)
            {
                PUSHWORD(State.PC);
                PUSHWORD(State.X);
                PUSHBYTE(State.A);
                PUSHBYTE(State.B);
                PUSHBYTE(State.CC);
                State.SWI = false;
                SEI();
                State.PC = RM16(0xFFFA);
            }
            else if (State.InterruptRequest == 0 && (State.CC & 0x10) != 0x10)
            {
                PUSHWORD(State.PC);
                PUSHWORD(State.X);
                PUSHBYTE(State.A);
                PUSHBYTE(State.B);
                PUSHBYTE(State.CC);
                State.InterruptRequest = 1;
                State.PC = RM16(0xFFF8);
                State.BusAvailable = 0;
                SEI();
                State.WAI = false;
            }
        }

        public void NMI()
        {
            State.NonMaskableInterrupt = 0;
        }

        public void IRQ()
        {
            State.InterruptRequest = 0;
        }

        /// <summary>
        /// Initiates power-on sequence
        /// </summary>
        public void BootStrap()
        {
            State.Reset = 0;
            Execute();
        }

    }
}