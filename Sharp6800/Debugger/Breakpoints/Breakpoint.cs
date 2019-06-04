namespace Sharp6800.Debugger.Breakpoints
{
    public class Breakpoint
    {
        public int Address { get; private set; }
        public bool IsEnabled { get; set; }

        public Breakpoint(int address)
        {
            Address = address;
            IsEnabled = true;
        }

        public Breakpoint(int address, bool isEnabled)
        {
            Address = address;
            IsEnabled = isEnabled;
        }

    }
}