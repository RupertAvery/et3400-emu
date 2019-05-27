using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sharp6800.Debugger;

namespace Sharp6800.Trainer
{
    public class MemoryMapCollection : IEnumerable<MemoryMap>
    {
        private List<MemoryMap> _memoryMaps;

        public MemoryMapCollection(IEnumerable<MemoryMap> memoryMaps)
        {
            _memoryMaps = memoryMaps.ToList();
        }

        public MemoryMapCollection()
        {
            _memoryMaps = new List<MemoryMap>();
        }

        public IEnumerator<MemoryMap> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(MemoryMap memoryMap)
        {
            _memoryMaps.Add(memoryMap);
        }

        public void Add(int startAddress, int endAddress, RangeType rangeType, string description)
        {
            _memoryMaps.Add(new MemoryMap()
            {
                Start = startAddress,
                End = endAddress,
                Type = rangeType,
                Description = description
            });
        }

        public MemoryMap this[int startAddress]
        {
            get
            {
                return _memoryMaps.FirstOrDefault(map => map.Start <= startAddress && map.End >= startAddress);
            }
        }

        public void Remove(MemoryMap memoryMap)
        {
            _memoryMaps.Remove(memoryMap);
        }

        public static MemoryMapCollection Load(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return Load(stream);
            }
        }

        public static MemoryMapCollection Load(Stream stream)
        {
            var maps = new List<MemoryMap>();

            var csvReader = new CsvReader(stream);

            var lines = csvReader.ReadAll();

            foreach (var line in lines)
            {
                var parts = line;
                if (parts.Length == 0) continue;
                if (parts.Length == 4)
                {
                    maps.Add(new MemoryMap()
                    {
                        Start = Convert.ToInt32(parts[0], 16),
                        End = Convert.ToInt32(parts[1], 16),
                        Type = parts[2] == "CODE" ? RangeType.Code : RangeType.Data,
                        Description = parts[3],
                    });
                }
                else
                {
                    throw new Exception("Error reading map file");
                }
            }

            return new MemoryMapCollection(maps);
        }

    }
}