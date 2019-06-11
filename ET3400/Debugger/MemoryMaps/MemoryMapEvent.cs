using System.Collections.Generic;

namespace ET3400.Debugger.MemoryMaps
{
    public class MemoryMapEvent
    {
        public MapEventType Type { get; }
        public IEnumerable<MemoryMap> MemoryMaps { get; }

        public MemoryMapEvent(MapEventType type, IEnumerable<MemoryMap> memoryMaps)
        {
            Type = type;
            MemoryMaps = memoryMaps;
        }

    }
}