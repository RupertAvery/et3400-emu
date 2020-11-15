using System;
using ET3400.Common;

namespace ET3400.Threading
{
    public interface ITrainerRunner : IDisposable
    {
        event OnTimerDelegate OnTimer;
        bool Running { get; }
        EventHandler<EventArgs> OnSleep { get; set; }
        void Stop(bool noWait = false);
        void Start();
        void Recalibrate();
        long Cycles { get;  }
    }
}