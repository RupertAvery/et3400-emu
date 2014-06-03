using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core6800;

namespace Sharp6800.Debugger
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
                DataRanges = new List<DataRange>();

            }
            catch (Exception)
            {
                throw new Exception(
                    "Error initializing the display. one or more segment image files may be missing.");
            }
        }

        public Cpu6800State State { get; set; }

        public IEnumerable<DataRange> DataRanges { get; set; }

        public void Display(int[] memory)
        {
            var buffer = new Bitmap(width, height);
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.White);

            int j = 0;

            for (int i = Start, offset = 0; j <= 16; offset++)
            {
                string buf = "";
                string code = "";
                if (i < memory.Length)
                {
                    var inDataRange = DataRanges.FirstOrDefault(range => i >= range.Start && i <= range.End);
                    if (inDataRange != null)
                    {
                        i = inDataRange.End + 1;
                    }
                    var ops = Disassembler.Disassemble(memory, i, ref buf) & 0x3;
                    int k;
                    for (k = 0; k < ops; k++)
                    {
                        code = code + string.Format("{0:X2}", memory[i + k]) + " ";
                    }
                    if (offset >= Offset)
                    {
                        DrawHex(g, 10, 20 * j, string.Format("{0:X4} {1,-9} {2}", i, code, buf.ToUpper()));
                        j++;
                    }
                    i += ops;
                    
                }
                else
                {
                    break;
                }
            }

            g.Dispose();

            var p = Graphics.FromHwnd(targetWnd);
            p.DrawImage(buffer, 0, 0);
            p.Dispose();
        }

        private void DrawHex(Graphics g, int x, int y, string asm)
        {
            Brush brush;
            FontStyle fs;
            brush = new SolidBrush(Color.Black);
            fs = FontStyle.Regular;
            var font = new Font("Courier New", 12, fs);
            g.DrawString(asm, font, brush, x, y);
        }

        public int Start { get; set; }

        public int Offset { get; set; }
    }
}