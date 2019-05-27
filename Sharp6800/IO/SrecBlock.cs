using System.Collections;
using System.Text;

namespace Sharp6800.Trainer
{
    public class SrecBlock
    {
        public int ByteCount { get; set; }
        public int Address { get; set; }
        public int[] Data { get; set; }
    }
}