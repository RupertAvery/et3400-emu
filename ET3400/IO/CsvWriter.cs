using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ET3400.IO
{
    public class CsvWriter : IDisposable
    {
        private StreamWriter sw;

        public CsvWriter(Stream stream)
        {
            sw = new StreamWriter(stream);
        }

        public void WriteLine(string[] parts)
        {
            var line = string.Join(",", parts.Select(part => part.Contains(",") ? $"\"{part}\"" : part));
            sw.WriteLine(line);
        }

        public void WriteAll(IEnumerable<string[]> data)
        {
            foreach (var parts in data)
            {
                var line = string.Join(",", parts.Select(part => part.Contains(",") ? $"\"{part}\"" : part));
                sw.WriteLine(line);
            }
        }

        public void Dispose()
        {
            sw?.Dispose();
        }
    }
}