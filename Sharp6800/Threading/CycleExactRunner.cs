using System.Threading;

namespace Sharp6800.Trainer.Threads
{
    /// <summary>
    /// Attempts to spread out instructions by executing one then sleeping for a quantum 
    /// of time before executing the next instruction. Consumes more CPU asit does not alwyas
    /// yield
    /// </summary>
    public class CycleExactRunner : TrainerRunnerBase
    {
        public CycleExactRunner(Trainer trainer)
            : base(trainer)
        {
        }

        private int CyclesPerSecond;
        private int spinTime = 100;

        protected void CheckSpeed()
        {
            CyclesPerSecond = _cycles - _lastCycles;
            RaiseTimerEvent(CyclesPerSecond);
            spinTime += (CyclesPerSecond - _trainer.Settings.BaseFrequency) / 10;

            if (spinTime > 5000) spinTime = 5000;
            if (spinTime < 1) spinTime = 1;
            sleeps = 0;
            _lastCycles = _cycles;
        }

        protected override void Run()
        {
            Running = true;

            while (Running)
            {
                _cycles += _trainer.Emulator.Execute();
                Thread.SpinWait(spinTime);
            }
        }
    }
}