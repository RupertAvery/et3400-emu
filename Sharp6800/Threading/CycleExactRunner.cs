using System;
using System.Threading;

namespace Sharp6800.Trainer.Threads
{
    public class CycleExactRunner : TrainerRunnerBase
    {
        ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim();
        private int lastBreakPointPC = -1;

        public CycleExactRunner(Trainer trainer)
            : base(trainer)
        {
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            //Running = true;
            var loopCycles = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                int cycles = _trainer.Emulator.PreExecute();

                if (_trainer.BreakpointsEnabled && _trainer.AtBreakPoint && lastBreakPointPC != _trainer.State.PC)
                {
                    Stop(true);
                    _trainer.RaiseStopEvent();
                    lastBreakPointPC = _trainer.State.PC;
                    break;
                }
                else
                {
                    lastBreakPointPC = -1;
                }

                if (cycles == 0)
                {
                    cycles = _trainer.Emulator.PostExecute();
                }

                loopCycles += cycles;

                _cycles += cycles;

                var limit = _trainer.Settings.ClockSpeed / 60;

                if (loopCycles > limit)
                {
                    loopCycles = 0;
                    OnSleep?.Invoke(this, new EventArgs());
                    manualResetEventSlim.Wait(17);
                    sleeps++;
                }
            }
            resetEvent.Set();
            Running = false;
        }

    }
}