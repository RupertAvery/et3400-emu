using System;
using Sharp6800.Common;

namespace Sharp6800.Trainer
{
    public class TrainerSettings
    {
        private int _clockSpeed;
        private bool _enableOutchHack;
        private EmulationModes _emulationMode;

        public TrainerSettings()
        {
            ClockSpeed = 100000;
            EmulationMode = EmulationModes.Regular;
            EnableOUTCHHack = true;
        }

        public bool EnableOUTCHHack
        {
            get => _enableOutchHack;
            set
            {
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
                _enableOutchHack = value;
            }
        }

        public int ClockSpeed
        {
            get => _clockSpeed;
            set
            {
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
                _clockSpeed = value;
            }
        }

        public EmulationModes EmulationMode
        {
            get => _emulationMode;
            set
            {
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
                _emulationMode = value;
            }
        }

        public EventHandler SettingsUpdated { get; set; }
    }
}