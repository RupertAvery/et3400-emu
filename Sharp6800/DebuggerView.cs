using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
            //Application.AddMessageFilter(this);
            this.Closing += OnClosing;
            MemoryViewPictureBox.MouseWheel += MemoryViewPictureBoxOnMouseWheel;
            DasmViewPictureBox.MouseWheel += DasmViewPictureBoxOnMouseWheel;

            _memoryDisplay = new MemoryDisplay(MemoryViewPictureBox, trainer);
            _disassemberDisplay = new DisassemberDisplay(DasmViewPictureBox, DasmViewScrollBar, trainer);
            _trainer = trainer;

            _trainer.OnStop += OnStop;
            _trainer.OnStart += OnStart;
            _trainer.OnStep += OnStep;

            _updateTimer = new System.Threading.Timer(state =>
            {
                UpdateDisplay();
            }, null, 0, 100);

            UpdateButtonState();
        }

        private void OnStep(object sender, EventArgs e)
        {
            UpdateDissassemblyView();
        }

        private void OnStart(object sender, EventArgs eventArgs)
        {
            UpdateButtonState();
        }

        private void OnStop(object sender, EventArgs eventArgs)
        {
            UpdateButtonState();
            UpdateDissassemblyView();
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Application.RemoveMessageFilter(this);

            _trainer.OnStop -= OnStop;
            _trainer.OnStart -= OnStart;

            _updateTimer.Dispose();
            _memoryDisplay.Dispose();
            _disassemberDisplay.Dispose();
        }

        private void UpdateDissassemblyView()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)UpdateDissassemblyView);
            }
            else
            {
                EnsureVisible(_trainer.State.PC);
            }
        }

        private void EnsureVisible(int address)
        {
            foreach (var item in DasmToolStripComboBox.Items)
            {
                var view = (DisassemblyView)item;
                if (address >= view.Start && address <= view.End)
                {
                    DasmToolStripComboBox.SelectedItem = view;
                    break;
                }
            }

            var line = _disassemberDisplay.SelectAddress(address);
            _disassemberDisplay.EnsureVisble(line);
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
            catch
            {
                // swallow
            }
        }


        private void DasmViewPictureBoxOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            DasmViewScrollBar.SetDeltaValue(mouseEventArgs.Delta);
        }

        private void MemoryViewPictureBoxOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            MemoryViewScrollBar.SetDeltaValue(mouseEventArgs.Delta);
        }

        private void DebuggerView_Load(object sender, EventArgs e)
        {
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "RAM", Start = 0x0000, End = 0x07FF });
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "Keypad", Start = 0xC003, End = 0xC005 });
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "Display", Start = 0xC110, End = 0xC16F });
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "ROM", Start = 0xFC00, End = 0xFFFF });
            MemToolStripComboBox.SelectedItem = MemToolStripComboBox.Items[0];

            DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "RAM", 0x0000, 0x07FF));
            DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "Fantom II", 0x1400, 0x1BFF));
            DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "TinyBasic", 0x1C00, 0x23FF));
            DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "ROM", 0xFC00, 0xFFFF));
            DasmToolStripComboBox.ComboBox.ValueMember = "Description";

            DasmToolStripComboBox.SelectedItem = DasmToolStripComboBox.Items[0];
        }

        private void MemoryViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _memoryDisplay.MemoryOffset = ((MemoryRange)MemToolStripComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _disassemberDisplay.ViewOffset = DasmViewScrollBar.Value;
        }

        private void MemoryViewScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _memoryDisplay.MemoryOffset = ((MemoryRange)MemToolStripComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;
        }

        private void DasmViewScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _disassemberDisplay.ViewOffset = DasmViewScrollBar.Value;
        }

        private void DasmAddrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DasmToolStripComboBox.SelectedItem = DasmToolStripComboBox.Items[2];
                _disassemberDisplay.ViewOffset = 0;
            }
        }

        //private void MemAddrTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        MemToolStripComboBox.SelectedItem = MemToolStripComboBox.Items[4];
        //        _memoryDisplay.MemoryOffset = ((MemoryRange)MemToolStripComboBox.SelectedItem).Start + MemoryViewScrollBar.Value * 8;

        //    }
        //}

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
                        var memoryMap = _trainer.MemoryMapManager.GetMemoryMap(_disassemberDisplay.SelectedLine.Value.Address);
                        if (memoryMap == null)
                        {
                            addRangeToolStripMenuItem.Enabled = true;
                            addCommentToolStripMenuItem.Enabled = true;
                            removeRangeToolStripMenuItem.Enabled = false;
                            removeCommentToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            addRangeToolStripMenuItem.Enabled = false;
                            addCommentToolStripMenuItem.Enabled = false;
                            switch (memoryMap.Type)
                            {
                                case RangeType.Data:
                                    removeRangeToolStripMenuItem.Enabled = true;
                                    break;
                                case RangeType.Comment:
                                    removeCommentToolStripMenuItem.Enabled = true;
                                    break;
                            }
                        }
                        disassemblerContextMenuStrip.Show(Cursor.Position);
                    }


                    break;
            }
        }

        private void AddRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                var addRangeDialog = new AddDataRange(_disassemberDisplay.SelectedLine.Value.Address, _disassemberDisplay.SelectedLine.Value.Address + 1);
                //addRangeDialog.Parent = this;
                addRangeDialog.StartPosition = FormStartPosition.CenterParent;
                var result = addRangeDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    var memoryMap = new MemoryMap()
                    {
                        Start = addRangeDialog.StartAddress,
                        End = addRangeDialog.EndAddress,
                        Type = addRangeDialog.RangeType,
                        Description = addRangeDialog.Description
                    };

                    _trainer.MemoryMapManager.AddMemoryMap(memoryMap);
                }
            }
        }

        private void RemoveRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                var memoryMap = _trainer.MemoryMapManager.GetMemoryMap(_disassemberDisplay.SelectedLine.Value.Address);

                if (memoryMap != null)
                {
                    if (MessageBox.Show($"Are you sure you want to remove the data range {memoryMap.Description}?",
                            "Remove Data Range", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _trainer.MemoryMapManager.RemoveMemoryMap(memoryMap);
                    }
                }
            }
        }

        private void ResetMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (!_trainer.IsRunning)
            {
                _trainer.Start();
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (_trainer.IsRunning)
            {
                _trainer.Stop();
            }
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            if (!_trainer.IsRunning)
            {
                _trainer.Step();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (_trainer.IsRunning)
            {
                _trainer.Reset();
            }
        }

        private void UpdateButtonState()
        {
            if (InvokeRequired)
            {
                Invoke((Action)UpdateButtonState);
            }
            else
            {
                var running = _trainer.IsRunning;
                StartButton.Enabled = !running;
                StopButton.Enabled = running;
                StepButton.Enabled = !running;
                ResetButton.Enabled = running;
            }
        }

        private void AddBreakpointButton_Click(object sender, EventArgs e)
        {

        }

        private void RemoveBreakpointButton_Click(object sender, EventArgs e)
        {

        }

        private void ClearAllBreakpointsButton_Click(object sender, EventArgs e)
        {

        }

        private void GotoBreakpointButton_Click(object sender, EventArgs e)
        {

        }

        private void AddRangeButton_Click(object sender, EventArgs e)
        {

        }

        private void RemoveRangeButton_Click(object sender, EventArgs e)
        {

        }

        private void ClearAllRangesButton_Click(object sender, EventArgs e)
        {

        }

        private void GotoRangeButton_Click(object sender, EventArgs e)
        {

        }

        private void MemToolStripComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var selectedRange = (MemoryRange) MemToolStripComboBox.SelectedItem;
            _memoryDisplay.MemoryOffset = selectedRange.Start;
            _memoryDisplay.MemoryRange = selectedRange;

            var maxValue = (selectedRange.End - selectedRange.Start) / 8;

            MemoryViewScrollBar.Value = 0;

            if (_memoryDisplay.VisibleItems >= maxValue)
            {
                MemoryViewScrollBar.Maximum = 0;
                MemoryViewScrollBar.Enabled = false;
            }
            else
            {
                // WHY DOES VisibleItems / 2 WORK???
                MemoryViewScrollBar.Maximum = maxValue - _memoryDisplay.VisibleItems / 2;
                MemoryViewScrollBar.Enabled = true;
            }

        }

        private void DasmToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var view = (DisassemblyView)DasmToolStripComboBox.SelectedItem;
            _disassemberDisplay.CurrentView = view;
            //_disassemberDisplay.MemoryOffset = view.Start;
            DasmViewScrollBar.Maximum = view.LineCount - _disassemberDisplay.VisibleItems / 2;
            DasmViewScrollBar.Value = 0;
        }

        private void DasmViewPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void DebuggerView_Activated(object sender, EventArgs e)
        {
            Application.AddMessageFilter(this);
            Debug.WriteLine("Added message filter");
        }

        private void DebuggerView_Enter(object sender, EventArgs e)
        {
        }

        private void DebuggerView_Leave(object sender, EventArgs e)
        {
        }

        private void DebuggerView_Deactivate(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(this);
            Debug.WriteLine("Removed message filter");
        }

        private void addCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                var addRangeDialog = new AddComment(_disassemberDisplay.SelectedLine.Value.Address);
                //addRangeDialog.Parent = this;
                addRangeDialog.StartPosition = FormStartPosition.CenterParent;
                var result = addRangeDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    var memoryMap = new MemoryMap()
                    {
                        Start = addRangeDialog.StartAddress,
                        End = addRangeDialog.StartAddress,
                        Type = addRangeDialog.RangeType,
                        Description = addRangeDialog.Description
                    };

                    _trainer.MemoryMapManager.AddMemoryMap(memoryMap);
                }
            }
        }

        private void removeCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                var memoryMap = _trainer.MemoryMapManager.GetMemoryMap(_disassemberDisplay.SelectedLine.Value.Address);

                if (memoryMap != null)
                {
                    if (MessageBox.Show($"Are you sure you want to remove the comment {memoryMap.Description}?",
                            "Remove Comment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _trainer.MemoryMapManager.RemoveMemoryMap(memoryMap);
                    }
                }
            }
        }
    }
}
