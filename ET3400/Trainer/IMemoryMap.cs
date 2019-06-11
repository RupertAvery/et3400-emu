using System.Collections.Generic;
using ET3400.Debugger;

namespace ET3400.Trainer
{
    public interface IMemoryMap
    {
        List<MemoryMap> MemoryMaps { get; }
        void AddMemoryMap(int startAddress, int endAddress, RangeType rangeType, string description);
        MemoryMap GetMemoryMap(int startAddress);
        void RemoveMemoryMap(MemoryMap memoryMap);
    }
}