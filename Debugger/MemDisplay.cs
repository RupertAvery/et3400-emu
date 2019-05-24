using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sharp6800.Debugger
{
    public class MemDisplay
    {
        private readonly Trainer.Trainer _trainer;
        private readonly IntPtr targetWnd;
        private readonly int width, height;

        public int Start { get; set; }
        public int Lines { get; set; }

        public MemDisplay(PictureBox target, Trainer.Trainer trainer)
        {
            _trainer = trainer;
            width = target.Width;
            height = target.Height;
            targetWnd = target.Handle;
            Lines = 16;
        }


        public void Display()
        {
            var buffer = new Bitmap(width, height);
            using (var g = Graphics.FromImage(buffer))
            {
                g.Clear(Color.White);

                var j = 0;
                for (var i = Start; i <= Start + 8 * Lines; i += 8)
                {
                    DrawHex(g, 10, 20 * j, i, _trainer.Memory);
                    j++;
                }
            }

            using (var p = Graphics.FromHwnd(targetWnd))
            {
                p.DrawImage(buffer, 0, 0);
            }
        }

        private void DrawHex(Graphics g, int x, int y, int start, int[] memory)
        {
            var k = 0;
            var s = new StringBuilder();
            if (start >= memory.Length) return;
            s.Append(string.Format("{0:X4}", start));

            while (start + k < memory.Length && k < 8)
            {
                s.Append(" " + string.Format("{0:X2}", memory[start + k] & 0xff));
                k++;
            }
 
            using (var brush = new SolidBrush(Color.Black))
            {
                using (var font = new Font("Courier New", 12, FontStyle.Regular))
                {
                    g.DrawString(s.ToString(), font, brush, x, y);
                }
            }
        }
    }
}
