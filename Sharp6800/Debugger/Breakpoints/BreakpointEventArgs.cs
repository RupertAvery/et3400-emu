namespace Sharp6800.Debugger.Breakpoints
{
    public class BreakpointEventArgs
    {
        public int Address { get; private set; }
        public BreakpointEventType EventType { get; private set; }

        public BreakpointEventArgs(BreakpointEventType eventType, int address)
        {
            Address = address;
            EventType = eventType;
        }
    }
}