using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Core6800;
using Sharp6800.Common;
using Timer = System.Threading.Timer;

namespace Sharp6800.Debugger
{
    public partial class DebuggerView : Form
    {
        private readonly Trainer.Trainer _trainer;

        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private readonly MemoryDisplay _memoryDisplay;
        private readonly DisassemberDisplay _disassemberDisplay;

        private Control focusObject;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_MOUSEWHEEL = 0x20a;
        private const int WM_SYSKEYDOWN = 0x104;

        private bool hasFocus = true;
        private Timer _updateTimer;

        public DebuggerView(Trainer.Trainer trainer)
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            this.Closing += OnClosing;
            MemoryViewPictureBox.MouseWheel += MemoryViewPictureBoxOnMouseWheel;
            DasmViewPictureBox.MouseWheel += DasmViewPictureBoxOnMouseWheel;
            MemoryViewPictureBox.KeyDown += MemoryViewPictureBoxOnKeyDown;
            DasmViewPictureBox.KeyDown += DasmViewPictureBoxOnKeyDown;

            _memoryDisplay = new MemoryDisplay(MemoryViewPictureBox, trainer);
            _disassemberDisplay = new DisassemberDisplay(DasmViewPictureBox, DasmViewScrollBar, trainer);
            _trainer = trainer;

            _trainer.OnStop += (sender, args) => UpdateDebuggerState();

            _updateTimer = new System.Threading.Timer(state =>
            {
                UpdateDisplay();
            }, null, 0, 100);
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Application.RemoveMessageFilter(this);
            _updateTimer.Dispose();
            _memoryDisplay.Dispose();
            _disassemberDisplay.Dispose();
        }


        private void MemoryViewPictureBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {

        }

