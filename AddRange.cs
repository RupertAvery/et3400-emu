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
    public partial class AddRange : Form
    {
        public int StartAddress { get; set; }
        public int EndAddress { get; set; }
        public RangeType RangeType { get; set; }
        public string Description { get; set; }

        public AddRange(int startAddress, int endAddress)
        {
            StartAddress = startAddress;
            EndAddress = endAddress;
            InitializeComponent();
            startTextBox.Text = "$" + startAddress.ToString("X4");
            endTextBox.Text = "$" + endAddress.ToString("X4");
            RangeType = RangeType.Data;
            dataRadioButton.Checked = true;
        }

        public AddRange()
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

                if (StartAddress < EndAddress)
                {
                    MessageBox.Show("The end address must be larger than or equal to the start address", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dataRadioButton.Checked)
                {
                    RangeType = RangeType.Data;
                }
                else
                {
                    RangeType = RangeType.Code;
                }

                if (descriptionTextBox.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please enter a description", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (StartAddress < 0)
                {
                    MessageBox.Show("The start address must be greater than 0", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (EndAddress < 0)
                {
                    MessageBox.Show("The start address must be less than $FFFF", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Description = descriptionTextBox.Text.Trim();

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
