using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharp6800
{
    class SegDisplay
    {
        readonly IntPtr targetWnd;
        readonly int width, height;
        //readonly Image[] segments = new Image[8];
        readonly Image[] vt = new Image[2];
        readonly Image[] hr = new Image[2];
        readonly Image[] dp = new Image[2];
        //readonly Image bg;
        private object bglock = new object();


        public int[] Memory { get; set; }

        public SegDisplay(PictureBox target)
        {
            try
            {
                width = target.Width;
                height = target.Height;
                //target.Image = new Bitmap(target.Width, target.Height);

                targetWnd = target.Handle;
                target.Paint += (sender, args) => Repaint(args.Graphics);

                //segments[0] = Image.FromFile("display/g.png");
                //segments[1] = Image.FromFile("display/f.png");
                //segments[2] = Image.FromFile("display/e.png");
                //segments[3] = Image.FromFile("display/d.png");
                //segments[4] = Image.FromFile("display/c.png");
                //segments[5] = Image.FromFile("display/b.png");
                //segments[6] = Image.FromFile("display/a.png");
                //segments[7] = Image.FromFile("display/dp.png");
                //bg = Image.FromFile("display/bg.png");

                vt[0] = Image.FromFile("display/vtoff.png");
                vt[1] = Image.FromFile("display/vton.png");
                hr[0] = Image.FromFile("display/hroff.png");
                hr[1] = Image.FromFile("display/hron.png");
                dp[0] = Image.FromFile("display/dpoff.png");
                dp[1] = Image.FromFile("display/dpon.png");

            }
            catch (Exception)
            {
                throw new Exception("Error initializing the display. one or more segment image files may be missing.");
            }
        }

        public void Write(int loc, int value)
        {
            var buffer = new Bitmap(38, 54);
            int position = 6 - ((loc & 0xF0) >> 4);
            int segment = loc & 0x7;
            System.Diagnostics.Debug.WriteLine("{0:X4}: {1:X2} {2}", loc, value, segment);

            lock (bglock)
            {
                var g = Graphics.FromImage(buffer);
                //g.DrawImage(bg, 0, 5, 38, 54);
                //DrawSegData(g, 0, 5, value);
                DrawSingleSegData(g, 0, 5, segment, value);
                g.Dispose();
            }

            var graphics = Graphics.FromHwnd(targetWnd);
            graphics.DrawImage(buffer, 20 + position * 45, 5);
            graphics.Dispose();
        }

        public void Write(int loc, int value, Graphics graphics)
        {
            var buffer = new Bitmap(38, 54);
            int position = 6 - ((loc & 0xF0) >> 4);
            int segment = loc & 0x7;


            lock (bglock)
            {
                var g = Graphics.FromImage(buffer);
                //g.DrawImage(bg, 0, 5, 38, 54);
                //DrawSegData(g, 0, 5, value);
                DrawSingleSegData(g, 0, 5, segment, value);
                g.Dispose();
            }

            graphics.DrawImage(buffer, 20 + position * 45, 5);
        }

        private string[] flags = {"H", "I", "N", "Z", "V", "C"};
        public void Repaint(Graphics graphics)
        {
            for (int i = 0xC16F; i >= 0xC110; i--)
            {
                Write(i, Memory[i], graphics);
            }

            for (int j = 0; j < 6; j++)
            {
                graphics.DrawString(flags[j], new Font("Arial", 8), Brushes.White, 42 + j * 45, 62);
            }
        }

        //private void DrawSegData(Graphics g, int x, int y, int segdata)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        if (((segdata >> i) & 1) == 1)
        //        {
        //            g.DrawImage(segments[i], x, y, 38, 54);
        //        }
        //    }
        //}


        private void DrawSingleSegData(Graphics g, int x, int y, int segment, int segdata)
        {
            //dp 31, 42  7 .
            //a  11,  5  6 -
            //b  26, 11  1 |
            //c  25, 27  4 |
            //d   8, 42  3 -
            //e   4, 27  5 |
            //f   5, 11  2 |
            //g  11, 23  0 -
            var state = (segdata) & 1;

            switch (segment)
            {
                case 0:
                    g.DrawImage(hr[state], x + 11, y + 23, 16, 5);
                    break;
                case 1:
                    g.DrawImage(vt[state], x + 5, y + 11, 7, 13);
                    break;
                case 2:
                    g.DrawImage(vt[state], x + 4, y + 27, 7, 13);
                    break;
                case 3:
                    g.DrawImage(hr[state], x + 8, y + 42, 16, 5);
                    break;
                case 4:
                    g.DrawImage(vt[state], x + 25, y + 27, 7, 13);
                    break;
                case 5:
                    g.DrawImage(vt[state], x + 26, y + 11, 7, 13);
                    break;
                case 6:
                    g.DrawImage(hr[state], x + 11, y + 5, 16, 5);
                    break;
                case 7:
                    g.DrawImage(dp[state], x + 31, y + 42, 5, 5);
                    break;

            }

        }

    }
}
