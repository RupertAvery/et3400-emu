using Sharp6800.Common;

namespace Sharp6800.Trainer
{
    public interface ITrainerRunner
    {
        event OnTimerDelegate OnTimer;
        void Quit();
        void Continue();
    }
}