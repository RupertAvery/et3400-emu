using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Schema;
using Sharp6800.Debugger;

namespace Sharp6800.Trainer
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

    public class MemoryMapEventBus
    {
        private Dictionary<MapEventType, List<Action<IEnumerable<MemoryMap>>>> _subscriptions;

        public MemoryMapEventBus()
        {
            _subscriptions = new Dictionary<MapEventType, List<Action<IEnumerable<MemoryMap>>>>();
            _subscriptions.Add(MapEventType.Add, new List<Action<IEnumerable<MemoryMap>>>());
            _subscriptions.Add(MapEventType.Update, new List<Action<IEnumerable<MemoryMap>>>());
            _subscriptions.Add(MapEventType.Remove, new List<Action<IEnumerable<MemoryMap>>>());
            _subscriptions.Add(MapEventType.Clear, new List<Action<IEnumerable<MemoryMap>>>());
        }

        public void Unsubscribe(MapEventType eventType, Action<IEnumerable<MemoryMap>> mapEventAction)
        {
            _subscriptions[eventType].Remove(mapEventAction);
        }

        public void Subscribe(MapEventType eventType, Action<IEnumerable<MemoryMap>> mapEventAction)
        {
            _subscriptions[eventType].Add(mapEventAction);
        }

        public void Publish(MapEventType eventType, IEnumerable<MemoryMap> memoryMaps)
        {
            foreach (var action in _subscriptions[eventType])
            {
                action(memoryMaps);
            }
        }

    }

    public class MemoryMapCollection : IEnumerable<MemoryMap>
    {
        private readonly MemoryMapEventBus _memoryMapEventBus;
        private List<MemoryMap> _memoryMaps;
        private object lockObject = new object();

        public bool RequestLock(int timeOut = 100)
        {
            return Monitor.TryEnter(lockObject, timeOut);
        }

        public void ReleaseLock()
        {
            Monitor.Exit(lockObject);
        }

        public MemoryMapCollection(MemoryMapEventBus memoryMapEventBus)
        {
            _memoryMapEventBus = memoryMapEventBus;
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
            lock (lockObject)
            {
                _memoryMaps.Add(memoryMap);
            }
            _memoryMapEventBus.Publish(MapEventType.Add, new[] { memoryMap });
        }

        public void Add(int startAddress, int endAddress, RangeType rangeType, string description)
        {
            var memoryMap = new MemoryMap()
            {
                Start = startAddress,
                End = endAddress,
                Type = rangeType,
                Description = description
            };
            lock (lockObject)
            {
                _memoryMaps.Add(memoryMap);
            }
            _memoryMapEventBus.Publish(MapEventType.Add, new[] { memoryMap });
        }

        public MemoryMap this[int startAddress]
        {
            get { return _memoryMaps.FirstOrDefault(map => map.Start <= startAddress && map.End >= startAddress); }
        }

        public void Remove(MemoryMap memoryMap)
        {
            lock (lockObject)
            {
                _memoryMaps.Remove(memoryMap);
            }

            _memoryMapEventBus.Publish(MapEventType.Remove, new[] { memoryMap });
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
            lock (lockObject)
            {
                _memoryMaps.Clear();
            }
            _memoryMapEventBus.Publish(MapEventType.Clear, null);
        }

        public void AddRange(IEnumerable<MemoryMap> memoryMaps)
        {
            lock (lockObject)
            {
                _memoryMaps.AddRange(memoryMaps);
            }

            _memoryMapEventBus.Publish(MapEventType.Add, memoryMaps);
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