using System;
using System.Threading;
using ET3400.Common;

namespace ET3400.Threading
{
    public abstract class TrainerRunnerBase : ITrainerRunner, IDisposable
    {
        protected readonly Trainer.Trainer _trainer;
        protected long _cycles;
        protected Thread _runner;
        public int sleeps;
        protected ManualResetEvent resetEvent;
        private CancellationTokenSource _cancelSource;
        public bool Running { get; protected set; }
        public EventHandler<EventArgs> OnFrameComplete { get; set; }
        public long Cycles
        {
            get => _cycles;
        }

        protected TrainerRunnerBase(Trainer.Trainer trainer)
        {
            _trainer = trainer;
            resetEvent = new ManualResetEvent(false);
        }

        protected abstract void Run(CancellationToken cancellationToken);

        protected virtual void Init()
        {
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
            _runner = new Thread(() =>
            {
                Running = true;
                Run(_cancelSource.Token);
                Running = false;
                resetEvent.Set();
            });
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