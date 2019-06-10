using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sharp6800.Trainer;

namespace Sharp6800
{
    public partial class SettingsForm : Form
    {
        private Sharp6800Settings _settings;

        public SettingsForm(Sharp6800Settings settings)
        {
            _settings = settings;
            this.Closing += OnClosing;
            _settings.SettingsUpdated += SettingsUpdated;
            InitializeComponent();
            SettingsUpdated(this, EventArgs.Empty);
            UpdateFrequency();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            _settings.SettingsUpdated -= SettingsUpdated;
        }


        private void SettingsUpdated(object sender, EventArgs e)
        {
            CpuPercentTrackBar.Value = _settings.CpuPercent;
            UpdateFrequency();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
        }

        private void CpuPercentTrackBar_Scroll(object sender, EventArgs e)
        {
            UpdateFrequency();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            CpuPercentTrackBar.Value = 100;
            UpdateFrequency();
        }

        private string Fix(int value)
        {
            //const float oneMega = 1048576f;
            //const float oneKilo = 1024f;
            const float oneMega = 1000000;
            const float oneKilo = 1000f;

            if (value >= oneMega)
            {
                return $"{Math.Round(value / oneMega, 2, MidpointRounding.ToEven)}MHz";

            }
            else if (value >= oneKilo)
            {
                return $"{Math.Round(value / oneKilo, 2, MidpointRounding.ToEven)}kHz";
            }
            else
            {
                return $"{value}Hz";
            }
        }

        private void UpdateFrequency()
        {
            _settings.SettingsUpdated -= SettingsUpdated;
            _settings.CpuPercent = CpuPercentTrackBar.Value;
            //var frequency = (int)((CpuPercentTrackBar.Value / 100f) * _settings.BaseFrequency);
            //_settings.ClockSpeed = frequency;
            _settings.SettingsUpdated += SettingsUpdated;

            BaseFrequencyLabel.Text = Fix(_settings.BaseFrequency);
            FrequencyLabel.Text = Fix(_settings.ClockSpeed);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
