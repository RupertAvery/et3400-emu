using System;
using System.Linq;
using System.Text;

namespace Sharp6800.Trainer
{
    public class S19Reader
    {
        private Trainer _trainer;

        public S19Reader(Trainer trainer)
        {
            _trainer = trainer;
        }

        public string Write(int address, int length)
        {
            var sb = new StringBuilder();
            var data = _trainer.Read(address, length);

            var offset = 0;

            while (offset < data.Length)
            {
                var byteCount = 31 - 3;

                if (data.Length - offset < byteCount)
                {
                    byteCount = data.Length - offset;
                }

                var buffer = new byte[byteCount];
                for (var i = 0; i < byteCount; i++)
                {
                    buffer[i] = data[offset + i];
                }

                var checkSum = CheckSum(byteCount, address + offset, buffer);

                var bufferHex = string.Join("", buffer.Select(b=> b.ToString("X2")));
                var addresshex = (address + offset).ToString("X4");
                var byteCountHex = (byteCount + 3).ToString("X2");
                var checkSumHex = checkSum.ToString("X2");

                sb.AppendLine($"S1{byteCountHex}{addresshex}{bufferHex}{checkSumHex}");

                offset += byteCount;
            }

            return sb.ToString();
        }

        private byte CheckSum(int byteCount, int address, byte[] data)
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

        public int Read(string content)
        {
            var bytes = 0;
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
                        //        S0	Header	16-bit
                        //"0000"	Yes	This record contains vendor specific ASCII text represented as a series of hex digit pairs. It is common to see the data for this record in the format of a null-terminated string. The text data can be anything including a mixture of the following information: file/module name, version/revision number, date/time, product name, vendor name, memory designator on PCB, copyright notice. It is common to see: 48 44 52 which is the ASCII H, D, and R - "HDR".[1]
                        case "S1":
                            //S1	Data	16-bit
                            //Address	Yes
                            //This record contains data that starts at the 16-bit address field.
                            //[2] This record is typically used for 8-bit microcontrollers, such as AVR, PIC, 8051, 68xx, 6502, 80xx, Z80.
                            //The number of bytes of data contained in this record is "Byte Count Field" minus 3,
                            //which is 2 bytes for "16-bit Address Field" and 1 byte for "Checksum Field".

                            addr = line.Substring(4, 4);
                            data = line.Substring(8, bytecount * 2 - 6);

                            var addrVal = Convert.ToInt32(addr, 16);

                            bytes += _trainer.Write(addr, data);

                            checksum = line.Substring(bytecount * 2 + 2, 2);
                            var checksumActual = Convert.ToInt32(checksum, 16);

                            break;
                        case "S2":
                            //S2	Data	24-bit
                            //Address	Yes	This record contains data that starts at a 24-bit address.[2] The number of bytes of data contained in this record is "Byte Count Field" minus 4, which is 3 bytes for "24-bit Address Field" and 1 byte for "Checksum Field".
                            break;
                        case "S3":
                            //S3	Data	32-bit
                            //Address	Yes	This record contains data that starts at a 32-bit address.[2] This record is typically used for 32-bit microcontrollers, such as ARM and 680x0. The number of bytes of data contained in this record is "Byte Count Field" minus 5, which is 4 bytes for "32-bit Address Field" and 1 byte for "Checksum Field".
                            //S4	Reserved	N/A	N/A	This record is reserved.
                            //S5	Count	16-bit
                            //Count	No	This optional record contains a 16-bit count of S1 / S2 / S3 records.[2] This record is used if the record count is less than or equal to 65,535 (0xFFFF), otherwise S6 record would be used.
                            //S6	Count	24-bit
                            //Count	No	This optional record contains a 24-bit count of S1 / S2 / S3 records. This record is used if the record count is less than or equal to 16,777,215 (0xFFFFFF). If less than 65,536 (0x010000), then S5 record would be used. NOTE: This newer record is the most recent change (not sure if official).[2]
                            break;
                        case "S7":
                            //S7	Start Address
                            //(Termination)	32-bit
                            //Address	No	This record contains the starting execution location at a 32-bit address.[2][3] This is used to terminate a series of S3 records. If a SREC file is only used to program a memory device and the execution location is ignored, then an address of zero could be used.
                            break;
                        case "S8":
                            //S8	Start Address
                            //(Termination)	24-bit
                            //Address	No	This record contains the starting execution location at a 24-bit address.[2][3] This is used to terminate a series of S2 records. If a SREC file is only used to program a memory device and the execution location is ignored, then an address of zero could be used.
                            break;
                        case "S9":
                            //S9	Start Address
                            //(Termination)	16-bit
                            //Address	No	This record contains the starting execution location at a 16-bit address.[2][3] This is used to terminate a series of S1 records. If a SREC file is only used to program a memory device and the execution location is ignored, then an address of zero could be used.
                            break;
                    }
                }
            }

            return bytes;
        }

    }
}