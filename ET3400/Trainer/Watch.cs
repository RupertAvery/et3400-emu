using System;

namespace ET3400.Trainer
{
    public struct Watch
    {
        public EventType EventType { get; set; }
        public int Address { get; set; }
        public int Value { get; set; }
        public Action<WatchEventArgs> Action { get; set; }
    }
}