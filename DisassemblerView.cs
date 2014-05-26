using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core6800;

namespace Sharp6800
{
    public partial class DisassemblerView : Form
    {
        public DisassemblerView()
        {
            InitializeComponent();
        }

        public DasmDisplay DasmDisplay { get; set; }

        public int[] Memory { get; set; }

        public Cpu6800State State { get; set; }

        private void DisassemblerView_Load(object sender, EventArgs e)
        {
            DasmDisplay = new DasmDisplay(pictureBox1);
        }

        public void Display()
        {
            DasmDisplay.Display(Memory, State, 0, 16);
        }
    }
}
