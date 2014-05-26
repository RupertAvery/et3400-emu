using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharp6800
{
    public class MemDisplay
    {
        private readonly IntPtr targetWnd;
        private readonly int width, height;

        public MemDisplay(PictureBox target)
        {
            try
            {
                width = target.Width;
                height = target.Height;
                //target.Image = new Bitmap(target.Width, target.Height);

                targetWnd = target.Handle;


            }
            catch (Exception)
            {
                throw new Exception(
                    "Error initializing the display. one or more segment image files may be missing.");
            }
        }


        public void Display(int[] memory, int start, int lines)
        {
            var buffer = new Bitmap(width, height);
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.White);

            int j = 0;
            for (int i = start; i <= start + 8 * lines; i += 8)
            {
                DrawHex(g, 10, 20 * j, i + j * 8, memory);
                j++;
            }

            g.Dispose();

            var p = Graphics.FromHwnd(targetWnd);
            p.DrawImage(buffer, 0, 0);
            p.Dispose();
        }

        private void DrawHex(Graphics g, int x, int y, int start, int[] memory)
        {
            var s = string.Format("{0:X4} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2}",
                                  start,
                                  memory[start] & 0xff,
                                  memory[start + 1] & 0xff,
                                  memory[start + 2] & 0xff,
                                  memory[start + 3] & 0xff,
                                  memory[start + 4] & 0xff,
                                  memory[start + 5] & 0xff,
                                  memory[start + 6] & 0xff,
                                  memory[start + 7] & 0xff
                );
            var font = new Font("Courier New", 12, FontStyle.Regular);
            var brush = new SolidBrush(Color.Black);
            g.DrawString(s, font, brush, x, y);
        }
    }
}
