using System;
using ET3400.Common;

namespace ET3400.Threading
{
    public interface ITrainerRunner : IDisposable
    {
        bool Running { get; }
        EventHandler<EventArgs> OnFrameComplete { get; set; }
        void Stop(bool noWait = false);
        void Start();
        void Recalibrate();
        long Cycles { get;  }
    }
}