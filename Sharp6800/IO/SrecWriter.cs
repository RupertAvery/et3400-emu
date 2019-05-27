using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sharp6800.Trainer
{
    public class SrecWriter
    {
        private readonly Stream stream;

        public SrecWriter(Stream stream)
        {
            this.stream = stream;
        }

        public void Write(IEnumerable<SrecBlock> srecBlocks)
        {
            var sw = new StreamWriter(stream);

            foreach (var block in srecBlocks)
            {
                var checkSum = SrecHelper.CalculateCheckSum(block.ByteCount, block.Address, block.Data);

                var bufferHex = string.Join("", block.Data.Select(b => b.ToString("X2")));
                var addresshex = block.Address.ToString("X4");
                var byteCountHex = (block.ByteCount + 3).ToString("X2");
                var checkSumHex = checkSum.ToString("X2");

                sw.Write($"S1{byteCountHex}{addresshex}{bufferHex}{checkSumHex}");
            }
        }
    }
}