namespace ET3400.Debugger
{
    public class MemoryMap
    {
        public int Start { get; set; }
        public int End { get; set; }
        public RangeType Type { get; set; }
        public string Description { get; set; }
    }
}