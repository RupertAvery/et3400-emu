using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sharp6800
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void About_Load(object sender, EventArgs e)
        {
            label1.Text = "Sharp6800 - ET3400 Emulator v" + Application.ProductVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
