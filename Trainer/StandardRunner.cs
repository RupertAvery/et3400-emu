using System.Threading;

namespace Sharp6800.Trainer
{
    public class StandardRunner : TrainerRunnerBase
    {
        public StandardRunner(Trainer trainer)
            : base(trainer)
        {
        }

        private int limit = 100;

        protected override void CheckSpeed()
        {
            CyclesPerSecond = _cycles - _lastCycles;
            RaiseTimerEvent();

            if (CyclesPerSecond > _trainer.Settings.ClockSpeed)
            {
                limit -= (CyclesPerSecond - _trainer.Settings.ClockSpeed) / 1000;
            }
            else if (CyclesPerSecond < _trainer.Settings.ClockSpeed)
            {
                limit += (_trainer.Settings.ClockSpeed - CyclesPerSecond) / 1000;
            }
            sleeps = 0;
            _lastCycles = _cycles;
        }

        protected override void EmuThread()
        {
            _running = true;
            var loopCycles = 0;

            while (_running)
            {
                var cycles = 0;
                cycles = _trainer.Emulator.Execute();
                loopCycles += cycles;
                _cycles += cycles;

                if (loopCycles > limit)
                {
                    loopCycles = 0;
                    Thread.Sleep(1);
                    sleeps++;
                }
            }
        }
    }
}