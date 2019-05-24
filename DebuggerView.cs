using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Core6800;
using Sharp6800.Common;

namespace Sharp6800.Debugger
{

    public partial class DebuggerView : Form, IMessageFilter
    {
        private readonly Trainer.Trainer _trainer;

        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private readonly MemDisplay _memDisplay;
        private readonly DasmDisplay _dasmDisplay;

        private Control focusObject;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_MOUSEWHEEL = 0x20a;
        private const int WM_SYSKEYDOWN = 0x104;

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_SYSKEYDOWN:
                    {
                        // Extract the keys being pressed
                        Keys keys = ((Keys)((int)m.WParam.ToInt64()));
                        switch (keys)
                        {
                            case Keys.F10:
                                if (!_trainer.Running)
                                {
                                    _trainer.Step();
                                    UpdateDebuggerState();
                                }
                                return true; // Prevent message reaching destination
                        }
                    }
                    break;
                case WM_MOUSEWHEEL:
                    {
                        // WM_MOUSEWHEEL, find the control at screen position m.LParam
                        Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                        IntPtr hWnd = WindowFromPoint(pos);
                        if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                        {
                            SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                            return true;
                        }
                    }
                    break;
                case WM_LBUTTONDOWN:
                    {
                        var lParam = ((int)m.LParam.ToInt64());
                        var pos = new Point()
                        {
                            X = lParam & 0xFFFF,
                            Y = (lParam >> 0x10) & 0xFFFF,
                        };
                        focusObject = FindControlAtPoint(this);
                        if (focusObject == MemoryViewPictureBox || focusObject == DasmViewPictureBox)
                        {
                            var x = DummyButton.Focus();
                        }
                    }

