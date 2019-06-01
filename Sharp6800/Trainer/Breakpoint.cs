namespace Sharp6800.Trainer
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
    }
}