using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Windows.Forms;
using Sharp6800.Trainer;

namespace Sharp6800.Debugger
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

        //public int SelectedAddress { get; private set; }
        //public int SelectedOffset { get; set; }
        public int ViewOffset { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int VisibleItems { get; }

        public DisassemblyLine? SelectedLine { get; private set; }
        public DisassemblyLine? CurrentLine { get; private set; }

        public void SelectLine(int x, int y)
        {
            var index = ViewOffset + y / _textheight;
            if (index < CurrentView.LineCount)
            {
                SelectedLine = CurrentView.Lines[ViewOffset + y / _textheight];
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
                    SelectedLine = CurrentView.Lines[SelectedLine.Value.LineNumber - 1];
                }
                EnsureVisble(SelectedLine.Value, -1);
            }
        }

        public void MoveDown()
        {
            if (SelectedLine.HasValue)
            {
                if (SelectedLine.Value.LineNumber + 1 < CurrentView.LineCount)
                {
                    SelectedLine = CurrentView.Lines[SelectedLine.Value.LineNumber + 1];
                }
                EnsureVisble(SelectedLine.Value, +1);
            }
        }

        private List<DisassemblyView> _disassemblyViews;

        public void AddDisassemblyView(DisassemblyView disassemblyView)
        {
            _disassemblyViews.Add(disassemblyView);
        }


        public DisassemblyView CurrentView { get; set; }

        private void SetUpColors()
        {
            var isAtBreakPoint = 1;
            var isSelected = 2;
            var isCurrentPC = 4;

            var allFlags = Color.Yellow;
            var selectedAndCurrent = Color.Yellow;
            var breakAndSelected = Color.DarkRed;
            var breakAndCurrent = Color.Orange;
            var @break = Color.DarkRed;
            var selected = Color.Blue;
            var current = Color.Yellow;

            colorLookup.Add(isAtBreakPoint | isSelected | isCurrentPC, allFlags);
            colorLookup.Add(isSelected | isCurrentPC, selectedAndCurrent);
            colorLookup.Add(isAtBreakPoint | isSelected, breakAndSelected);
            colorLookup.Add(isAtBreakPoint | isCurrentPC, breakAndCurrent);
            colorLookup.Add(isAtBreakPoint, @break);
            colorLookup.Add(isSelected, selected);
            colorLookup.Add(isCurrentPC, current);
        }

        public DisassemberDisplay(Control target, VScrollBar scrollBar, Trainer.Trainer trainer)
        {
            _trainer = trainer;
            _scrollBar = scrollBar;
            Width = target.Width;
            Height = target.Height;
            VisibleItems = Height / _textheight - 1;
            _targetWnd = target.Handle;
            SetUpColors();
            _brush = new SolidBrush(Color.Black);
            _font = new Font("Courier New", 12, FontStyle.Regular);
            _disassemblyViews = new List<DisassemblyView>();
        }

        public DisassemblyLine? GetLineFromAddress(int address)
        {
            return CurrentView.GetLineFromAddress(address);
        }

        public void UpdateDisplay()
        {
            if (IsDisposed) return;
            if (CurrentView == null) return;

            if (CurrentView.IsDirty)
            {
                CurrentView.Refresh();
            }

            try
            {
                using (var buffer = new Bitmap(Width, Height))
                {
                    using (var g = Graphics.FromImage(buffer))
                    {
                        g.Clear(Color.White);

                        int lineNo = 0;
                        //var lastComment = "";

                        foreach (var line in CurrentView.Lines.Skip(ViewOffset).ToList())
                        {
                            var isAtBreakPoint = _trainer.Breakpoints.Contains(line.Address);
                            var isSelected = false;
                            var isCurrentPC = (!_trainer.Running && line.Address == _trainer.State.PC);

                            if (SelectedLine.HasValue && line.LineNumber == SelectedLine.Value.LineNumber)
                            {
                                isSelected = true;
                            }

                            if (isAtBreakPoint || isSelected || isCurrentPC)
                            {
                                var flags = (isAtBreakPoint ? 1 : 0) +
                                            (isSelected ? 2 : 0) +
                                            (isCurrentPC ? 4 : 0);

                                Color highlightColor = colorLookup[flags];

                                using (var brush = new SolidBrush(highlightColor))
                                {
                                    g.FillRectangle(brush, 0, 20 * lineNo, Width, 20);
                                }
                            }

                            var position = _textheight * lineNo;
                            var bottomPosition = _textheight * (lineNo + 1) - 1;

                            if (isCurrentPC)
                            {
                                using (var pen = new Pen(Color.DarkGray))
                                {
                                    g.DrawLine(pen, 0, position, Width, position);
                                    g.DrawLine(pen, 0, bottomPosition, Width, bottomPosition);
                                }
                            }

                            DrawHex(g, 2, position, $"${line.Address:X4}: {line.Text}", isAtBreakPoint, isSelected, isCurrentPC);

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
            catch (Exception e)
            {
            }
        }

        private void DrawText(Graphics g, int x, int y, string text)
        {
            g.DrawString(text, _font, _brush, x, y);
        }

        private void DrawHex(Graphics g, int x, int y, string asm, bool isAtBreakPoint, bool isSelected, bool isCurrent)
        {
            using (var brush = new SolidBrush(isAtBreakPoint && !isCurrent ? Color.White : Color.Black))
            {
                g.DrawString(asm, _font, brush, x, y);
            }
        }

        public void Dispose()
        {
            IsDisposed = true;
            _brush?.Dispose();
            _font?.Dispose();
        }

        public bool IsDisposed { get; private set; }

        public void PageDown()
        {
            var currentValue = _scrollBar.Value;
            currentValue += VisibleItems;
            if (currentValue > _scrollBar.Maximum)
            {
                _scrollBar.Value = _scrollBar.Maximum;
                //CurrentLine = CurrentView.Lines[ViewOffset + VisibleItems];
            }
            else
            {
                _scrollBar.Value = currentValue;
            }
        }

        public void PageUp()
        {
            var currentValue = _scrollBar.Value;
            currentValue -= VisibleItems;
            if (currentValue < 0)
            {
                _scrollBar.Value = 0;
                //CurrentLine = CurrentView.Lines[ViewOffset];
            }
            else
            {
                _scrollBar.Value = currentValue;
            }
        }

        public void EnsureVisble(int address, int direction)
        {
            var line = GetLineFromAddress(address);
        }

        public void EnsureVisble(DisassemblyLine? line, int direction)
        {
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
    }



}