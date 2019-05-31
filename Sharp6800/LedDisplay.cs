using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Sharp6800.Trainer;

namespace Sharp6800
{
    public class LedDisplay : IDisposable
    {
        private object bglock = new object();
        private readonly IMemory _trainer;
        private readonly IntPtr targetWnd;
        private readonly int width, height;
        private readonly Image[] vt = new Image[2];
        private readonly Image[] hr = new Image[2];
        private readonly Image[] dp = new Image[2];
        private readonly PictureBox _target;

        private string[] flags = { "H", "I", "N", "Z", "V", "C" };
        
        private System.Threading.Timer _updateTimer;

        public LedDisplay(PictureBox target, IMemory trainer)
        {
            _trainer = trainer;
            _target = target;

            _updateTimer = new System.Threading.Timer(state =>
            {
                Redraw();
            }, null, 0, 16);

            try
            {
                width = target.Width;
                height = target.Height;

                targetWnd = _target.Handle;
                _target.Paint += TargetOnPaint;

                vt[0] = Image.FromStream(ResourceHelper.GetEmbeddedResourceStream(typeof(LedDisplay).Assembly, "display/vtoff.png"));
                vt[1] = Image.FromStream(ResourceHelper.GetEmbeddedResourceStream(typeof(LedDisplay).Assembly, "display/vton.png"));
                hr[0] = Image.FromStream(ResourceHelper.GetEmbeddedResourceStream(typeof(LedDisplay).Assembly, "display/hroff.png"));
                hr[1] = Image.FromStream(ResourceHelper.GetEmbeddedResourceStream(typeof(LedDisplay).Assembly, "display/hron.png"));
                dp[0] = Image.FromStream(ResourceHelper.GetEmbeddedResourceStream(typeof(LedDisplay).Assembly, "display/dpoff.png"));
                dp[1] = Image.FromStream(ResourceHelper.GetEmbeddedResourceStream(typeof(LedDisplay).Assembly, "display/dpon.png"));
            }
            catch (Exception)
            {
                throw new Exception("Error initializing the display. one or more segment image files may be missing.");
            }
        }

        private void TargetOnPaint(object sender, PaintEventArgs e)
        {
            Repaint(e.Graphics);
        }

        public void Write(int address, int data)
        {
            //var buffer = new Bitmap(38, 54);
            //int position = 6 - ((loc & 0xF0) >> 4);
            //int segment = loc & 0x7;
            ////System.Diagnostics.Debug.WriteLine("{0:X4}: {1:X2} {2}", loc, value, segment);

            //lock (bglock)
            //{
            //    var g = Graphics.FromImage(buffer);
            //    //g.DrawImage(bg, 0, 5, 38, 54);
            //    //DrawSegData(g, 0, 5, value);
            //    DrawSingleSegData(g, 0, 5, segment, value);
            //    g.Dispose();
            //}

            //var graphics = Graphics.FromHwnd(targetWnd);
            //graphics.DrawImage(buffer, 20 + position * 45, 5);
            //graphics.Dispose();
        }

        private void Write(int address, int data, Graphics graphics)
        {
            using (var buffer = new Bitmap(38, 54))
            {
                int position = 6 - ((address & 0xF0) >> 4);
                int segment = address & 0x7;

                using (var g = Graphics.FromImage(buffer))
                {
                    DrawSingleSegData(g, 0, 5, segment, data);
                }

                graphics.DrawImage(buffer, 20 + position * 45, 5);
            }
        }


        public void Repaint(Graphics graphics)
        {
            try
            {
                if (Monitor.TryEnter(bglock, 10))
                {
                    for (int address = 0xC16F; address >= 0xC110; address--)
                    {
                        if ((address & 0x08) != 0x08)
                        {
                            Write(address, _trainer.Memory[address], graphics);
                        }
                    }

                    using (var font = new Font("Arial", 8))
                    {
                        for (int position = 0; position < 6; position++)
                        {
                            graphics.DrawString(flags[position], font, Brushes.White, 42 + position * 45, 62);
                        }
                    }
                    Monitor.Exit(bglock);
                }
            }
            catch
            {
                // Swallow
            }

        }

        public void Redraw()
        {
            using (var graphics = Graphics.FromHwnd(targetWnd))
            {
                Repaint(graphics);
            }
        }

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

        public void Dispose()
        {
            _target.Paint -= TargetOnPaint;
            _updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _updateTimer?.Dispose();
        }
    }
}
