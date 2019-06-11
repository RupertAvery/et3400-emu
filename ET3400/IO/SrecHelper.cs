using System.Collections.Generic;

namespace ET3400.IO
{
    public class SrecHelper
    {

        public static byte CalculateCheckSum(int byteCount, int address, int[] data)
        {
            var checksumVal = byteCount;
            checksumVal += address;
            for (var p = 0; p < data.Length; p += 2)
            {
                var byteVal = data[p];
                checksumVal += byteVal;
            }
            checksumVal = checksumVal & 0xff;
            checksumVal = ~checksumVal;
            return (byte)checksumVal;
        }

        public static IEnumerable<SrecBlock> ToSrecBlocks(int address, int[] data)
        {
            var offset = 0;

            while (offset < data.Length)
            {
                var byteCount = 31 - 3;

                if (data.Length - offset < byteCount)
                {
                    byteCount = data.Length - offset;
                }

                var buffer = new int[byteCount];
                for (var i = 0; i < byteCount; i++)
                {
                    buffer[i] = data[offset + i];
                }

                yield return new SrecBlock()
                {
                    Address = address + offset,
                    Data = buffer
                };

                offset += byteCount;
            }


        }

    }
}