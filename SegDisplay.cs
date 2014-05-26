using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Sharp6800
{
    class SegDisplay
    {
        readonly IntPtr targetWnd;
        readonly int width, height;
        readonly Image[] segments = new Image[8];
        readonly Image bg;

        public SegDisplay(PictureBox target)
        {
            try
            {
                width = target.Width;
                height = target.Height;
                //target.Image = new Bitmap(target.Width, target.Height);

                targetWnd = target.Handle;

                segments[0] = Image.FromFile("display/g.png");
                segments[1] = Image.FromFile("display/f.png");
                segments[2] = Image.FromFile("display/e.png");
                segments[3] = Image.FromFile("display/d.png");
                segments[4] = Image.FromFile("display/c.png");
                segments[5] = Image.FromFile("display/b.png");
                segments[6] = Image.FromFile("display/a.png");
                segments[7] = Image.FromFile("display/dp.png");
                bg = Image.FromFile("display/bg.png");

            }
            catch (Exception)
            {
                throw new Exception("Error initializing the display. one or more segment image files may be missing.");
            }
        }


        public void Display(int[] memory)
        {
            var buffer = new Bitmap(width, height);
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.Black);

            int j = 0;
            for (int i = 0xC160; i >= 0xC110; i -= 16)
            {
                int segdata = memory[i];
                g.DrawImage(bg, 20 + j * 45, 5, 38, 54);
                DrawSegData(g, 20 + j * 45, 5, segdata);
                j++;
                if (j > 8) j = 0;
            }

            g.Dispose();

            var p = Graphics.FromHwnd(targetWnd);
            p.DrawImage(buffer, 0, 0);
            p.Dispose();
        }

        private void DrawSegData(Graphics g, int x, int y, int segdata)
        {
            for (int i = 0; i < 8; i++)
            {
                if (((segdata / (int)Math.Pow(2, i)) & 1) == 1)
                {
                    g.DrawImage(segments[i], x, y, 38, 54);
                }
            }
        }
    }
}
