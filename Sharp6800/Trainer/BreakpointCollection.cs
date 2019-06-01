using System;
using System.Collections;
using System.Collections.Generic;

namespace Sharp6800.Trainer
{
    public enum BreakpointEventType
    {
        Add,
        Remove,
        Clear,
        Enable,
        Disable
    }

    public class BreakpointEventArgs
    {
        public int Address { get; private set; }
        public BreakpointEventType EventType { get; private set; }

        public BreakpointEventArgs(BreakpointEventType eventType, int address)
        {
            Address = address;
            EventType = eventType;
        }
    }

    public class BreakpointCollection : IEnumerable<Breakpoint>
    {
        private Dictionary<int, Breakpoint> _breakpointLookup;

        public EventHandler<BreakpointEventArgs> OnChange { get; set; }

        public BreakpointCollection()
        {
            _breakpointLookup = new Dictionary<int, Breakpoint>();
        }

        public Breakpoint this[int address]
        {
            get
            {
                if (_breakpointLookup.TryGetValue(address, out Breakpoint value))
                {
                    return value;
                }
                return null;
            }
        }

        public void Add(int address)
        {
            var breakpoint = new Breakpoint(address);
            _breakpointLookup.Add(address, breakpoint);
            OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Add, address));
        }

        public void Clear()
        {
            _breakpointLookup.Clear();
            OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Clear, 0));
        }

        public void Remove(int address)
        {
            if (_breakpointLookup.ContainsKey(address))
            {
                _breakpointLookup.Remove(address);
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Remove, address));
            }
        }

        public void Toggle(int address)
        {
            if (_breakpointLookup.TryGetValue(address, out Breakpoint value))
            {
                value.IsEnabled = !value.IsEnabled;
                if (value.IsEnabled)
                {
                    OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Enable, address));
                }
                else
                {
                    OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Disable, address));
                }
            }
        }

        public void Enable(int address)
        {
            if (_breakpointLookup.TryGetValue(address, out Breakpoint value))
            {
                value.IsEnabled = true;
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Enable, address));
            }
        }


        public void Disable(int address)
        {
            if (_breakpointLookup.TryGetValue(address, out Breakpoint value))
            {
                value.IsEnabled = false;
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Disable, address));
            }
        }

        public IEnumerator<Breakpoint> GetEnumerator()
        {
            return _breakpointLookup.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}