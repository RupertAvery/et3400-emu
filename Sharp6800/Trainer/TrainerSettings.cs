using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sharp6800.Common;

namespace Sharp6800.Trainer
{
    public class TrainerSettings
    {
        private int _clockSpeed;
        private int _baseFrequency;
        private int _cpuPercent;
                
        public EventHandler SettingsUpdated { get; set; }

        public int ClockSpeed
        {
            get => _clockSpeed;
            set
            {
                _clockSpeed = value;
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public int BaseFrequency
        {
            get { return _baseFrequency; }
            set
            {
                _baseFrequency = value;
                SettingsUpdated?.Invoke(this, EventArgs.Empty);
            }
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


        public TrainerSettings()
        {
            BaseFrequency = 100000;
            ClockSpeed = 100000;
            CpuPercent = 100;
        }

        public static TrainerSettings Load(string path)
        {
            var props = typeof(TrainerSettings).GetProperties().Where(p => p.PropertyType == typeof(int) || p.PropertyType == typeof(string)).ToList();

            var lines = File.ReadAllLines(path);
            var instance = new TrainerSettings();

            foreach (var line in lines)
            {
                if (!line.StartsWith(";"))
                {
                    var setting = line.Split(new char[] { '=' });
                    if (setting.Length == 2)
                    {
                        var propName = setting[0].Trim();
                        var value = setting[1].Trim();
                        var property = props.FirstOrDefault(p => p.Name == propName);
                        if (property != null)
                        {
                            if (property.PropertyType == typeof(int))
                            {
                                try
                                {
                                    property.SetValue(instance, int.Parse(value));
                                }
                                catch
                                {
                                    //swallow
                                }
                            }
                            else
                            {
                                property.SetValue(instance, value);
                            }
                        }
                    }
                }
            }

            return instance;
        }

        public static void Save(TrainerSettings settings, string path)
        {
            var props = typeof(TrainerSettings).GetProperties().Where(p => p.PropertyType == typeof(int) || p.PropertyType == typeof(string)).ToList();

            var sb = new StringBuilder();

            sb.AppendLine("; Sharp6800 Configuration File");

            foreach (var property in props)
            {
                string value = "";
                if (property.PropertyType == typeof(int))
                {
                    value = property.GetValue(settings).ToString();
                }
                else
                {
                    value = (string)property.GetValue(settings);
                }

                sb.AppendLine($"{property.Name} = {value}");
            }

            File.WriteAllText(path, sb.ToString());
        }

    }
}