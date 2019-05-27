using Core6800;

namespace Sharp6800.Trainer
{
    public struct WatchEventArgs
    {
        public Cpu6800State State { get; set; }
        public int Address { get; set; }
        public int Value { get; set; }
    }
}