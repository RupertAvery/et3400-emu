using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sharp6800.Debugger;

namespace Sharp6800
{

    //class HexFile
    //{
    //    public static void Write(string file, int[] data)
    //    {
    //        using (var fs = new StreamWriter(file))
    //        {
    //            int j = 0;
    //            foreach (var b in data)
    //            {
    //                fs.Write(string.Format("{0:X2}", b));
    //                j++;
    //                if (j != 32) continue;
    //                fs.Write("\r\n");
    //                j = 0;
    //            }
    //        }
    //    }
    //}

    class DataBlock
    {
        public DataBlock(string address, string data)
        {
            Address = Convert.ToInt32(address, 16);
            if (data.Length % 2 == 1) throw new Exception("Invalid data block");
            Data = new int[data.Length / 2];
            for (var p = 0; p < data.Length; p += 2)
            {
                Data[p / 2] = Convert.ToInt32(data.Substring(p, 2), 16);
            }
        }

        public int Address { get; private set; }
        public int[] Data { get; private set; }
    }

    class SrecFile
    {
        public static void Write(string file, List<DataBlock> dataBlocks)
        {

        }

        public static IEnumerable<DataBlock> Read(string file)
        {
            var dataBlocks = new List<DataBlock>();
            var content = File.ReadAllText(file);
            var lines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    var bytecount = Convert.ToInt32(line.Substring(2, 2), 16);
                    string addr;
                    string data;
                    string checksum;
                    switch (line.Substring(0, 2))
                    {
                        case "S1":
                            addr = line.Substring(4, 4);
                            data = line.Substring(8, bytecount * 2 - 6);
                            dataBlocks.Add(new DataBlock(addr, data));
                            checksum = line.Substring(bytecount * 2 + 2, 2);
                            break;
                        case "S2":
                            break;
                        case "S3":
                            break;
                        case "S7":
                            break;
                        case "S8":
                            break;
                        case "S9":
                            break;
                    }
                }


            }
            return dataBlocks;
        }

    }
}
