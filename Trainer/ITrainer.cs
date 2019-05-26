using System;
using System.Collections.Generic;
using Core6800;

namespace Sharp6800.Trainer
{

    public enum EventType {
        Read,
        Write,
        PCChange,
        AccARead,
        AccAWrite,
        AccBRead,
        AccBWrite,
        SpRead,
        SpWrite,
        IxRead,
        IxWrite,
    }

    public interface ITrainer : IMemory, IMemoryMap
    {
        List<int> Breakpoints { get; }
        Cpu6800State State { get; }
        bool Running { get; }
        void AddWatch(Watch watch);
    }
}