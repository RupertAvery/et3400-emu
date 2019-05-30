using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Sharp6800.Trainer.Threads
{
    /// <summary>
    /// Implements a speed limiter that executes as many instructions in succession as possible
    /// before yielding processor time. Uses less CPU
    /// </summary>
    public class StandardRunner : TrainerRunnerBase
    {
        private object lockCycles = new object();
        protected Timer _timer;

        public StandardRunner(Trainer trainer)
            : base(trainer)
        {
        }

        private int limit = 100;
        private int referenceLimit = 0;

        protected void CheckSpeed()
        {
            if(!Running) return;

            lock (lockCycles)
            {
                CyclesPerSecond = _cycles;
                _cycles = 0;
            }

            //CyclesPerSecond = _cycles - _lastCycles;
            //var diff = (_trainer.Settings.ClockSpeed - CyclesPerSecond);
            RaiseTimerEvent();

            var delta = (_trainer.Settings.ClockSpeed - CyclesPerSecond) / 1000;
            Debug.WriteLine("delta: {0}, limit {1}, clock: {2}, cps: {3}", delta, limit, _trainer.Settings.ClockSpeed, CyclesPerSecond);
            limit += delta;
            var ratio = CyclesPerSecond / (float) _trainer.Settings.ClockSpeed;

            if (Math.Abs(ratio - 1.0) <= 0.02)
            {
                referenceLimit = limit;
                _timer?.Dispose();
            }
            if (limit < 0) limit = 0;
            sleeps = 0;
            //_lastCycles = _cycles;
        }

        protected override void Init()
        {
            if (referenceLimit == 0)
            {
                _timer = new Timer(state => CheckSpeed(), null, 0, 1000);
            }
        }

        public override void Recalibrate()
        {
            if (referenceLimit != 0)
            {
                referenceLimit = 0;
                Init();
            }
        }

        private int lastBreakPointPC = -1;

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }

        protected override void Run()
        {
            //Running = true;
            var loopCycles = 0;

            while (Running)
            {
                int cycles = _trainer.Emulator.PreExecute();
                
                if (_trainer.BreakpointsEnabled && _trainer.AtBreakPoint && lastBreakPointPC != _trainer.State.PC)
                {
                    Stop(true);
                    _trainer.StopExternal();
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
            resetEvent.Set();
        }
    }
}