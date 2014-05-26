using System;
using System.Drawing;
using System.Windows.Forms;
using Core6800;

namespace Sharp6800
{
    public class DasmDisplay
    {
        private readonly IntPtr targetWnd;
        private readonly int width, height;

        public DasmDisplay(PictureBox target)
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


        public void Display(int[] memory, Cpu6800State state, int start, int lines)
        {
            var buffer = new Bitmap(width, height);
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.White);

            int j = 0;
            for (int i = start; j <= lines; )
            {
                i += DrawHex(g, 10, 20 * j, ref i, memory, state);
                j++;
            }

            g.Dispose();

            var p = Graphics.FromHwnd(targetWnd);
            p.DrawImage(buffer, 0, 0);
            p.Dispose();
        }

        private int DrawHex(Graphics g, int x, int y, ref int start, int[] memory, Cpu6800State state)
        {
            string buf = "";
            var ops = Disassembler.Disassemble(start, ref buf, new int[] { memory[start] }, new int[] { memory[start], memory[start + 1], memory[start + 2], memory[start + 3] });

            var s = string.Format("{0:X4} {1}", start, buf);
            Brush brush;
            FontStyle fs;
            if (start == state.PC)
            {
                brush = new SolidBrush(Color.DarkRed);
                fs = FontStyle.Bold;
            }
            else
            {
                brush = new SolidBrush(Color.Black);
                fs = FontStyle.Regular;
            }
            var font = new Font("Courier New", 12, fs);
            g.DrawString(s, font, brush, x, y);
            return ops & 0x3;
        }
    }
}