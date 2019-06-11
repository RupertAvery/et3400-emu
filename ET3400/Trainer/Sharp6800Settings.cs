using System;
using System.IO;
using System.Text;

namespace ET3400.Trainer
{
    public enum ClockSpeedSetting
    {
        Low,
        High
    }

    public class DebuggerSettings
    {
        public Boolean ShowMemory { get; set; }
        public Boolean ShowDisassembly { get; set; }
        public Boolean ShowStatus { get; set; }
        public int FormHeight { get; set; }
    }

    public class ET3400Settings
    {
        private int _baseFrequency;
        private int _cpuPercent;
        private ClockSpeedSetting _clockSpeedSetting;

        public EventHandler SettingsUpdated { get; set; }

        public DebuggerSettings DebuggerSettings { get; set; }

        public ClockSpeedSetting ClockSpeedSetting
        {
            get => _clockSpeedSetting;
            private set
            {
                switch (value)
                {
                    case ClockSpeedSetting.Low:
                        _baseFrequency = 100_000;
                        break;
                    case ClockSpeedSetting.High:
                        _baseFrequency = 1_000_000;
                        break;
                }
                _clockSpeedSetting = value;
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public int ClockSpeed
        {
            get => (int)((_cpuPercent / 100f) * _baseFrequency);
        }

        public int BaseFrequency
        {
            get { return _baseFrequency; }
        }

        public int CpuPercent
        {
            get { return _cpuPercent; }
            set
            {
                _cpuPercent = value;
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }


        public ET3400Settings()
        {
            ResetSpeed(ClockSpeedSetting.Low);
            DebuggerSettings = new DebuggerSettings()
            {
                ShowMemory = true,
                ShowDisassembly = true,
                ShowStatus = true,
            };
        }

        public void ResetSpeed(ClockSpeedSetting clockSpeed)
        {
            ClockSpeedSetting = clockSpeed;
            CpuPercent = 100;
        }

        public static ET3400Settings Load(string path)
        {
            var lines = File.ReadAllLines(path);
            var instance = new ET3400Settings();

            foreach (var line in lines)
            {
                if (!line.StartsWith(";"))
                {
                    var setting = line.Split(new char[] { '=' });
                    if (setting.Length == 2)
                    {
                        var propName = setting[0].Trim();
                        var value = setting[1].Trim();
                        switch (propName)
                        {
                            case nameof(instance.ClockSpeedSetting):
                                if (value == "Low")
                                {
                                    instance.ClockSpeedSetting = ClockSpeedSetting.Low;
                                }
                                else if (value == "High")
                                {
                                    instance.ClockSpeedSetting = ClockSpeedSetting.High;
                                }
                                else
                                {
                                    instance.ClockSpeedSetting = ClockSpeedSetting.Low;
                                }
                                break;
                            case nameof(instance.CpuPercent):
                                try
                                {
                                    instance.CpuPercent = int.Parse(value);
                                }
                                catch
                                {
                                    //swallow
                                }
                                break;
                            case nameof(instance.DebuggerSettings.ShowMemory):
                                instance.DebuggerSettings.ShowMemory = value.ToLower() == "yes";
                                break;
                            case nameof(instance.DebuggerSettings.ShowDisassembly):
                                instance.DebuggerSettings.ShowDisassembly = value.ToLower() == "yes";
                                break;
                            case nameof(instance.DebuggerSettings.ShowStatus):
                                instance.DebuggerSettings.ShowStatus = value.ToLower() == "yes";
                                break;
                            case nameof(instance.DebuggerSettings.FormHeight):
                                try
                                {
                                    instance.DebuggerSettings.FormHeight = int.Parse(value);
                                }
                                catch
                                {
                                    //swallow
                                }
                                break;
                        }
                    }
                }
            }

            return instance;
        }

        public static void Save(ET3400Settings settings, string path)
        {
            var sb = new StringBuilder();

            sb.AppendLine("; ET3400 Configuration File");
            sb.AppendLine($"{nameof(settings.ClockSpeedSetting)} = {Enum.GetName(typeof(ClockSpeedSetting), settings.ClockSpeedSetting)}");
            sb.AppendLine($"{nameof(settings.CpuPercent)} = {settings.CpuPercent}");
            sb.AppendLine();
            sb.AppendLine("; Debugger Settings");
            sb.AppendLine($"{nameof(settings.DebuggerSettings.ShowMemory)} = {(settings.DebuggerSettings.ShowMemory ? "Yes" : "No")}");
            sb.AppendLine($"{nameof(settings.DebuggerSettings.ShowDisassembly)} = {(settings.DebuggerSettings.ShowDisassembly ? "Yes" : "No")}");
            sb.AppendLine($"{nameof(settings.DebuggerSettings.ShowStatus)} = {(settings.DebuggerSettings.ShowStatus ? "Yes" : "No")}");
            sb.AppendLine($"{nameof(settings.DebuggerSettings.FormHeight)} = {settings.DebuggerSettings.FormHeight}");

            File.WriteAllText(path, sb.ToString());
        }

    }
}