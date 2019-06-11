using System;
using System.IO;

namespace ET3400.IO
{
    public class RomReader : IDisposable
    {
        private readonly Stream stream;
        private bool isInternalStream;
        public RomReader(Stream stream)
        {
            this.stream = stream;
        } 

        public RomReader(string file)
        {
            isInternalStream = true;
            this.stream = new FileStream(file,FileMode.Open, FileAccess.Read);
        } 

        public int[] Read()
        {
            var sr = new StreamReader(stream);
            var data = sr.ReadToEnd();
            var lines = data.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            var rom = new int[lines.Length * 32];

            var j = 0;
            foreach (var line in lines)
            {
                for (var i = 0; i < 32; i++)
                {
                    rom[j * 32 + i] = Convert.ToInt32(line.Substring(i * 2, 2), 16);
                }
                j++;
            }

            return rom;
        }

        public void Dispose()
        {
            if (isInternalStream)
            {
                stream?.Dispose();
            }
        }
    }
}