using System;
using System.Windows.Forms;

namespace Sharp6800
{
    public partial class MemoryView : Form
    {
        public MemoryView()
        {
            InitializeComponent();
        }

        public MemDisplay MemDisplay { get; set; }

        private void Memory_Load(object sender, EventArgs e)
        {
            MemDisplay = new MemDisplay(pictureBox1);
            comboBox1.Items.Add(new ComboBoxItem() { Description = "RAM ($0000)", Start = 0x0000 });
            comboBox1.Items.Add(new ComboBoxItem() { Description = "Keypad ($C003)", Start = 0xC003 });
            comboBox1.Items.Add(new ComboBoxItem() { Description = "Display ($C110)", Start = 0xC110 });
            comboBox1.Items.Add(new ComboBoxItem() { Description = "ROM ($FC00)", Start = 0xFC00 });
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MemDisplay.Start = ((ComboBoxItem)comboBox1.SelectedItem).Start;
            vScrollBar1.Value = 0;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            MemDisplay.Start = ((ComboBoxItem)comboBox1.SelectedItem).Start + vScrollBar1.Value * 8;
        }


    }

    public class ComboBoxItem
    {
        public string Description { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
