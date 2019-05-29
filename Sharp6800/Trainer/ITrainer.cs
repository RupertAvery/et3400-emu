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

    public interface ITrainer : IMemory
    {
        MemoryMapManager MemoryMapManager { get; }
        void WriteMemory(int address, int[] data, int length);
        int[] ReadMemory(int address, int length);
        List<int> Breakpoints { get; }
        Cpu6800State State { get; }
        bool IsRunning { get; }
        void AddWatch(Watch watch);
    }
}