using System;
using System.Threading;
using ET3400.Common;

namespace ET3400.Threading
{
    public abstract class TrainerRunnerBase : ITrainerRunner, IDisposable
    {
        protected readonly Trainer.Trainer _trainer;
        protected int _cycles;
        protected int _lastCycles;
        protected Thread _runner;
        public int sleeps;
        protected ManualResetEvent resetEvent;
        private CancellationTokenSource _cancelSource;

        public event OnTimerDelegate OnTimer;

        public bool Running { get; protected set; }
        public EventHandler<EventArgs> OnSleep { get; set; }

        protected TrainerRunnerBase(Trainer.Trainer trainer)
        {
            _trainer = trainer;
            resetEvent = new ManualResetEvent(false);
        }

        protected abstract void Run(CancellationToken cancellationToken);

        protected virtual void Init()
        {

        }

        protected void RaiseTimerEvent(int cyclesPerSecond)
        {
            OnTimer?.Invoke(cyclesPerSecond);
        }

        /// <summary>
        /// Sets the quit flag on the emulator and waits for execution of the current instruction to complete
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
                _cancelSource.Cancel();
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
            _cancelSource?.Dispose();
            _cancelSource = new CancellationTokenSource();
            _runner = new Thread(() => { Run(_cancelSource.Token); });
            Running = true;
            _runner.Start();
        }

        public virtual void Dispose()
        {
            Stop();
            _cancelSource?.Dispose();
        }

        public virtual void Recalibrate()
        {

        }
    }
}