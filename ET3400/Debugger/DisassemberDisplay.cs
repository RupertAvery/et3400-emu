using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ET3400.Trainer;

namespace ET3400.Debugger
{
    public class DisassemberDisplay : IDisposable
    {
        private Dictionary<int, Color> colorLookup = new Dictionary<int, Color>();
        private SolidBrush _brush;
        private Font _font;
        private int _textheight = 20;
        private readonly ITrainer _trainer;
        private readonly IntPtr _targetWnd;
        private VScrollBar _scrollBar;
        private Bitmap _breakpointEnabledBitmap;
        private Bitmap _breakpointDisabledBitmap;
        private object lockObject = new object();
        private Control _target;
        private DisassemblyView currentView;

        public int ViewOffset { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int VisibleItems { get; private set; }

        public DisassemblyLine? SelectedLine { get; private set; }
        public DisassemblyLine? CurrentLine { get; private set; }
        public EventHandler<SelectLineEventArgs> OnSelectLine { get; set; }

        public DisassemblyLine? GetLine(int x, int y)
        {
            DisassemblyLine? line = null;
            var index = ViewOffset + y / _textheight;
            if (index < CurrentView.LineCount)
            {
                if (CurrentView.RequestLock())
                {
                    line = CurrentView.Lines[ViewOffset + y / _textheight];
                    CurrentView.ReleaseLock();
                }
            }

            return line;
        }

        public void SelectLine(int x, int y)
        {
            var index = ViewOffset + y / _textheight;
            if (index < CurrentView.LineCount)
            {
                if (CurrentView.RequestLock())
                {
                    SelectedLine = CurrentView.Lines[ViewOffset + y / _textheight];
                    CurrentView.ReleaseLock();
                }
            }
            else
            {
                SelectedLine = null;
            }
        }

        public void MoveUp()
        {
            if (SelectedLine.HasValue)
            {
                if (SelectedLine.Value.LineNumber - 1 >= 0)
                {
                    if (CurrentView.RequestLock())
                    {
                        SelectedLine = CurrentView.Lines[SelectedLine.Value.LineNumber - 1];
                        CurrentView.ReleaseLock();
                    }
                }
                EnsureVisble(SelectedLine.Value);
            }
        }

        public void MoveDown()
        {
            if (SelectedLine.HasValue)
            {
                if (SelectedLine.Value.LineNumber + 1 < CurrentView.LineCount)
                {
                    if (CurrentView.RequestLock())
                    {
                        SelectedLine = CurrentView.Lines[SelectedLine.Value.LineNumber + 1];
                        CurrentView.ReleaseLock();
                    }
                }
                EnsureVisble(SelectedLine.Value);
            }
        }

        //private List<DisassemblyView> _disassemblyViews;

        //public void AddDisassemblyView(DisassemblyView disassemblyView)
        //{
        //    _disassemblyViews.Add(disassemblyView);
        //}

        public DisassemblyView CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                Resize();
                _scrollBar.Value = 0;
            }
        }

        private void SetUpColors()
        {
            var isAtBreakPoint = 1;
            var isSelected = 2;
            var isCurrentPC = 4;

            var yellow = Color.FromArgb(0xff, 0xff, 0xf1, 0x81);

            var allFlags = yellow;
            var selectedAndCurrent = yellow;
            var breakAndCurrent = yellow;
            var breakAndSelected = Color.DarkRed;
            var @break = Color.DarkRed;
            var selected = Color.Blue;
            var current = yellow;

            colorLookup.Add(isAtBreakPoint | isSelected | isCurrentPC, allFlags);
            colorLookup.Add(isSelected | isCurrentPC, selectedAndCurrent);
            colorLookup.Add(isAtBreakPoint | isSelected, breakAndSelected);
            colorLookup.Add(isAtBreakPoint | isCurrentPC, breakAndCurrent);
            colorLookup.Add(isAtBreakPoint, @break);
            colorLookup.Add(isSelected, selected);
            colorLookup.Add(isCurrentPC, current);
        }

        private void LoadImages()
        {
            _breakpointEnabledBitmap = ET3400.Properties.Resources.BreakpointEnable_16x;
            _breakpointDisabledBitmap = ET3400.Properties.Resources.BreakpointDisable_16x;
        }

        public DisassemberDisplay(Control target, VScrollBar scrollBar, Trainer.Trainer trainer)
        {
            _trainer = trainer;
            _target = target;
            Resize();
            _scrollBar = scrollBar;
            _targetWnd = target.Handle;
            SetUpColors();
            _target.MouseClick += MouseClick;
            //_scrollBar.Scroll += Scroll;
            _scrollBar.ValueChanged += ValueChanged;

            LoadImages();
            _brush = new SolidBrush(Color.Black);
            _font = new Font("Courier New", 12, FontStyle.Regular);
            //_disassemblyViews = new List<DisassemblyView>();
        }


