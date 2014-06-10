using System.Diagnostics;
using System.Threading;

namespace Sharp6800.Trainer
{
    /// <summary>
    /// Implements a speed limiter that executes as many instructions in succession as possible
    /// before yielding processor time. Uses less CPU
    /// </summary>
    public class StandardRunner : TrainerRunnerBase
    {
        private object lockCycles = new object();

        public StandardRunner(Trainer trainer)
            : base(trainer)
        {
        }

        private int limit = 100;

        protected override void CheckSpeed()
        {
            lock (lockCycles)
            {
                CyclesPerSecond = _cycles;
                _cycles = 0;
            }

            //CyclesPerSecond = _cycles - _lastCycles;
            //var diff = (_trainer.Settings.ClockSpeed - CyclesPerSecond);
            RaiseTimerEvent();

            var delta = (_trainer.Settings.ClockSpeed - CyclesPerSecond) / 1000;
            Debug.WriteLine("delta: {0}, limit {1}, clock: {2}, cps: {3}", delta, limit, _trainer.Settings.ClockSpeed , CyclesPerSecond);
            limit += delta;

            sleeps = 0;
            //_lastCycles = _cycles;
        }

        protected override void EmuThread()
        {
            _running = true;
            var loopCycles = 0;

            while (_running)
            {
                int cycles = _trainer.Emulator.Execute();
                loopCycles += cycles;
                lock (lockCycles)
                {
                    _cycles += cycles;
                }


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