using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sharp6800
{
    public partial class Goto : Form
    {
        public int Address { get; private set; }

        public Goto()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (toTextBox.Text.Trim().StartsWith("$"))
                {
                    Address = Convert.ToInt32(toTextBox.Text.Trim().Substring(1), 16);
                }
                else if (toTextBox.Text.Trim().ToLower().StartsWith("0x"))
                {
                    Address = Convert.ToInt32(toTextBox.Text.Trim().Substring(2), 16);
                }
                else
                {
                    Address = Convert.ToInt32(toTextBox.Text.Trim());
                }

                if (Address < 0)
                {
                    MessageBox.Show("The start address must be greater than 0", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Address < 0)
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
