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
    public partial class AddDataRange : Form
    {
        public int StartAddress { get; set; }
        public int EndAddress { get; set; }
        public RangeType RangeType { get; set; }
        public string Description { get; set; }

        public AddDataRange(int startAddress, int endAddress)
        {
            StartAddress = startAddress;
            EndAddress = endAddress;
            InitializeComponent();
            startTextBox.Text = "$" + startAddress.ToString("X4");
            endTextBox.Text = "$" + endAddress.ToString("X4");
            RangeType = RangeType.Data;
        }

        public AddDataRange()
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
            if (startTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter a start address", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (endTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter an end address", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            }
            catch (Exception exception)
            {
                MessageBox.Show("Please enter a valid start address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                if (endTextBox.Text.Trim().StartsWith("$"))
                {
                    EndAddress = Convert.ToInt32(endTextBox.Text.Trim().Substring(1), 16);
                }
                else if (startTextBox.Text.Trim().ToLower().StartsWith("0x"))
                {
                    StartAddress = Convert.ToInt32(startTextBox.Text.Trim().Substring(2), 16);
                }
                else
                {
                    EndAddress = Convert.ToInt32(endTextBox.Text.Trim());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Please enter a valid end address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (StartAddress > EndAddress)
            {
                MessageBox.Show("The end address must be larger than or equal to the start address", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (descriptionTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter a description", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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

            if (EndAddress < 0)
            {
                MessageBox.Show("The end address must be greater than $0000", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (EndAddress > 0xFFFF)
            {
                MessageBox.Show("The end address must be less than $FFFF", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Description = descriptionTextBox.Text.Trim();

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
