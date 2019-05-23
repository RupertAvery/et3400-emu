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

    public partial class Address : Form
    {
        public int StartAddress { get; private set; }
        public int EndAddress { get; private set; }
        public Address()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            fromTextBox.Text = "$0000";
            toTextBox.Text = "$001F";
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (fromTextBox.Text.StartsWith("$"))
                {
                    StartAddress = Convert.ToInt32(fromTextBox.Text.Trim().Substring(1), 16);
                }
                else if (fromTextBox.Text.Trim().ToLower().StartsWith("0x"))
                {
                    StartAddress = Convert.ToInt32(fromTextBox.Text.Trim().Substring(2), 16);
                }
                else
                {
                    StartAddress = Convert.ToInt32(fromTextBox.Text.Trim());
                }

                if (toTextBox.Text.Trim().StartsWith("$"))
                {
                    EndAddress = Convert.ToInt32(toTextBox.Text.Trim().Substring(1), 16);
                }
                else if (fromTextBox.Text.Trim().ToLower().StartsWith("0x"))
                {
                    StartAddress = Convert.ToInt32(fromTextBox.Text.Trim().Substring(2), 16);
                }
                else
                {
                    EndAddress = Convert.ToInt32(toTextBox.Text.Trim());
                }

                if (StartAddress > EndAddress || StartAddress == EndAddress)
                {
                    throw new Exception("The end address must be larger than the start address");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