        private void DasmViewPictureBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            var oldValue = DasmViewScrollBar.Value;
            ;
            if (keyEventArgs.KeyCode == Keys.Down)
            {
                if (oldValue + 1 > DasmViewScrollBar.Maximum) return;
                DasmViewScrollBar.Value = oldValue + 1;
            }
            if (keyEventArgs.KeyCode == Keys.Up)
            {
                if (oldValue - 1 < 0) return;
                DasmViewScrollBar.Value = oldValue - 1;
            }
        }

        public void UpdateDebuggerState()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)UpdateDebuggerState);
            }
            else
            {
                if (_trainer.State.PC >= 0xFC00)
                {
                    DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[1];
                }
                else if (_trainer.State.PC < 0xFC00)
                {
                    DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[0];
                }

                var line = _disassemberDisplay.SelectAddress(_trainer.State.PC);
                _disassemberDisplay.EnsureVisble(line, +1);
            }
        }

        public void UpdateDisplay()
        {
            _disassemberDisplay.UpdateDisplay();
            _memoryDisplay.UpdateDisplay();

            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    PCTextBox.Text = String.Format("{0:X4}", _trainer.State.PC);
                    IXTextBox.Text = String.Format("{0:X4}", _trainer.State.X);
                    SPTextBox.Text = String.Format("{0:X4}", _trainer.State.S);
                    ACCATextBox.Text = String.Format("{0:X2}", _trainer.State.A);
                    ACCBTextBox.Text = String.Format("{0:X2}", _trainer.State.B);
                    var ccText = Convert.ToString(_trainer.State.CC, 2);
                    CCTextBox.Text = new String('0', 8 - ccText.Length) + ccText;
                }));
            }
            catch(Exception ex)
            {

            }
        }


        private void DasmViewPictureBoxOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            var delta = mouseEventArgs.Delta / 120;
            if (DasmViewScrollBar.Value - delta < 0)
            {
                DasmViewScrollBar.Value = 0;
            }
            else
                if (DasmViewScrollBar.Value - delta > 1000)
            {
                DasmViewScrollBar.Value = 1000;
            }
            else
            {
                DasmViewScrollBar.Value -= delta;
            }
        }

        private void MemoryViewPictureBoxOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            var delta = mouseEventArgs.Delta / 120;
            if (MemoryViewScrollBar.Value - delta < 0)
            {
                MemoryViewScrollBar.Value = 0;
            }
            else
                if (MemoryViewScrollBar.Value - delta > 1000)
            {
                MemoryViewScrollBar.Value = 1000;
            }
            else
            {
                MemoryViewScrollBar.Value -= delta;
            }
        }

        private void DebuggerView_Load(object sender, EventArgs e)
        {
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "RAM", Start = 0x0000, End = 1024 });
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "Keypad", Start = 0xC003, End = 0xC003 + 1024 });
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "Display", Start = 0xC110, End = 0xC110 + 1024 });
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "ROM", Start = 0xFC00, End = 0xFFFF });
            MemoryViewComboBox.Items.Add(new CustomMemoryRange(MemAddrTextBox));
            MemoryViewComboBox.SelectedItem = MemoryViewComboBox.Items[0];

            DasmViewComboBox.Items.Add(new DisassemblyView(_trainer, "RAM", 0x0000, 1024));
            DasmViewComboBox.Items.Add(new DisassemblyView(_trainer, "ROM", 0xFC00, 0xFFFF));
            //DasmViewComboBox.Items.Add(new CustomMemoryRange(DasmAddrTextBox));

            DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[0];
        }

        private void MemoryViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MemAddrTextBox.Text = "";
            _memoryDisplay.MemoryOffset = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start;
            MemoryViewScrollBar.Value = 0;
        }

        private void MemoryViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _memoryDisplay.MemoryOffset = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DasmAddrTextBox.Text = "";
            var view = (DisassemblyView)DasmViewComboBox.SelectedItem;
            _disassemberDisplay.CurrentView = view;
            //_disassemberDisplay.MemoryOffset = view.Start;
            DasmViewScrollBar.Maximum = view.LineCount - _disassemberDisplay.VisibleItems / 2;

            DasmViewScrollBar.Value = 0;
        }

        private void DasmViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _disassemberDisplay.ViewOffset = DasmViewScrollBar.Value;
        }

        private void MemoryViewScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _memoryDisplay.MemoryOffset = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _disassemberDisplay.ViewOffset = DasmViewScrollBar.Value;
        }

        private void DasmAddrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[2];
                _disassemberDisplay.ViewOffset = 0;
            }
        }

        private void MemAddrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MemoryViewComboBox.SelectedItem = MemoryViewComboBox.Items[4];
                _memoryDisplay.MemoryOffset = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;

            }
        }

        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    //
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtPoint(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(Cursor.Position));
            return null;
        }

        private void DasmViewPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _disassemberDisplay.SelectLine(e.X, e.Y);

                    break;
                case MouseButtons.Right:
                    _disassemberDisplay.SelectLine(e.X, e.Y);

                    if (_disassemberDisplay.SelectedLine.HasValue)
                    {
                        var memoryMap = _trainer.GetMemoryMap(_disassemberDisplay.SelectedLine.Value.Address);
                        if (memoryMap == null)
                        {
                            addRangeToolStripMenuItem.Enabled = true;
                            removeRangeToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            addRangeToolStripMenuItem.Enabled = false;
                            removeRangeToolStripMenuItem.Enabled = true;
                        }
                        disassemblerContextMenuStrip.Show(Cursor.Position);
                    }


                    break;
            }
        }

        private void AddRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hasFocus = false;
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                var addRangeDialog = new AddRange(_disassemberDisplay.SelectedLine.Value.Address, _disassemberDisplay.SelectedLine.Value.Address + 1);
                //addRangeDialog.Parent = this;
                addRangeDialog.StartPosition = FormStartPosition.CenterParent;
                var result = addRangeDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    _trainer.AddMemoryMap(addRangeDialog.StartAddress, addRangeDialog.EndAddress, addRangeDialog.RangeType, addRangeDialog.Description);
                }
            }
            hasFocus = true;
        }

        private void RemoveRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hasFocus = false;
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                var memoryMap = _trainer.GetMemoryMap(_disassemberDisplay.SelectedLine.Value.Address);

                if (memoryMap != null)
                {
                    if (MessageBox.Show($"Are you sure you want to remove the map {memoryMap.Description}?",
                            "Remove Memory Map", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _trainer.RemoveMemoryMap(memoryMap);
                    }
                }
            }
            hasFocus = true;
        }

        private void DasmViewPictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
