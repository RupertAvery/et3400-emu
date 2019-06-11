using System;
using System.Collections.Generic;

namespace ET3400.Debugger.MemoryMaps
{
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
}