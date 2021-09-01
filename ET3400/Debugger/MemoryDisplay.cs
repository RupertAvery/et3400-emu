using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ET3400.Common;

namespace ET3400.Debugger
{
    public class MemoryDisplay : IDisposable
    {
        private object _lockObject = new object();
        private readonly Trainer.Trainer _trainer;
        private readonly IntPtr targetWnd;
        private int _textheight = 20;
        private SolidBrush _brush;
        private Font _font;
        private Control _target;
        private readonly VScrollBar _scrollBar;
        private MemoryRange _memoryRange;

        public int Width { get; set; }
        public int Height { get; set; }
        public int VisibleItems { get; private set; }

        public bool IsDisposed { get; private set; }

        public MemoryRange MemoryRange
        {
            get => _memoryRange;
            set
            {
                _memoryRange = value;
                MemoryOffset = _memoryRange.Start;
                Resize();
                _scrollBar.Value = 0;
            }
        }

        public int MemoryOffset { get; set; }

        public MemoryDisplay(PictureBox target, VScrollBar scrollBar, Trainer.Trainer trainer)
        {
            _trainer = trainer;
            _target = target;
            _scrollBar = scrollBar;
            Resize();
            targetWnd = target.Handle;
            
            //_target.MouseWheel += OnMouseWheel;
            //_scrollBar.Scroll += Scroll;
            _scrollBar.ValueChanged += ValueChanged;

            _brush = new SolidBrush(Color.Black);
            _font = new Font("Courier New", 12, FontStyle.Regular);
        }

        //private void Scroll(object sender, ScrollEventArgs e)
        //{
        //    MemoryOffset = MemoryRange.Start + _scrollBar.Value * 8;
        //}

        private void ValueChanged(object sender, EventArgs e)
        {
            MemoryOffset = MemoryRange.Start + _scrollBar.Value * 8;
        }



        //private void OnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        //{
        //    _scrollBar.SetDeltaValue(mouseEventArgs.Delta);
        //}


        private int Min(int a, int b)
        {
            if (a <= b) return a;
            return b;
        }

        public void UpdateDisplay()
        {
            if (MemoryRange == null) return;
            try
            {
                using (var buffer = new Bitmap(Width, Height))
                {
                    using (var g = Graphics.FromImage(buffer))
                    {
                        g.Clear(Color.White);

                        var j = 0;

                        var end = Min(MemoryRange.End, MemoryOffset + 8 * VisibleItems);

                        for (var address = MemoryOffset; address <= end; address += 8)
                        {
                            var s = new StringBuilder();
                            var k = 0;

                            while (address + k < _trainer.Memory.Length && k < 8)
                            {
                                s.Append(" " + string.Format("{0:X2}", _trainer.Memory.ReadMem(address + k) & 0xff));
                                k++;
                            }

                            DrawText(g, 2, j * 20, $"${address:X4}:", Color.DarkBlue);
                            DrawText(g, 70, j * 20, s.ToString(), Color.DarkRed);

                            j++;
                        }
                    }

                    using (var p = Graphics.FromHwnd(targetWnd))
                    {
                        p.DrawImage(buffer, 0, 0);
                    }
                }
            }
            catch
            {
                // swallow
            }
        }

        private void DrawText(Graphics g, int x, int y, string s, Color color)
        {
            lock (_lockObject)
            {
                if (!IsDisposed)
                {
                    using (var brush = new SolidBrush(color))
                    {
                        g.DrawString(s, _font, brush, x, y);
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                IsDisposed = true;
                //_target.MouseWheel -= OnMouseWheel;
                //_scrollBar.Scroll -= Scroll;
                _scrollBar.ValueChanged -= ValueChanged;
                _brush?.Dispose();
                _font?.Dispose();
            }
        }

        public void Resize()
        {
            Width = _target.Width;
            Height = _target.Height;
            VisibleItems = Height / _textheight - 1;

            if (MemoryRange != null)
            {
                var maxValue = (MemoryRange.End - MemoryRange.Start) / 8;

                if (VisibleItems >= maxValue)
                {
                    _scrollBar.Maximum = 0;
                    _scrollBar.Enabled = false;
                }
                else
                {
                    // WHY DOES VisibleItems / 2 WORK???
                    _scrollBar.Maximum = maxValue - VisibleItems;
                    _scrollBar.Enabled = true;
                }
            }

        }
    }
}
