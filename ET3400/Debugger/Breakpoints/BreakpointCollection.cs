using System;
using System.Collections;
using System.Collections.Generic;

namespace ET3400.Debugger.Breakpoints
{
    public class BreakpointCollection : IEnumerable<Breakpoint>
    {
        private Dictionary<int, Breakpoint> _breakpointLookup;

        public EventHandler<BreakpointEventArgs> OnChange { get; set; }

        public bool IsDirty { get; set; }

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
            if (!_breakpointLookup.ContainsKey(address))
            {
                _breakpointLookup.Add(address, breakpoint);
                IsDirty = true;
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Add, address));
            }
        }

        public void Add(int address, bool enabled)
        {
            if (!_breakpointLookup.ContainsKey(address))
            {
                var breakpoint = new Breakpoint(address, enabled);
                _breakpointLookup.Add(address, breakpoint);
                IsDirty = true;
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Add, address));
            }
        }

        public void Clear()
        {
            _breakpointLookup.Clear();
            IsDirty = true;
            OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Clear, 0));
        }

        public void Remove(int address)
        {
            if (_breakpointLookup.ContainsKey(address))
            {
                _breakpointLookup.Remove(address);
                IsDirty = true;
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Remove, address));
            }
        }

        public void Toggle(int address)
        {
            if (_breakpointLookup.TryGetValue(address, out Breakpoint value))
            {
                value.IsEnabled = !value.IsEnabled;
                IsDirty = true;
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
                IsDirty = true;
                OnChange?.Invoke(this, new BreakpointEventArgs(BreakpointEventType.Enable, address));
            }
        }


        public void Disable(int address)
        {
            if (_breakpointLookup.TryGetValue(address, out Breakpoint value))
            {
                value.IsEnabled = false;
                IsDirty = true;
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