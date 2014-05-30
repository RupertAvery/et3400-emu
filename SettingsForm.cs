using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sharp6800
{
    public partial class SettingsForm : Form
    {
        public Trainer Trainer { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Trainer.ClockSpeed = hScrollBar1.Value;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            hScrollBar1.Value = Trainer.ClockSpeed;
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = string.Format("Clock Speed: {0} cycles/sec", hScrollBar1.Value);
            Trainer.ClockSpeed = hScrollBar1.Value;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            hScrollBar1.Value = Trainer.DefaultClockSpeed;
        }
    }
}
