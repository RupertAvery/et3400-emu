using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            InitializeComponent();
        }

        private int GetMappedSpeed(int value)
        {
            return (int)Math.Pow(10, value);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            _settings.ClockSpeed = GetMappedSpeed(hScrollBar1.Value);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            hScrollBar1.Value = 5;
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            _settings.ClockSpeed = GetMappedSpeed(hScrollBar1.Value);
            label1.Text = string.Format("Clock Speed: {0} cycles/sec", _settings.ClockSpeed);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            hScrollBar1.Value = 5;
            OUTCHCheckBox.Checked = true;
        }

        private void OUTCHCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.EnableOUTCHHack = OUTCHCheckBox.Checked;
        }
    }
}
