using System;
using System.Diagnostics;
using System.Threading;

namespace ET3400.Threading
{
    /// <summary>
    /// A Runner that uses a separate thread and a semaphore along with QueryPerformanceCounter
    /// to accurately time frames length
    /// </summary>
    public class ThreadSyncRunner : TrainerRunnerBase
    {
        private readonly AutoResetEvent _threadSync = new AutoResetEvent(false);
        private Thread _timerThread;
        private int _lastBreakPointPc = -1;
        private double _nextFrameAt;
        private readonly double _secondsPerFrame = 1D / 60D;


        //private const int CyclesPerFrame = 16667; // 1MHz
        //private const int CyclesPerFrame = 33333; // 2MHz!
        //private const int CyclesPerFrame = 1667; // 100Khz better match for Rick's demos
        //private const int CyclesPerFrame = 8333; // 500Khz according to Heathkit manual

        private long _frames;
        //protected Timer _fpsTimer;
        //private long lastframes;
        //private long _lastCycles;
        //private long fps;

        public ThreadSyncRunner(Trainer.Trainer trainer)
            : base(trainer)
        {
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            _timerThread = new Thread(TimerThread);
            _timerThread.Start();

            //_fpsTimer = new Timer((o) =>
            //{
            //    fps = frames - lastframes;
            //    var cps = _cycles - _lastCycles;
            //    Debug.WriteLine($"FPS:{fps}, CPS:{cps}");
            //    _lastCycles = _cycles;
            //    lastframes = frames;
            //}, null, 1000, 1000);

            while (Running && !cancellationToken.IsCancellationRequested)
            {
                _threadSync.WaitOne();
                RunFrame();
                OnFrameComplete?.Invoke(this, new EventArgs());
                _frames++;
            }
        }

        private void RunFrame()
        {
            var frameCycles = _trainer.Settings.ClockSpeed / 60;

            while (frameCycles > 0)
            {
                int cycles = _trainer.Emulator.PreExecute();

                if (_trainer.BreakpointsEnabled && _trainer.AtBreakPoint && _lastBreakPointPc != _trainer.State.PC)
                {
                    Stop(true);
                    Running = false;
                    _trainer.RaiseStopEvent();
                    _lastBreakPointPc = _trainer.State.PC;
                }
                else
                {
                    _lastBreakPointPc = -1;
                }

                if (cycles == 0)
                {
                    cycles = _trainer.Emulator.PostExecute();
                }

                frameCycles -= cycles;
                _cycles += cycles;
            }
        }

        private void TimerThread(object state)
        {
            _nextFrameAt = PerformanceCounter.GetTime();

            while (Running)
            {
                double currentSec = PerformanceCounter.GetTime();

                if (currentSec - _nextFrameAt >= _secondsPerFrame)
                {
                    //double diff = currentSec - nextFrameAt;
                    //Log("Can't keep up! Skipping " + (int)(diff * 1000) + " milliseconds");
                    _nextFrameAt = currentSec;
                }

                if (currentSec >= _nextFrameAt)
                {
                    _nextFrameAt += _secondsPerFrame;

                    _threadSync.Set();
                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();
            _threadSync.Dispose();
            //_fpsTimer.Dispose();
        }
    }
}