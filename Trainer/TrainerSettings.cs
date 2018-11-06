using Sharp6800.Common;

namespace Sharp6800.Trainer
{
    public class TrainerSettings
    {
        public TrainerSettings()
        {
            ClockSpeed = 100000;
            EmulationMode = EmulationModes.Regular;
            EnableOUTCHHack = true;
        }

        public bool EnableOUTCHHack { get; set; }
        public int ClockSpeed { get; set; }
        public EmulationModes EmulationMode { get; set; }
    }
}