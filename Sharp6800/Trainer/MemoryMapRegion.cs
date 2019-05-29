namespace Sharp6800.Trainer
{
    public class MemoryMapRegion
    {
        public MemoryMapCollection MemoryMapCollection { get; set; }
        public string Name { get; set; }
        public MemoryMapRegionType RegionType { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public MemoryMapRegion()
        {
            MemoryMapCollection = new MemoryMapCollection();
        }
    }
}