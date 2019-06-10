using System;
using System.Collections.Generic;
using Core6800;
using Sharp6800.Debugger.Breakpoints;
using Sharp6800.Debugger.MemoryMaps;

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
        BreakpointCollection Breakpoints { get; }
        MemoryMapManager MemoryMapManager { get; }
        void WriteMemory(int address, int[] data, int length);
        int[] ReadMemory(int address, int length);
        Cpu6800State State { get; }
        bool IsRunning { get; }
        void AddWatch(Watch watch);
        void ToggleBreakPoint(int valueAddress);
    }
}