                    break;
                case WM_KEYDOWN:
                    {
                        // Extract the keys being pressed
                        Keys keys = ((Keys)((int)m.WParam.ToInt64()));

                        if (focusObject == MemoryViewPictureBox)
                        {
                            var oldValue = MemoryViewScrollBar.Value;
                            ;
                            switch (keys)
                            {
                                case Keys.Down:
                                    if (oldValue + 1 > MemoryViewScrollBar.Maximum)
                                    {
                                        MemoryViewScrollBar.Value = MemoryViewScrollBar.Maximum;
                                    }
                                    else
                                    {
                                        MemoryViewScrollBar.Value = oldValue + 1;
                                    }
                                    break;
                                case Keys.Up:
                                    if (oldValue - 1 < 0) return true;
                                    MemoryViewScrollBar.Value = oldValue - 1;
                                    break;
                                case Keys.PageDown:
                                    if (oldValue + 16 > MemoryViewScrollBar.Maximum)
                                    {
                                        MemoryViewScrollBar.Value = MemoryViewScrollBar.Maximum;
                                    }
                                    else
                                    {
                                        MemoryViewScrollBar.Value = oldValue + 16;
                                    }
                                    break;
                                case Keys.PageUp:
                                    if (oldValue - 16 < 0)
                                    {
                                        MemoryViewScrollBar.Value = 0;
                                    }
                                    else
                                    {
                                        MemoryViewScrollBar.Value = oldValue - 16;
                                    }
                                    break;
                            }

                        }
                        else if (focusObject == DasmViewPictureBox)
                        {
                            var oldValue = DasmViewScrollBar.Value;
                            ;
                            switch (keys)
                            {
                                case Keys.Down:
                                    if (_dasmDisplay.SelectedOffset < 0xFFFF)
                                    {
                                        _dasmDisplay.SelectedOffset++;
                                        if (_dasmDisplay.SelectedOffset > _dasmDisplay.Offset + 16)
                                        {
                                            DasmViewScrollBar.Value++;
                                        }
                                    }
                                    break;
                                case Keys.Up:
                                    if (_dasmDisplay.SelectedOffset > 0)
                                    {
                                        _dasmDisplay.SelectedOffset--;
                                        if (_dasmDisplay.SelectedOffset < _dasmDisplay.Offset)
                                        {
                                            DasmViewScrollBar.Value--;
                                        }
                                    }
                                    break;
                                case Keys.PageDown:
                                    if (oldValue + 16 > DasmViewScrollBar.Maximum)
                                    {
                                        DasmViewScrollBar.Value = DasmViewScrollBar.Maximum;
                                    }
                                    else
                                    {
                                        DasmViewScrollBar.Value = oldValue + 16;
                                    }

                                    _dasmDisplay.SelectedOffset = DasmViewScrollBar.Value;
                                    break;
                                case Keys.PageUp:
                                    if (oldValue - 16 < 0)
                                    {
                                        DasmViewScrollBar.Value = 0;
                                    }
                                    else
                                    {
                                        DasmViewScrollBar.Value = oldValue - 16;
                                    }
                                    _dasmDisplay.SelectedOffset = DasmViewScrollBar.Value;
                                    break;

                                case Keys.F4:
                                    if (_trainer.Running)
                                    {
                                        _trainer.Stop();
                                    }

                                    break;

                                case Keys.F5:
                                    if (!_trainer.Running)
                                    {
                                        _trainer.Start();
                                    }

                                    break;

                                case Keys.F9:
                                    _trainer.ToggleBreakPoint(_dasmDisplay.SelectedAddress);
                                    break;
                            }
                            return true; // Prevent message reaching destination

                        }
                        return false;
                    }
            }
            return false;
        }

        public DebuggerView(Trainer.Trainer trainer)
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            this.Closing += OnClosing;
            MemoryViewPictureBox.MouseWheel += MemoryViewPictureBoxOnMouseWheel;
            DasmViewPictureBox.MouseWheel += DasmViewPictureBoxOnMouseWheel;
            MemoryViewPictureBox.KeyDown += MemoryViewPictureBoxOnKeyDown;
            DasmViewPictureBox.KeyDown += DasmViewPictureBoxOnKeyDown;
            _memDisplay = new MemDisplay(MemoryViewPictureBox, trainer);
            _dasmDisplay = new DasmDisplay(DasmViewPictureBox, trainer);
            _trainer = trainer;
            _trainer.OnStop += (sender, args) => UpdateDebuggerState();
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Application.RemoveMessageFilter(this);
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
                if (_trainer.State.PC < 0xFC00)
                {
                    DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[0];
                }

                if (_trainer.State.PC > _dasmDisplay.LastAddress)
                {
                    var offset = _dasmDisplay.GetOffsetFromAddress(_trainer.State.PC);
                    if (offset < DasmViewScrollBar.Maximum)
                    {
                        DasmViewScrollBar.Value = offset;
                    }
                }

                if (_dasmDisplay.GetOffsetFromAddress(_trainer.State.PC) < _dasmDisplay.Offset)
                {
                    var offset = _dasmDisplay.GetOffsetFromAddress(_trainer.State.PC);
                    if (offset < DasmViewScrollBar.Maximum)
                    {
                        DasmViewScrollBar.Value = offset;
                    }
                }
            }
        }

        public void UpdateDisplay()
        {

            _dasmDisplay.Display();
            _memDisplay.Display();

            Invoke(new MethodInvoker(() =>
            {
                PCTextBox.Text = String.Format("{0:X4}", _trainer.State.PC);
                IXTextBox.Text = String.Format("{0:X4}", _trainer.State.X);
                SPTextBox.Text = String.Format("{0:X4}", _trainer.State.S);
                ACCATextBox.Text = String.Format("{0:X2}", _trainer.State.A);
                ACCBTextBox.Text = String.Format("{0:X2}", _trainer.State.B);
                var ccText = Convert.ToString(_trainer.State.CC, 2);

                CCTextBox.Text = new String('0', 8 - ccText.Length) + ccText;
                //CCHTextBox.Text = String.Format("{0}", (_trainer.State.CC & 0x20) >> 5);
                //CCITextBox.Text = String.Format("{0}", (_trainer.State.CC & 0x10) >> 4);
                //CCNTextBox.Text = String.Format("{0}", (_trainer.State.CC & 0x08) >> 3);
                //CCZTextBox.Text = String.Format("{0}", (_trainer.State.CC & 0x04) >> 2);
                //CCVTextBox.Text = String.Format("{0}", (_trainer.State.CC & 0x02) >> 1);
                //CCCTextBox.Text = String.Format("{0}", _trainer.State.CC & 0x01);
            }));
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
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "RAM ($0000)", Start = 0x0000, End = 1024 });
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "Keypad ($C003)", Start = 0xC003, End = 0xC003 + 1024 });
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "Display ($C110)", Start = 0xC110, End = 0xC110 + 1024 });
            MemoryViewComboBox.Items.Add(new MemoryRange() { Description = "ROM ($FC00)", Start = 0xFC00, End = 0xFFFF });
            MemoryViewComboBox.Items.Add(new CustomMemoryRange(MemAddrTextBox));
            MemoryViewComboBox.SelectedItem = MemoryViewComboBox.Items[0];

            DasmViewComboBox.Items.Add(new MemoryRange() { Description = "RAM ($0000)", Start = 0x0000, End = 1024 });
            DasmViewComboBox.Items.Add(new MemoryRange() { Description = "ROM ($FC00)", Start = 0xFC00, End = 0xFFFF });
            DasmViewComboBox.Items.Add(new CustomMemoryRange(DasmAddrTextBox));
            DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[0];
        }

        private void MemoryViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MemAddrTextBox.Text = "";
            _memDisplay.Start = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start;
            MemoryViewScrollBar.Value = 0;
        }

        private void MemoryViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _memDisplay.Start = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DasmAddrTextBox.Text = "";
            var memoryRange = ((MemoryRange)DasmViewComboBox.SelectedItem);
            DasmViewScrollBar.Value = 0;
            _dasmDisplay.Start = memoryRange.Start;
            DasmViewScrollBar.Maximum = memoryRange.End - memoryRange.Start - 16;
        }

        private void DasmViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _dasmDisplay.Offset = DasmViewScrollBar.Value;
        }

        private void MemoryViewScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _memDisplay.Start = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _dasmDisplay.Offset = DasmViewScrollBar.Value;
        }

        private void DasmAddrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DasmViewComboBox.SelectedItem = DasmViewComboBox.Items[2];
                _dasmDisplay.Start = ((MemoryRange)DasmViewComboBox.SelectedItem).Start;
                _dasmDisplay.Offset = 0;
            }
        }

        private void MemAddrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MemoryViewComboBox.SelectedItem = MemoryViewComboBox.Items[4];
                _memDisplay.Start = ((MemoryRange)MemoryViewComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;

            }
        }

        private void DasmViewPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void DasmViewPictureBox_MouseDown(object sender, MouseEventArgs e)
        {

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

        private void MemoryViewPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void DasmViewPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            _dasmDisplay.SelectedOffset = _dasmDisplay.Offset + e.Y / 20;

        }
    }
}
