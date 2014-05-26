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
    public partial class Memory : Form
    {
        public Memory()
        {
            InitializeComponent();
        }

        public MemDisplay MemDisplay { get; set; }

        private void Memory_Load(object sender, EventArgs e)
        {
            MemDisplay = new MemDisplay(pictureBox1);
        }
    }
}
