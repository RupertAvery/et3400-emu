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
        private readonly Trainer.Trainer _trainer;
        private readonly IntPtr targetWnd;
        private readonly int width, height;

        public DasmDisplay(PictureBox target, Trainer.Trainer trainer)
        {
            _trainer = trainer;
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


        public int GetOffsetFromAddress(int address)
        {
            for (int i = Start, offset = 0; i < _trainer.Memory.Length; offset++)
            {
                string buf = "";
                var ops = Disassembler.Disassemble(_trainer.Memory, i, ref buf) & 0x3;
                if (address == i)
                {
                    return offset;
                }
                i += ops;
            }

            return 0;
        }

        public void Display()
        {
            var buffer = new Bitmap(width, height);
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.White);

            int j = 0;

            for (int i = Start, offset = 0; j <= 16; offset++)
            {
                string buf = "";
                string code = "";
                var ops = 0;

                if (i < _trainer.Memory.Length)
                {
                    var inDataRange = false;

                    if (_trainer.MemoryMaps != null)
                    {
                        foreach (var memoryMap in _trainer.MemoryMaps)
                        {
                            inDataRange = i >= memoryMap.Start && i <= memoryMap.End;
                            if (inDataRange)
                            {
                                ops = memoryMap.End - memoryMap.Start + 1;

                                if (ops > 8)
                                {
                                    ops = 8;
                                }

                                break;
                            }
                        }
                    }

                    if (!inDataRange)
                    {
                        ops = Disassembler.Disassemble(_trainer.Memory, i, ref buf) & 0x3;
                    }

                    for (var k = 0; k < ops; k++)
                    {
                        if (i + k < _trainer.Memory.Length)
                        {
                            code = code + string.Format("{0:X2}", _trainer.Memory[i + k]) + " ";
                        }
                    }

                    if (offset >= Offset)
                    {
                        var isAtBreakPoint = _trainer.Breakpoints.Contains(i);
                        var isSelected = false;
                        var isCurrentPC = (!_trainer.Running && i == _trainer.State.PC);

                        if (offset == SelectedOffset)
                        {
                            SelectedAddress = i;
                            isSelected = true;
                        }


                        if (isAtBreakPoint || isSelected || isCurrentPC)
                        {
                            Color highlightColor = Color.White;

                            if (isAtBreakPoint && isSelected && isCurrentPC)
                            {
                                highlightColor = Color.DodgerBlue;
                            }
                            else if (isSelected && isCurrentPC)
                            {
                                highlightColor = Color.DodgerBlue;
                            }
                            else if (isAtBreakPoint && isSelected)
                            {
                                highlightColor = Color.DodgerBlue;
                            }
                            else if (isAtBreakPoint && isCurrentPC)
                            {
                                highlightColor = Color.Orange;
                            }
                            else if (isAtBreakPoint)
                            {
                                highlightColor = Color.DarkRed;
                            }
                            else if (isSelected)
                            {
                                highlightColor = Color.Blue;
                            }
                            else if (isCurrentPC)
                            {
                                highlightColor = Color.Yellow;
                            }

                            using (var brush = new SolidBrush(highlightColor))
                            {
                                g.FillRectangle(brush, 2, 20 * j, width - 2, 20);
                            }
                        }

                        DrawHex(g, 10, 20 * j, string.Format("{0:X4} {1,-9} {2}", i, code, buf.ToUpper()), isAtBreakPoint);
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

        private void DrawHex(Graphics g, int x, int y, string asm, bool isAtBreakPoint)
        {
            using (var brush = new SolidBrush(isAtBreakPoint ? Color.White : Color.Black))
            {
                using (var font = new Font("Courier New", 12, FontStyle.Regular))
                {
                    g.DrawString(asm, font, brush, x, y);
                }
            }
        }

        public int SelectedAddress { get; private set; }
        public int SelectedOffset { get; set; }


        public int Start { get; set; }
        public int Offset { get; set; }

    }
}