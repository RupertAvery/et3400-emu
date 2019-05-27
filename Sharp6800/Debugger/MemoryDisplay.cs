using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sharp6800.Common;

namespace Sharp6800.Debugger
{
    public class MemoryDisplay : IDisposable
    {
        private object _lockObject = new object();
        private readonly Trainer.Trainer _trainer;
        private readonly IntPtr targetWnd;
        private int _textheight = 20;
        private SolidBrush _brush;
        private Font _font;

        public int Width { get; set; }
        public int Height { get; set; }

        public int VisibleItems { get; }


        public bool IsDisposed { get; private set; }
        public MemoryRange MemoryRange { get; set; }
        public int MemoryOffset { get; set; }

        public MemoryDisplay(Control target, Trainer.Trainer trainer)
        {
            _trainer = trainer;
            Width = target.Width;
            Height = target.Height;
            targetWnd = target.Handle;
            VisibleItems = Height / _textheight - 1;

            _brush = new SolidBrush(Color.Black);
            _font = new Font("Courier New", 12, FontStyle.Regular);
        }

        private int Min(int a, int b)
        {
            if (a <= b) return a;
            return b;
        }

        public void UpdateDisplay()
        {
            if (MemoryRange == null) return;
            using (var buffer = new Bitmap(Width, Height))
            {
                using (var g = Graphics.FromImage(buffer))
                {
                    g.Clear(Color.White);

                    var j = 0;

                    var end = Min(MemoryRange.End, MemoryOffset + 8 * VisibleItems);

                    for (var address = MemoryOffset; address <= end; address += 8)
                    {
                        DrawHex(g, 10, 20 * j, address);
                        j++;
                    }
                }

                using (var p = Graphics.FromHwnd(targetWnd))
                {
                    p.DrawImage(buffer, 0, 0);
                }
            }
        }

        private void DrawHex(Graphics g, int x, int y, int address)
        {
            var k = 0;
            var s = new StringBuilder();

            if (address >= _trainer.Memory.Length) return;

            s.Append(string.Format("{0:X4}", address));

            while (address + k < _trainer.Memory.Length && k < 8)
            {
                s.Append(" " + string.Format("{0:X2}", _trainer.Memory[address + k] & 0xff));
                k++;
            }

            lock (_lockObject)
            {
                if (!IsDisposed)
                {
                    g.DrawString(s.ToString(), _font, _brush, x, y);
                }
            }
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                IsDisposed = true;
                _brush?.Dispose();
                _font?.Dispose();
            }
        }
    }
}
