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
        public TrainerSettings Settings { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private int GetMappedSpeed(int value)
        {
            return (int)Math.Pow(10, value);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Settings.ClockSpeed = GetMappedSpeed(hScrollBar1.Value);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            hScrollBar1.Value = 5;
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Settings.ClockSpeed = GetMappedSpeed(hScrollBar1.Value);
            label1.Text = string.Format("Clock Speed: {0} cycles/sec", Settings.ClockSpeed);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            hScrollBar1.Value = 5;
        }

        private void OUTCHCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.EnableOUTCHHack = OUTCHCheckBox.Checked;
        }
    }
}
