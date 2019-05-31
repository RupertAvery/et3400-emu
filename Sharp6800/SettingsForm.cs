using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sharp6800.Trainer;

namespace Sharp6800
{
    public partial class SettingsForm : Form
    {
        private TrainerSettings _settings;

        public SettingsForm(TrainerSettings settings)
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
            if (value >= 1_000_000)
            {
                return $"{Math.Round(value / 1_000_000f, 2, MidpointRounding.ToEven)}MHz";

            }
            else if (value >= 1_000)
            {
                return $"{Math.Round(value / 1_000f, 2, MidpointRounding.ToEven)}MHz";
            }
            else
            {
                return $"{value}Hz";
            }
        }

        private void UpdateFrequency()
        {
            var frequency = (int)((CpuPercentTrackBar.Value / 100f) * _settings.BaseFrequency);
            BaseFrequencyLabel.Text = Fix(_settings.BaseFrequency);
            FrequencyLabel.Text = Fix(frequency);
            _settings.SettingsUpdated -= SettingsUpdated;
            _settings.CpuPercent = CpuPercentTrackBar.Value;
            _settings.ClockSpeed = frequency;
            _settings.SettingsUpdated += SettingsUpdated;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
