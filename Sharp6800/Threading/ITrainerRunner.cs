using System;
using Sharp6800.Common;

namespace Sharp6800.Trainer.Threads
{
    public interface ITrainerRunner : IDisposable
    {
        event OnTimerDelegate OnTimer;
        bool Running { get; }
        EventHandler<EventArgs> OnSleep { get; set; }
        void Stop(bool noWait = false);
        void Start();
        void Recalibrate();
    }
}