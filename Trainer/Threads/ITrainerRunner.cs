using Sharp6800.Common;

namespace Sharp6800.Trainer.Threads
{
    public interface ITrainerRunner
    {
        event OnTimerDelegate OnTimer;
        void Quit();
        void Continue();
    }
}