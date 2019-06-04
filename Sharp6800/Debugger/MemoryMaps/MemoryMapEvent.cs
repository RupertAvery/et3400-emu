using System.Collections.Generic;

namespace Sharp6800.Debugger.MemoryMaps
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