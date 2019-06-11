using Core6800;

namespace ET3400.Trainer
{
    public struct WatchEventArgs
    {
        public Cpu6800State State { get; set; }
        public int Address { get; set; }
        public int Value { get; set; }
    }
}