namespace Sharp6800.Debugger
{
    public struct DisassemblyLine
    {
        public int LineNumber { get; set; }
        public LineType LineType { get; set; }
        public string Text { get; set; }
        public int Address { get; set; }
        public int ByteLength { get; set; }
        public string Opcodes { get; set; }
        public string Instruction { get; set; }
        public string Operands { get; set; }
    }
}