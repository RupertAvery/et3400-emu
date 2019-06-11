using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ET3400.Common;
using ET3400.Debugger;
using ET3400.Debugger.Breakpoints;
using ET3400.Debugger.MemoryMaps;
using ET3400.Trainer;
using Timer = System.Threading.Timer;

namespace ET3400
{
    public partial class DebuggerView : Form
    {
        private readonly ET3400.Trainer.Trainer _trainer;
        private object lockObject = new object();
        private bool IsDisposing;

        private readonly MemoryDisplay _memoryDisplay;
        private readonly DisassemberDisplay _disassemberDisplay;

        private Control focusObject;
        private Timer _updateTimer;

        public ListViewItem FromMemoryMap(MemoryMap memoryMap)
        {
            var item = new ListViewItem()
            {
                Text = memoryMap.Description,
            };
            item.Tag = memoryMap;
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, memoryMap.Type.ToString()));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, memoryMap.Start.ToString("X4")));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, memoryMap.End.ToString("X4")));
            return item;
        }

        public DebuggerView(ET3400.Trainer.Trainer trainer)
        {
            InitializeComponent();
            //Application.AddMessageFilter(this);
            this.Closing += OnClosing;
            DasmViewPictureBox.MouseWheel += DasmViewPictureBoxOnMouseWheel;

            _memoryDisplay = new MemoryDisplay(MemoryViewPictureBox, MemoryViewScrollBar, trainer);
            _disassemberDisplay = new DisassemberDisplay(DasmViewPictureBox, DasmViewScrollBar, trainer);
            _disassemberDisplay.OnSelectLine += OnSelectLine;

            _trainer = trainer;

            _trainer.OnStop += OnStop;
            _trainer.OnStart += OnStart;
            _trainer.OnStep += OnStep;
            _trainer.Breakpoints.OnChange += OnChange;

            _trainer.MemoryMapEventBus.Subscribe(MapEventType.Add, AddMapEventAction);
            _trainer.MemoryMapEventBus.Subscribe(MapEventType.Update, UpdateMapEventAction);
            _trainer.MemoryMapEventBus.Subscribe(MapEventType.Remove, RemoveMapEventAction);
            _trainer.MemoryMapEventBus.Subscribe(MapEventType.Clear, ClearMapEventAction);

            var region = trainer.MemoryMapManager.GetRegion("RAM");

            foreach (var memoryMap in region.MemoryMapCollection)
            {
                RangesListView.Items.Add(FromMemoryMap(memoryMap));
            }

            _updateTimer = new Timer(state =>
            {
                UpdateDisplay();
            }, null, 0, Timeout.Infinite);


            UpdateButtonState();

            BreakpointsListView.Sorting = SortOrder.Ascending;
            BreakpointsListView.ListViewItemSorter = new BreakpointComparer();
            RangesListView.Sorting = SortOrder.Ascending;
            RangesListView.ListViewItemSorter = new MemoryMapComparer();

            var debuggerSettings = _trainer.Settings.DebuggerSettings;
            memoryToolStripMenuItem.Checked = debuggerSettings.ShowMemory;
            disassemblyToolStripMenuItem.Checked = debuggerSettings.ShowDisassembly;
            statusToolStripMenuItem.Checked = debuggerSettings.ShowStatus;

            UpdateFormSize();
        }

        private void OnSelectLine(object sender, SelectLineEventArgs e)
        {
            if (e.MemoryMap == null)
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
                switch (e.MemoryMap.Type)
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

        private void OnChange(object sender, BreakpointEventArgs e)
        {

            switch (e.EventType)
            {
                case BreakpointEventType.Add:
                    var lvi = new ListViewItem() { Text = $"${e.Address:X4}", Tag = e.Address };
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "Yes" });
                    BreakpointsListView.Items.Add(lvi);
                    break;
                case BreakpointEventType.Remove:
                    {
                        var item = BreakpointsListView.Items.Cast<ListViewItem>().FirstOrDefault(x => (int)x.Tag == e.Address);
                        if (item != null)
                        {
                            BreakpointsListView.Items.Remove(item);
                        }
                    }
                    break;
                case BreakpointEventType.Clear:
                    BreakpointsListView.Items.Clear();
                    break;
                case BreakpointEventType.Enable:
                    {
                        var item = BreakpointsListView.Items.Cast<ListViewItem>().FirstOrDefault(x => (int)x.Tag == e.Address);
                        if (item != null)
                        {
                            item.SubItems[1].Text = "Yes";
                        }
                    }
                    break;
                case BreakpointEventType.Disable:
                    {
                        var item = BreakpointsListView.Items.Cast<ListViewItem>().FirstOrDefault(x => (int)x.Tag == e.Address);
                        if (item != null)
                        {
                            item.SubItems[1].Text = "No";
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerable<ListViewItem> FindListViewItem(IEnumerable<MemoryMap> memoryMaps)
        {
            foreach (var memoryMap in memoryMaps)
            {
                foreach (ListViewItem item in RangesListView.Items)
                {
                    if (item.Tag == memoryMap)
                    {
                        yield return item;
                    }
                }
            }
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

            IsDisposing = true;

            _trainer.OnStop -= OnStop;
            _trainer.OnStart -= OnStart;
            _trainer.OnStep -= OnStep;

            _trainer.Breakpoints.OnChange -= OnChange;
            _disassemberDisplay.OnSelectLine -= OnSelectLine;

            _trainer.MemoryMapEventBus.Unsubscribe(MapEventType.Add, AddMapEventAction);
            _trainer.MemoryMapEventBus.Unsubscribe(MapEventType.Update, UpdateMapEventAction);
            _trainer.MemoryMapEventBus.Unsubscribe(MapEventType.Remove, RemoveMapEventAction);
            _trainer.MemoryMapEventBus.Unsubscribe(MapEventType.Clear, ClearMapEventAction);

            _updateTimer.Dispose();
            _memoryDisplay.Dispose();
            _disassemberDisplay.Dispose();

        }

        private void AddMapEventAction(IEnumerable<MemoryMap> memoryMaps)
        {
            foreach (var memoryMap in memoryMaps)
            {
                RangesListView.Items.Add(FromMemoryMap(memoryMap));
            }
        }

        private void UpdateMapEventAction(IEnumerable<MemoryMap> memoryMaps)
        {
            throw new NotImplementedException();
        }

        private void RemoveMapEventAction(IEnumerable<MemoryMap> memoryMaps)
        {
            foreach (var listViewItem in FindListViewItem(memoryMaps))
            {
                RangesListView.Items.Remove(listViewItem);
            }
        }

        private void ClearMapEventAction(IEnumerable<MemoryMap> memoryMaps)
        {
            foreach (ListViewItem listViewItem in RangesListView.Items)
            {
                RangesListView.Items.Remove(listViewItem);
            }
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

        private void UpdateDisplay()
        {
            try
            {
                _disassemberDisplay.UpdateDisplay();
                _memoryDisplay.UpdateDisplay();
                UpdateState();
            }
            finally
            {
                if (!IsDisposing)
                {
                    _updateTimer.Change(100, Timeout.Infinite);
                }
            }
        }

        private void UpdateState()
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke((Action)UpdateState);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                PCTextBox.Text = String.Format("{0:X4}", _trainer.State.PC);
                IXTextBox.Text = String.Format("{0:X4}", _trainer.State.X);
                SPTextBox.Text = String.Format("{0:X4}", _trainer.State.S);
                ACCATextBox.Text = String.Format("{0:X2}", _trainer.State.A);
                ACCBTextBox.Text = String.Format("{0:X2}", _trainer.State.B);
                var ccText = Convert.ToString(_trainer.State.CC, 2);
                CCTextBox.Text = new String('0', 8 - ccText.Length) + ccText;
            }
        }


        private void DasmViewPictureBoxOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            DasmViewScrollBar.SetDeltaValue(mouseEventArgs.Delta);
        }

        private void DebuggerView_Load(object sender, EventArgs e)
        {
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "RAM", Start = 0x0000, End = 0x07FF });
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "Keypad", Start = 0xC003, End = 0xC005 });
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "Display", Start = 0xC110, End = 0xC16F });
            MemToolStripComboBox.Items.Add(new MemoryRange() { Description = "ROM", Start = 0xFC00, End = 0xFFFF });
            MemToolStripComboBox.SelectedItem = MemToolStripComboBox.Items[0];

            DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "RAM", 0x0000, 0x07FF));
            //DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "Fantom II", 0x1400, 0x1BFF));
            //DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "TinyBasic", 0x1C00, 0x23FF));
            DasmToolStripComboBox.Items.Add(new DisassemblyView(_trainer, "ROM", 0xFC00, 0xFFFF));
            DasmToolStripComboBox.ComboBox.ValueMember = "Description";

            DasmToolStripComboBox.SelectedItem = DasmToolStripComboBox.Items[0];
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
            var address = _disassemberDisplay.SelectedLine.HasValue ? _disassemberDisplay.SelectedLine.Value.Address : 0;

            var addBreakpointDialog = new AddBreakpoint(address);

            addBreakpointDialog.StartPosition = FormStartPosition.CenterParent;
            var result = addBreakpointDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                address = addBreakpointDialog.StartAddress;
                if (_trainer.Breakpoints[address] == null)
                {
                    _trainer.Breakpoints.Add(address);
                }
            }

        }

        private void RemoveBreakpointButton_Click(object sender, EventArgs e)
        {
            if (BreakpointsListView.SelectedItems != null && BreakpointsListView.SelectedItems.Count > 0)
            {
                var address = (int)BreakpointsListView.SelectedItems[0].Tag;
                //if (MessageBox.Show($"Are you sure you want to remove the breakpoint at  {address}?",
                //        "Remove Breakpoint", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                _trainer.Breakpoints.Remove(address);
                //}
            }
        }

        private void ClearAllBreakpointsButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all breakpoints?", "Clear All Breakpoints",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                DialogResult.Yes)
            {
                _trainer.Breakpoints.Clear();
            }
        }

        private void GotoBreakpointButton_Click(object sender, EventArgs e)
        {
            GotoBreakpoint();
        }

        private void ShowAddDataRange()
        {
            var address = _disassemberDisplay.SelectedLine.HasValue
                ? _disassemberDisplay.SelectedLine.Value.Address
                : 0;

            var addRangeDialog = new AddDataRange(address, address + 1);
            addRangeDialog.StartPosition = FormStartPosition.CenterParent;
            var result = addRangeDialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                if (_trainer.MemoryMapManager.GetMemoryMap(addRangeDialog.StartAddress) != null)
                {
                    MessageBox.Show("The address range you selected already contains a memory map", "Add Memory Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

        private void RemoveRangeButton_Click(object sender, EventArgs e)
        {
            if (RangesListView.SelectedItems != null && RangesListView.SelectedItems.Count > 0)
            {
                var memoryMap = (MemoryMap)RangesListView.SelectedItems[0].Tag;
                if (MessageBox.Show($"Are you sure you want to remove the data range {memoryMap.Description}?",
                        "Remove Data Range", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _trainer.MemoryMapManager.RemoveMemoryMap(memoryMap);
                }
            }
        }

        private void ClearAllRangesButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all memory maps?", "Clear All Memory Maps",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                DialogResult.Yes)
            {
                _trainer.MemoryMapManager.GetRegion("RAM").MemoryMapCollection.Clear();
            }

        }

        private void GotoRangeButton_Click(object sender, EventArgs e)
        {
            GotoRange();
        }

        private void MemToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedRange = (MemoryRange)MemToolStripComboBox.SelectedItem;
            _memoryDisplay.MemoryRange = selectedRange;
        }

        private void DasmToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var view = (DisassemblyView)DasmToolStripComboBox.SelectedItem;
            _disassemberDisplay.CurrentView = view;
        }

        private void DebuggerView_Activated(object sender, EventArgs e)
        {
            Application.AddMessageFilter(this);
        }

        private void DebuggerView_Deactivate(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void AddRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRange();
        }

        private void RemoveRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveRange();
        }

        private void addCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddComment();
        }

        private void removeCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveComment();
        }

        private void AddRange()
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

        private void RemoveComment()
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

        private void RemoveRange()
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

        public void ShowAddComment()
        {
            var address = _disassemberDisplay.SelectedLine.HasValue ? _disassemberDisplay.SelectedLine.Value.Address : 0;
            var addRangeDialog = new AddComment(address);
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

        private void BreakpointsListView_DoubleClick(object sender, EventArgs e)
        {
            GotoBreakpoint();
        }

        private void RangesListView_DoubleClick(object sender, EventArgs e)
        {
            GotoRange();
        }

        private void GotoBreakpoint()
        {
            if (BreakpointsListView.SelectedItems != null && BreakpointsListView.SelectedItems.Count > 0)
            {
                var address = (int)BreakpointsListView.SelectedItems[0].Tag;
                EnsureVisible(address);
            }
        }

        private void GotoRange()
        {
            if (RangesListView.SelectedItems != null && RangesListView.SelectedItems.Count > 0)
            {
                var map = (MemoryMap)RangesListView.SelectedItems[0].Tag;
                EnsureVisible(map.Start);
            }
        }

        private void RangesListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    GotoRangeButton_Click(null, EventArgs.Empty);
                    break;
                case Keys.Delete:
                    RemoveRangeButton_Click(null, EventArgs.Empty);
                    break;
            }
        }

        public void RefreshDisassemblyViews()
        {
            foreach (DisassemblyView disasemblyView in DasmToolStripComboBox.Items)
            {
                disasemblyView.Refresh();
            }
        }

        private void ToggleBreakPoint()
        {
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                _trainer.ToggleBreakPoint(_disassemberDisplay.SelectedLine.Value.Address);
            }
        }

        private void ToggleBreakpointEnabled()
        {
            if (_disassemberDisplay.SelectedLine.HasValue)
            {
                _trainer.ToggleBreakPointEnabled(_disassemberDisplay.SelectedLine.Value.Address);
            }
        }

        private void BreakpointsListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    GotoBreakpointButton_Click(null, EventArgs.Empty);
                    break;
                case Keys.Delete:
                    RemoveBreakpointButton_Click(null, EventArgs.Empty);
                    break;
            }
        }

        private void DasmViewPictureBox_DoubleClick(object sender, EventArgs e)
        {
        }

        private void addDataRangeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowAddDataRange();
        }

        private void addCommentToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowAddComment();
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (memoryToolStripMenuItem.Checked && CheckCount()) return;
            memoryToolStripMenuItem.Checked = !memoryToolStripMenuItem.Checked;
            debuggerSettings.ShowMemory = memoryToolStripMenuItem.Checked;
            UpdateFormSize();
        }

        private void disassemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (disassemblyToolStripMenuItem.Checked && CheckCount()) return;
            disassemblyToolStripMenuItem.Checked = !disassemblyToolStripMenuItem.Checked;
            debuggerSettings.ShowDisassembly = disassemblyToolStripMenuItem.Checked;
            UpdateFormSize();
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusToolStripMenuItem.Checked && CheckCount()) return;
            statusToolStripMenuItem.Checked = !statusToolStripMenuItem.Checked;
            debuggerSettings.ShowStatus = statusToolStripMenuItem.Checked;
            UpdateFormSize();
        }


        private bool CheckCount()
        {
            var count = (memoryToolStripMenuItem.Checked ? 1 : 0)
                        + (disassemblyToolStripMenuItem.Checked ? 1 : 0)
                        + (statusToolStripMenuItem.Checked ? 1 : 0);

            return (count == 1);
        }

        private DebuggerSettings debuggerSettings
        {
            get => _trainer.Settings.DebuggerSettings;

        }

        private void UpdateFormSize()
        {
            _updateTimer.Change(Timeout.Infinite, Timeout.Infinite);

            if (debuggerSettings.FormHeight > 200)
            {
                Height = debuggerSettings.FormHeight;
            }

            var left = 12;
            Width = 0;

            MemoryGroupBox.Visible = debuggerSettings.ShowMemory;
            toolStripLabel1.Visible = debuggerSettings.ShowMemory;
            MemToolStripComboBox.Visible = debuggerSettings.ShowMemory;

            if (debuggerSettings.ShowMemory)
            {
                MemoryGroupBox.Left = left;
                left += MemoryGroupBox.Width + 10;
            }

            DisassemblyGroupBox.Visible = debuggerSettings.ShowDisassembly;
            toolStripLabel2.Visible = debuggerSettings.ShowDisassembly;
            DasmToolStripComboBox.Visible = debuggerSettings.ShowDisassembly;

            if (debuggerSettings.ShowDisassembly)
            {
                DisassemblyGroupBox.Left = left;
                left += DisassemblyGroupBox.Width + 10;
            }

            StatusGroupBox.Visible = debuggerSettings.ShowStatus;
            if (debuggerSettings.ShowStatus)
            {
                StatusGroupBox.Left = left;
                left += StatusGroupBox.Width + 10;
            }

            Width = left + 12;

            _updateTimer.Change(100, Timeout.Infinite);
        }

        private void DasmViewPictureBox_SizeChanged(object sender, EventArgs e)
        {
            _disassemberDisplay.Resize();
        }

        private void MemoryViewPictureBox_SizeChanged(object sender, EventArgs e)
        {
            _memoryDisplay.Resize();
        }

        private void DebuggerView_Shown(object sender, EventArgs e)
        {
        }

        private void DebuggerView_ResizeEnd(object sender, EventArgs e)
        {
            debuggerSettings.FormHeight = Height;
        }
    }
}

