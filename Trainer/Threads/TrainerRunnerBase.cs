using System.Threading;
using System.Windows.Forms;
using Sharp6800.Common;
using Timer = System.Threading.Timer;

namespace Sharp6800.Trainer.Threads
{
    public abstract class TrainerRunnerBase : ITrainerRunner
    {
        public int CyclesPerSecond { get; protected set; }
        public event OnTimerDelegate OnTimer;
        protected readonly Trainer _trainer;

        protected TrainerRunnerBase(Trainer trainer)
        {
            _trainer = trainer;
        }

        protected bool _running;
        protected Timer _timer;
        private Timer _displayTimer;
        protected readonly object _lockobject = new object();
        protected int _cycles;
        protected int _lastCycles;
        protected Thread _runner;
        public int sleeps;

        protected abstract void CheckSpeed();

        protected abstract void EmuThread();

        protected void RaiseTimerEvent()
        {
            if (OnTimer != null) OnTimer(CyclesPerSecond);
        }

        /// <summary>
        /// Sets the quit flag on the emulator and waits for execution of the current opcode to complete
        /// </summary>
        public void Quit()
        {
            // wait for emulation thread to terminate
            _running = false;

            lock (_lockobject)
            {
                if (_runner != null)
                {
                    while (_runner.IsAlive)
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                }
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                if (_displayTimer != null)
                {
                    _displayTimer.Dispose();
                }
            }
        }

        public void Continue()
        {
            _runner = CreateThread();
            _timer = new Timer(state => CheckSpeed(), null, 0, 1000);
            _runner.Start();
        }

        protected Thread CreateThread()
        {
            return new Thread(EmuThread);
        }
    }
}