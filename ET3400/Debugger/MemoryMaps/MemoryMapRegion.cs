namespace ET3400.Debugger.MemoryMaps
{
    public class MemoryMapRegion
    {
        public MemoryMapCollection MemoryMapCollection { get; set; }
        public string Name { get; set; }
        public MemoryMapRegionType RegionType { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        //public bool IsDirty { get; set; }

        public MemoryMapRegion(MemoryMapEventBus memoryMapEventBus)
        {
            MemoryMapCollection = new MemoryMapCollection(memoryMapEventBus);
        }
    }
}