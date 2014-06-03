using System.Threading;

namespace Sharp6800.Trainer
{
    public class CycleExactRunner : TrainerRunnerBase
    {
        public CycleExactRunner(Trainer trainer)
            : base(trainer)
        {
        }

        private int spinTime = 100;

        protected override void CheckSpeed()
        {
            CyclesPerSecond = _cycles - _lastCycles;
            RaiseTimerEvent();
            if (CyclesPerSecond > _trainer.Settings.ClockSpeed)
            {
                spinTime += (CyclesPerSecond - _trainer.Settings.ClockSpeed) / 10;
            }
            else if (CyclesPerSecond < _trainer.Settings.ClockSpeed)
            {
                spinTime -= (_trainer.Settings.ClockSpeed - CyclesPerSecond) / 10;
            }

            if (spinTime > 25000) spinTime = 25000;
            if (spinTime < 1) spinTime = 1;
            sleeps = 0;
            _lastCycles = _cycles;
        }

        protected override void EmuThread()
        {
            _running = true;

            while (_running)
            {
                _cycles += _trainer.Emulator.Execute();
                Thread.SpinWait(spinTime);
            }
        }
    }
}