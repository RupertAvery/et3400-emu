using System.Collections.Generic;

namespace Sharp6800.Debugger.MemoryMaps
{
    public class MapEventArgs
    {
        public MapEventType Type { get; }
        public IEnumerable<MemoryMap> MemoryMaps { get; }

        public MapEventArgs(MapEventType type, IEnumerable<MemoryMap> memoryMaps)
        {
            Type = type;
            MemoryMaps = memoryMaps;
        }
    }
}