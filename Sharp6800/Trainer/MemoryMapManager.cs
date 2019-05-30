using System;
using System.Collections.Generic;
using System.Linq;
using Sharp6800.Debugger;

namespace Sharp6800.Trainer
{
    public enum MapEventType
    {
        Add,
        Remove,
        Update,
        Clear
    }

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

    public class MemoryMapManager
    {
        private List<MemoryMapRegion> _memoryMapsRegions;

        public MemoryMapManager()
        {
            _memoryMapsRegions = new List<MemoryMapRegion>();
        }

        public void AddRegion(MemoryMapRegion memoryMapRegion)
        {
            _memoryMapsRegions.Add(memoryMapRegion);
        }

        public void RemoveRegion(MemoryMapRegion memoryMapRegion)
        {
            _memoryMapsRegions.Remove(memoryMapRegion);
        }

        public void RemoveRegionByName(string name)
        {
            var rgn = _memoryMapsRegions.FirstOrDefault(region => region.Name == name);
            if (rgn != null)
            {
                _memoryMapsRegions.Remove(rgn);
            }
        }

        public MemoryMapRegion GetRegion(string name)
        {
            return _memoryMapsRegions.FirstOrDefault(region => region.Name == name);
        }

        public MemoryMap GetMemoryMap(int address)
        {
            var region = _memoryMapsRegions.FirstOrDefault(x => x.Start <= address && x.End >= address);
            if (region != null)
            {
                if (region.MemoryMapCollection.RequestLock())
                {
                    var memoryMap = region.MemoryMapCollection.FirstOrDefault(x => x.Start <= address && x.End >= address);
                    region.MemoryMapCollection.ReleaseLock();
                    return memoryMap;
                }
            }

            return null;
        }

        public void ClearRegions()
        {
            //foreach (var region in _memoryMapsRegions)
            //{
            //    region.MemoryMapCollection.OnChanged(this, );
            //}
            _memoryMapsRegions.Clear();
        }

        public void AddMemoryMap(MemoryMap memoryMap)
        {
            var region = _memoryMapsRegions.FirstOrDefault(x => x.Start <= memoryMap.Start && x.End >= memoryMap.End);
            if (region != null)
            {
                region.MemoryMapCollection.Add(memoryMap);
            }
        }

        public void RemoveMemoryMap(MemoryMap memoryMap)
        {
            var region = _memoryMapsRegions.FirstOrDefault(x => x.Start <= memoryMap.Start && x.End >= memoryMap.End);
            if (region != null)
            {
                region.MemoryMapCollection.Remove(memoryMap);
            }
        }
    }
}