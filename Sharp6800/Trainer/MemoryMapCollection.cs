using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;
using Sharp6800.Debugger;

namespace Sharp6800.Trainer
{
    public class MemoryMapCollection : IEnumerable<MemoryMap>
    {
        private List<MemoryMap> _memoryMaps;

        public MemoryMapCollection()
        {
            _memoryMaps = new List<MemoryMap>();
        }

        public IEnumerator<MemoryMap> GetEnumerator()
        {
            return _memoryMaps.GetEnumerator();
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
            get { return _memoryMaps.FirstOrDefault(map => map.Start <= startAddress && map.End >= startAddress); }
        }

        public void Remove(MemoryMap memoryMap)
        {
            _memoryMaps.Remove(memoryMap);
        }

        public static IEnumerable<MemoryMap> Load(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return Load(stream);
            }
        }

        private static RangeType ParseRangeType(string type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "data":
                    return RangeType.Data;
                case "code":
                    return RangeType.Code;
                case "comment":
                    return RangeType.Comment;
                default:
                    throw new Exception($"Invalid range type: {type}");
            }
        }

        public static IEnumerable<MemoryMap> Load(Stream stream)
        {
            var maps = new List<MemoryMap>();

            using (var csvReader = new CsvReader(stream))
            {
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
                            Type = ParseRangeType(parts[2]),
                            Description = parts[3],
                        });
                    }
                    else
                    {
                        throw new Exception("Error reading map file");
                    }
                }
            }

            return maps;
        }

        public void Clear()
        {
            _memoryMaps.Clear();
        }

        public void AddRange(IEnumerable<MemoryMap> memoryMaps)
        {
            _memoryMaps.AddRange(memoryMaps);
        }

        public static void Save(Stream stream, IEnumerable<MemoryMap> memoryMaps)
        {
            var maps = new List<MemoryMap>();
            using (var csvWriter = new CsvWriter(stream))
            {
                csvWriter.WriteAll(memoryMaps.OrderBy(memoryMap => memoryMap.Start).Select(memoryMap => new string[]
                  {
                    $"{memoryMap.Start:X4}",
                    $"{memoryMap.End:X4}",
                    $"{Enum.GetName(typeof(RangeType), memoryMap.Type)}",
                    $"{memoryMap.Description}"
                  }));
            }
        }
    }
}