using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ET3400.IO
{
    public class SrecWriter : IDisposable
    {
        private readonly StreamWriter streamWriter;

        public SrecWriter(Stream stream)
        {
            streamWriter = new StreamWriter(stream);
        }

        public void WriteAll(IEnumerable<SrecBlock> srecBlocks)
        {
            foreach (var block in srecBlocks)
            {
                var checkSum = SrecHelper.CalculateCheckSum(block.ByteCount, block.Address, block.Data);

                var bufferHex = string.Join("", block.Data.Select(b => b.ToString("X2")));
                var addresshex = block.Address.ToString("X4");
                var byteCountHex = (block.ByteCount + 3).ToString("X2");
                var checkSumHex = checkSum.ToString("X2");

                streamWriter.WriteLine($"S1{byteCountHex}{addresshex}{bufferHex}{checkSumHex}");
            }
        }

        public void Dispose()
        {
            streamWriter?.Dispose();
        }
    }
}