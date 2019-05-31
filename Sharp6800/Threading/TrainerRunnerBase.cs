using System;
using System.Threading;
using System.Windows.Forms;
using Sharp6800.Common;
using Timer = System.Threading.Timer;

namespace Sharp6800.Trainer.Threads
{
    public abstract class TrainerRunnerBase : ITrainerRunner, IDisposable
    {
        protected readonly Trainer _trainer;
        protected int _cycles;
        protected int _lastCycles;
        protected Thread _runner;
        public int sleeps;
        protected ManualResetEvent resetEvent;

        public event OnTimerDelegate OnTimer;

        public bool Running { get; protected set; }

        protected TrainerRunnerBase(Trainer trainer)
        {
            _trainer = trainer;
            resetEvent = new ManualResetEvent(false);
        }

        protected abstract void Run();

        protected virtual void Init()
        {

        }

        protected void RaiseTimerEvent(int cyclesPerSecond)
        {
            OnTimer?.Invoke(cyclesPerSecond);
        }

        /// <summary>
        /// Sets the quit flag on the emulator and waits for execution of the current opcode to complete
        /// </summary>
        public void Stop(bool noWait = false)
        {
            //if (!breakpoint)
            //{
            //    resetEvent.WaitOne();
            //    resetEvent.Reset();
            //}
            if (Running)
            {
                Running = false;
                if (!noWait)
                {
                    resetEvent.WaitOne();
                    resetEvent.Reset();
                }
            }
        }

        public void Start()
        {
            Init();
            _runner = new Thread(Run);
            Running = true;
            _runner.Start();
        }

        public virtual void Dispose()
        {
            Running = false;
        }

        public virtual void Recalibrate()
        {

        }
    }
}