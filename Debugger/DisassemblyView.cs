using System.Collections.Generic;
using Sharp6800.Trainer;

namespace Sharp6800.Debugger
{
    public class DisassemblyView
    {
        private int _startAddress;
        private int _endAddress;
        private ITrainer _trainer;
        private string _description;

        public string Description
        {
            get => _description;
        }

        public int Start
        {
            get => _startAddress;
        }

        public int End
        {
            get => _endAddress;
        }

        public int LineCount
        {
            get { return Lines.Count; }
        }

        public DisassemblyView(ITrainer trainer, string description, int start, int end)
        {
            _trainer = trainer;
            _description = description;
            _startAddress = start;
            _endAddress = end;
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

        private void DisassembleRange()
        {
            var currentAddress = _startAddress;
            Lines = new List<DisassemblyLine>();

            DisassemblyLine disassemblyLine;
            var lineNumber = 0;
            while (currentAddress < _endAddress)
            {
                var memoryMap = _trainer.GetMemoryMap(currentAddress);
                if (memoryMap != null)
                {
                    disassemblyLine = new DisassemblyLine()
                    {
                        Text = memoryMap.Description,
                        LineType = LineType.Comment,
                        Address = currentAddress
                    };

                    Lines.Add(disassemblyLine);

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
}