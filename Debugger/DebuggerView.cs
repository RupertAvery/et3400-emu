using System;
using System.Windows.Forms;
using Core6800;
using Sharp6800.Common;

namespace Sharp6800.Debugger
{
    public partial class DebuggerView : Form
    {
        public MemDisplay MemDisplay { get; set; }
        public DasmDisplay DasmDisplay { get; set; }
        public int[] Memory { get; set; }
        public Cpu6800State State { get; set; }

        public DebuggerView()
        {
            InitializeComponent();
            MemDisplay = new MemDisplay(MemeoryViewPictureBox);
            DasmDisplay = new DasmDisplay(DasmViewPictureBox);
        }

        private void DebuggerView_Load(object sender, EventArgs e)
        {
            MemoryViewComboBox.Items.Add(new ComboBoxItem() { Description = "RAM ($0000)", Start = 0x0000 });
            MemoryViewComboBox.Items.Add(new ComboBoxItem() { Description = "Keypad ($C003)", Start = 0xC003 });
            MemoryViewComboBox.Items.Add(new ComboBoxItem() { Description = "Display ($C110)", Start = 0xC110 });
            MemoryViewComboBox.Items.Add(new ComboBoxItem() { Description = "ROM ($FC00)", Start = 0xFC00 });
            MemoryViewComboBox.SelectedItem = MemoryViewComboBox.Items[0];

            DasmViewComboBox.Items.Add(new ComboBoxItem() { Description = "RAM ($0000)", Start = 0x0000 });
            DasmViewComboBox.Items.Add(new ComboBoxItem() { Description = "ROM ($FC00)", Start = 0xFC00 });
            DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[0];
        }

        public void Display()
        {
            DasmDisplay.Display(Memory);
        }

        private void MemoryViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MemDisplay.Start = ((ComboBoxItem)MemoryViewComboBox.SelectedItem).Start;
            MemoryViewScrollBar.Value = 0;
        }

        private void MemoryViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            MemDisplay.Start = ((ComboBoxItem)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DasmDisplay.Start = ((ComboBoxItem)DasmViewComboBox.SelectedItem).Start;
        }

        private void DasmViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            DasmDisplay.Offset = DasmViewScrollBar.Value;
        }


    }
}
