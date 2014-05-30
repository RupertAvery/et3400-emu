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
            DasmDisplay = new DasmDisplay(pictureBox1);
        }

        public DasmDisplay DasmDisplay { get; set; }

        public int[] Memory { get; set; }

        public Cpu6800State State { get; set; }

        private void DisassemblerView_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add(new ComboBoxItem() { Description = "RAM ($0000)", Start = 0x0000 });
            comboBox1.Items.Add(new ComboBoxItem() { Description = "ROM ($FC00)", Start = 0xFC00 });
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        public void Display()
        {
            DasmDisplay.Display(Memory);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            DasmDisplay.Offset = vScrollBar1.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DasmDisplay.Start = ((ComboBoxItem) comboBox1.SelectedItem).Start;
        }
    }
}
