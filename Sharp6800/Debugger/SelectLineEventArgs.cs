namespace Sharp6800.Debugger
{
    public class SelectLineEventArgs
    {
        public SelectLineEventArgs(DisassemblyLine? currentLine, MemoryMap memoryMap)
        {
            CurrentLine = currentLine;
            MemoryMap = memoryMap;
        }

        public DisassemblyLine? CurrentLine { get; }
        public MemoryMap MemoryMap { get; }
    }
}