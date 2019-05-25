using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sharp6800.Debugger
{
    public class MemoryMap
    {
        public int Start { get; set; }
        public int End { get; set; }
        public RangeType Type { get; set; }
        public string Description { get; set; }
    }
}