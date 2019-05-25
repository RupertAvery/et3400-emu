using System.Collections.Generic;
using Sharp6800.Debugger;

namespace Sharp6800.Trainer
{
    public interface IMemoryMap
    {
        List<MemoryMap> MemoryMaps { get; }
        void AddMemoryMap(int startAddress, int endAddress, RangeType rangeType, string description);
        MemoryMap GetMemoryMap(int startAddress);
        void RemoveMemoryMap(MemoryMap memoryMap);
    }
}