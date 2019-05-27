using System.Collections.Generic;
using System.Linq;
using Sharp6800.Trainer;

namespace Sharp6800.Debugger
{
    public class DisassemblyView
    {
        private ITrainer _trainer;
        private object lockObject = new object();

        public bool IsDirty { get; private set; }
        public string Description { get; }
        public int Start { get; }
        public int End { get; }

        public int LineCount
        {
            get { return Lines.Count; }
        }

        public DisassemblyView(ITrainer trainer, string description, int start, int end)
        {
            _trainer = trainer;
            Description = description;
            Start = start;
            End = end;
            trainer.AddWatch(new Watch()
            {
                EventType = EventType.Write,
                Action = args =>
                {
                    if (args.Address >= start && args.Address <= end)
                    {
                        IsDirty = true;
                    }
                }
            });
            DisassembleRange();
        }

        public List<DisassemblyLine> Lines { get; private set; }

        private string BytesToString(int start, int length)
        {
            var code = "";
            for (var k = 0; k < length; k++)
            {
                code = code + string.Format("{0:X2}", _trainer.Memory[start + k]) + " ";
            }
            return code;
        }

        public void Refresh()
        {
            DisassembleRange();
        }

        private void DisassembleRange()
        {
            lock (lockObject)
            {
                var currentAddress = Start;
                Lines = new List<DisassemblyLine>();

                DisassemblyLine disassemblyLine;
                var lineNumber = 0;
                while (currentAddress < End)
                {
                    var memoryMap = _trainer.MemoryMaps[currentAddress];
                    if (memoryMap != null)
                    {
                        disassemblyLine = new DisassemblyLine()
                        {
                            Text = memoryMap.Description,
                            LineType = LineType.Comment,
                            Address = currentAddress,
                            LineNumber = lineNumber
                        };

                        Lines.Add(disassemblyLine);
                        lineNumber++;

                        var totalLength = memoryMap.End - memoryMap.Start + 1;

                        while (currentAddress < memoryMap.End)
                        {
                            var byteLength = totalLength - (currentAddress - memoryMap.Start);

                            if (byteLength > 8)
                            {
                                byteLength = 8;
                            }

                            disassemblyLine = new DisassemblyLine()
                            {
                                Text = BytesToString(currentAddress, byteLength),
                                LineType = LineType.Data,
                                Address = currentAddress,
                                LineNumber = lineNumber
                            };

                            currentAddress += byteLength;

                            Lines.Add(disassemblyLine);
                            lineNumber++;
                        }
                    }
                    else
                    {
                        var result = Disassembler.Disassemble(_trainer.Memory, currentAddress);

                        var opCodes = BytesToString(currentAddress, result.ByteLength);

                        disassemblyLine = new DisassemblyLine()
                        {
                            Text = $"{opCodes,-9} {result.Operand:2}",
                            LineType = LineType.Assembly,
                            ByteLength = result.ByteLength,
                            Address = currentAddress,
                            LineNumber = lineNumber
                        };

                        Lines.Add(disassemblyLine);
                        lineNumber++;

                        currentAddress += result.ByteLength;
                    }
                }
            }
        }

        public DisassemblyLine? GetLineFromAddress(int address)
        {
            lock (lockObject)
            {
                return Lines.Where(x => x.Address == address).Cast<DisassemblyLine?>().FirstOrDefault();
            }
        }
    }
}