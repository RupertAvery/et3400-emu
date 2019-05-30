using Sharp6800.Common;

namespace Sharp6800.Trainer.Threads
{
    public interface ITrainerRunner
    {
        event OnTimerDelegate OnTimer;
        bool Running { get; }
        void Stop(bool noWait = false);
        void Start();
        void Recalibrate();
    }
}