        //private void Scroll(object sender, ScrollEventArgs e)
        //{
        //    ViewOffset = _scrollBar.Value;
        //}

        private void ValueChanged(object sender, EventArgs e)
        {
            ViewOffset = _scrollBar.Value;
        }


        public DisassemblyLine? GetLineFromAddress(int address)
        {
            return CurrentView.GetLineFromAddress(address);
        }

        public void UpdateDisplay()
        {
            if (Monitor.TryEnter(lockObject, 100))
            {
                try
                {
                    if (IsDisposed) return;
                    if (CurrentView == null) return;

                    if (CurrentView.IsDirty)
                    {
                        CurrentView.Refresh();
                    }

                    using (var buffer = new Bitmap(Width, Height))
                    {
                        using (var g = Graphics.FromImage(buffer))
                        {
                            g.Clear(Color.White);

                            int lineNo = 0;
                            //var lastComment = "";

                            foreach (var line in CurrentView.Lines.Skip(ViewOffset).ToList())
                            {
                                var top = _textheight * lineNo;
                                var left = 2;

                                var breakPoint = _trainer.Breakpoints[line.Address];
                                var isBreakPoint = breakPoint != null;
                                var isBreakPointEnabled = isBreakPoint && breakPoint.IsEnabled && line.LineType == LineType.Assembly;
                                var isSelected = false;
                                var isCurrentPC = (!_trainer.IsRunning && line.Address == _trainer.State.PC);

                                if (SelectedLine.HasValue && line.LineNumber == SelectedLine.Value.LineNumber)
                                {
                                    isSelected = true;
                                }

                                if (isBreakPoint && line.LineType == LineType.Assembly)
                                {
                                    if (breakPoint.IsEnabled)
                                    {
                                        g.DrawImage(_breakpointEnabledBitmap, left, top + 2);
                                    }
                                    else
                                    {
                                        g.DrawImage(_breakpointDisabledBitmap, left, top + 2);
                                    }
                                }

                                left += 18;

                                if (isBreakPointEnabled || isSelected || isCurrentPC)
                                {
                                    var flags = (isBreakPointEnabled ? 1 : 0) +
                                                (isSelected ? 2 : 0) +
                                                (isCurrentPC ? 4 : 0);

                                    if (flags != 0)
                                    {
                                        Color highlightColor = colorLookup[flags];

                                        using (var brush = new SolidBrush(highlightColor))
                                        {
                                            g.FillRectangle(brush, left, top, Width, 20);
                                        }
                                    }
                                }

                                if (isCurrentPC)
                                {
                                    var bottomPosition = _textheight * (lineNo + 1) - 1;
                                    using (var pen = new Pen(Color.DarkGray))
                                    {
                                        g.DrawLine(pen, left, top, Width, top);
                                        g.DrawLine(pen, left, bottomPosition, Width, bottomPosition);
                                    }
                                }

                                DrawText(g, left, top, $"${line.Address:X4}:", Color.DarkBlue, isBreakPointEnabled, isSelected, isCurrentPC);

                                left += 70;

                                if (line.LineType == LineType.Comment)
                                {
                                    DrawText(g, left, top, line.Text, Color.Green, isBreakPointEnabled, isSelected, isCurrentPC);
                                }
                                else if (line.LineType == LineType.Assembly)
                                {
                                    DrawText(g, left, top, line.Opcodes, Color.Black, isBreakPointEnabled, isSelected, isCurrentPC);
                                    left += 110;
                                    DrawText(g, left, top, line.Instruction, Color.DarkBlue, isBreakPointEnabled, isSelected, isCurrentPC);
                                    left += 50;
                                    DrawText(g, left, top, line.Operands, Color.DarkRed, isBreakPointEnabled, isSelected, isCurrentPC);
                                }
                                else if (line.LineType == LineType.Data)
                                {
                                    DrawText(g, left, top, line.Text, Color.DarkRed, isBreakPointEnabled, isSelected, isCurrentPC);
                                }

                                lineNo++;

                                if (lineNo > VisibleItems) break;
                            }

                            //_disassemblyView.Refresh();
                        }

                        using (var p = Graphics.FromHwnd(_targetWnd))
                        {
                            p.DrawImage(buffer, 0, 0);
                        }
                    }

                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }

        private void DrawText(Graphics g, int x, int y, string text, Color defaultColor, bool isAtBreakPoint, bool isSelected, bool isCurrent)
        {
            var color = isCurrent ? Color.Black :
                isAtBreakPoint || isSelected ? Color.White: defaultColor;

            using (var brush = new SolidBrush(color))
            {
                g.DrawString(text, _font, brush, x, y);
            }
        }

        public void Dispose()
        {
            lock (lockObject)
            {
                IsDisposed = true;
                //_breakpointEnabledBitmap.Dispose();
                //_breakpointDisabledBitmap.Dispose();
                _target.MouseClick -= MouseClick;
                //_scrollBar.Scroll -= Scroll;
                _scrollBar.ValueChanged -= ValueChanged;

                _brush?.Dispose();
                _font?.Dispose();
            }
        }

        public bool IsDisposed { get; private set; }

        public void PageDown()
        {
            if (CurrentView.RequestLock())
            {
                var currentValue = _scrollBar.Value;
                currentValue += VisibleItems;
                if (currentValue > _scrollBar.Maximum)
                {
                    _scrollBar.Value = _scrollBar.Maximum;

                    SelectedLine = CurrentView.Lines[CurrentView.LineCount - 1];
                }
                else
                {
                    _scrollBar.Value = currentValue;
                    if (SelectedLine.Value.LineNumber + VisibleItems < CurrentView.LineCount)
                    {
                        try
                        {
                            SelectedLine = CurrentView.Lines[SelectedLine.Value.LineNumber + VisibleItems];
                        }
                        catch (Exception ex)
                        {
                            // TODO: Try to lock on disassembly updates, and then determine if code changes occured
                            // before the current line
                            Debug.WriteLine($"{CurrentView.Lines.Count}:{SelectedLine.Value.LineNumber - VisibleItems}");
                        }

                    }
                    else
                    {
                        SelectedLine = CurrentView.Lines[CurrentView.LineCount - 1];
                    }
                }
                CurrentView.ReleaseLock();
            }
        }

        public void PageUp()
        {
            if (CurrentView.RequestLock())
            {
                var currentValue = _scrollBar.Value;
                currentValue -= VisibleItems;
                if (currentValue < 0)
                {
                    _scrollBar.Value = 0;

                    SelectedLine = CurrentView.Lines[0];

                }
                else
                {
                    _scrollBar.Value = currentValue;
                    if (SelectedLine.Value.LineNumber - VisibleItems > 0)
                    {
                        try
                        {
                            SelectedLine = CurrentView.Lines[SelectedLine.Value.LineNumber - VisibleItems];
                        }
                        catch (Exception ex)
                        {
                            // TODO: Try to lock on disassembly updates, and then determine if code changes occured
                            // before the current line
                            Debug.WriteLine($"{CurrentView.Lines.Count}:{SelectedLine.Value.LineNumber - VisibleItems}");
                        }
                    }
                    else
                    {
                        SelectedLine = CurrentView.Lines[0];
                    }
                }
                CurrentView.ReleaseLock();
            }
        }

        public void EnsureVisble(DisassemblyLine? line)
        {
            if (!line.HasValue) return;

            var currentAddress = CurrentLine.HasValue ? CurrentLine.Value.Address : 0;

            var direction = Math.Abs(line.Value.Address - currentAddress);

            if (line.HasValue)
            {
                var offset = line.Value.LineNumber;
                var relativeOffset = offset - ViewOffset;

                if (relativeOffset >= VisibleItems)
                {
                    if (relativeOffset - VisibleItems < VisibleItems / 2)
                    {
                        _scrollBar.Value += relativeOffset - VisibleItems;
                    }
                    else
                    {
                        if (offset < _scrollBar.Maximum)
                        {
                            if (direction == -1)
                            {
                                _scrollBar.Value = offset - VisibleItems;
                            }
                            else
                            {
                                _scrollBar.Value = offset;
                            }
                        }
                    }
                }
                else if (relativeOffset < 0)
                {
                    if (relativeOffset < VisibleItems / 2)
                    {
                        _scrollBar.Value += relativeOffset;
                    }
                    else
                    {
                        if (offset > 0)
                        {
                            _scrollBar.Value = offset;
                        }
                    }
                }
            }
        }

        public DisassemblyLine? SelectAddress(int address)
        {
            var line = GetLineFromAddress(address);
            if (line != null)
            {
                SelectedLine = line;
            }

            return line;
        }

        public void Resize()
        {
            Width = _target.Width;
            Height = _target.Height;
            VisibleItems = Height / _textheight - 1;

            if (CurrentView != null)
            {
                _scrollBar.Maximum = CurrentView.LineCount - VisibleItems / 2;
            }
        }


        private void MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (e.X < 20)
                    {
                        var line = GetLine(e.X, e.Y);
                        if (line.HasValue)
                        {
                            _trainer.ToggleBreakPoint(line.Value.Address);
                            //_trainer.ToggleBreakPointEnabled(_disassemberDisplay.SelectedLine.Value.Address);
                            return;
                        }
                    }
                    else
                    {
                        SelectLine(e.X, e.Y);
                    }


                    break;
                case MouseButtons.Right:
                    SelectLine(e.X, e.Y);

                    if (SelectedLine.HasValue)
                    {
                        var memoryMap = _trainer.MemoryMapManager.GetMemoryMap(SelectedLine.Value.Address);

                        OnSelectLine?.Invoke(this, new SelectLineEventArgs(CurrentLine, memoryMap));
                        
                    }


                    break;
            }
        }

    }
}