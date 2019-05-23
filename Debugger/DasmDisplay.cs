using Core6800;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sharp6800.Debugger
{
    public class DasmDisplay
    {
        private readonly IntPtr targetWnd;
        private readonly int width, height;
        private int _selectedOffset;

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

        public int LastAddress { get; private set; }
        public int FirstAddress { get; private set; }


        public int GetOffsetFromAddress(int[] memory, int address)
        {
            for (int i = Start, offset = 0; i < memory.Length; offset++)
            {
                string buf = "";
                var ops = Disassembler.Disassemble(memory, i, ref buf) & 0x3;
                if (address == i)
                {
                    return offset;
                }
                i += ops;
            }

            return 0;
        }

        public void Display(int[] memory, Cpu6800State state, List<int> breakpoints)
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

                    if (MemoryMaps != null)
                    {
                        foreach (var memoryMap in MemoryMaps)
                        {
                            var inDataRange = memoryMap.Ranges.FirstOrDefault(range => i >= range.Start && i <= range.End);
                            if (inDataRange != null)
                            {
                                i = inDataRange.End + 1;
                                break;
                            }
                        }

                    }

                    var ops = Disassembler.Disassemble(memory, i, ref buf) & 0x3;
                    int k;

                    for (k = 0; k < ops; k++)
                    {
                        code = code + string.Format("{0:X2}", memory[i + k]) + " ";
                    }


                    if (offset >= Offset)
                    {
                        if (offset == _selectedOffset)
                        {
                            SelectedAddress = i;
                            using (var brush = new SolidBrush(Color.Blue))
                            {
                                g.FillRectangle(brush, 2, 20 * j, width - 2, 20);
                            }
                        }

                        if (breakpoints.Contains(i))
                        {
                            using (var brush = new SolidBrush(Color.DarkRed))
                            {
                                g.FillRectangle(brush, 2, 20 * j, width - 2, 20);
                            }
                        }

                        if (i == state.PC)
                        {
                            using (var brush = new SolidBrush(Color.Yellow))
                            {
                                g.FillRectangle(brush, 2, 20 * j, width - 2, 20);
                            }
                        }

                        DrawHex(g, 10, 20 * j, string.Format("{0:X4} {1,-9} {2}", i, code, buf.ToUpper()));
                        j++;
                    }

                    LastAddress = i;

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
            using (var brush = new SolidBrush(Color.Black))
            {
                using (var font = new Font("Courier New", 12, FontStyle.Regular))
                {
                    g.DrawString(asm, font, brush, x, y);

                }
            }
        }

        public int SelectedAddress { get; private set; }


        public int Start { get; set; }

        public int Offset { get; set; }

        public IEnumerable<MemoryMap> MemoryMaps { get; set; }

        public void SelectOffset(int selectedOffset)
        {
            _selectedOffset = selectedOffset;
        }
    }
}