using System;
using System.Collections.Generic;
using System.IO;

namespace ET3400.IO
{
    public class CsvReader : IDisposable
    {
        private string data;
        private StreamReader sr;
        private string filepath;

        public CsvReader(Stream stream)
        {
            sr = new StreamReader(stream);
        }

        public IEnumerable<string[]> ReadAll()
        {
            data = sr.ReadToEnd();

            var lines = data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            char[] buffer = new char[256];

            foreach (var line in lines)
            {
                var ptr = 0;
                var parts = new List<string>();
                var inQuotes = false;
                var bufferPtr = 0;
                var quoteCount = 0;
                while (ptr < line.Length)
                {
                    var chr = line[ptr];
                    if (chr == ',' && !inQuotes)
                    {
                        parts.Add(new string(buffer, 0, bufferPtr));
                        bufferPtr = 0;
                        quoteCount = 0;
                    }
                    else
                    {
                        if (chr == '"')
                        {
                            if (quoteCount == 0)
                            {
                                bufferPtr = 0;
                            }
                            inQuotes = !inQuotes;
                            quoteCount++;
                        }
                        else
                        {
                            buffer[bufferPtr] = chr;
                            bufferPtr++;
                        }
                    }
                    ptr++;
                }

                if (inQuotes) { throw new Exception("Unterminated quote"); }

                parts.Add(new string(buffer, 0, bufferPtr));

                yield return parts.ToArray();
            }
        }

        public void Dispose()
        {
            sr?.Dispose();
        }
    }
}