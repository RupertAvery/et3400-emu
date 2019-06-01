using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp6800.Debugger;

namespace Sharp6800
{
    public partial class AddBreakpoint : Form
    {
        public int StartAddress { get; set; }

        public AddBreakpoint(int startAddress)
        {
            StartAddress = startAddress;
            InitializeComponent();
            startTextBox.Text = "$" + startAddress.ToString("X4");
        }

        public AddBreakpoint()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (startTextBox.Text.StartsWith("$"))
                {
                    StartAddress = Convert.ToInt32(startTextBox.Text.Trim().Substring(1), 16);
                }
                else if (startTextBox.Text.Trim().ToLower().StartsWith("0x"))
                {
                    StartAddress = Convert.ToInt32(startTextBox.Text.Trim().Substring(2), 16);
                }
                else
                {
                    StartAddress = Convert.ToInt32(startTextBox.Text.Trim());
                }

                if (StartAddress < 0)
                {
                    MessageBox.Show("The start address must be greater than $0000", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (StartAddress > 0xFFFF)
                {
                    MessageBox.Show("The start address must be less than $FFFF", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
