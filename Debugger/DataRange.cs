using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sharp6800.Debugger
{
    public enum RangeType
    {
        Code,
        Data
    }

    public class MemoryMap
    {
        [XmlElement("start")]
        public string Start { get; set; }
        [XmlArray("ranges")]
        [XmlArrayItem("range")]
        public List<DataRange> Ranges { get; set; }
    }

    public class DataRange : IXmlSerializable 
    {
        [XmlAttribute("start")]
        public int Start { get; set; }
        [XmlAttribute("end")]
        public int End { get; set; }
        [XmlAttribute("comment")]
        public string Comment { get; set; }
        [XmlAttribute("type")]
        public RangeType Type { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        private static int GetAddress(string address)
        {
            address = address.Trim();
            if (address.StartsWith("0x"))
            {
                return Convert.ToInt32(address.Substring(2), 16);
            }
            if (address.StartsWith("$"))
            {
                return Convert.ToInt32(address.Substring(1), 16);
            }
            return Convert.ToInt32(address, 16);
        }


        private static string SetAddress(int address)
        {
            return string.Format("${0:X4}", address);
        }

        public void ReadXml(XmlReader reader)
        {
            var x = reader.Name;
            Start = GetAddress(reader.GetAttribute("start"));
            End = GetAddress(reader.GetAttribute("end"));
            Comment = reader.GetAttribute("comment");
            RangeType rangeType;
            Enum.TryParse(reader.GetAttribute("type"), true, out rangeType);
            Type = rangeType;
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("start",SetAddress(Start));
            writer.WriteAttributeString("end", SetAddress(End));
            writer.WriteAttributeString("type", Enum.GetName(typeof(RangeType), Type).ToLowerInvariant());
            if (Comment != null)
            {
                writer.WriteAttributeString("comment", Comment);
            }
        }
    }
}