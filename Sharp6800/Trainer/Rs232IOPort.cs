using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sharp6800.Trainer
{
    public class Rs232IOPort
    {
        private int sendState = 0;
        private int sendBuffer = 0;
        private int rcvState = 0;
        private int rcvBuffer = 0x7F;

        public void Send(int value)
        {
            value &= 1;

            if (sendState == 0)
            {
                if (value == 0)
                {
                    sendBuffer = 0;
                    sendState++;
                }
            }
            else if (sendState == 9)
            {
                if (value == 1)
                {
                    if (sendBuffer > 0)
                    {
                        Debug.Write($"{new string((char)sendBuffer, 1)}");
                    }
                    sendState = 0;
                }
            }
            else
            {
                sendBuffer |= value << sendState - 1;
                sendState++;
            }
        }

        private Queue<int> inputBuffer = new Queue<int>();
        private int tempBuffer;

        public void FeedString(string value)
        {
            foreach (var chr in value)
            {
                inputBuffer.Enqueue(chr);
            }
        }

        public void FeedCharacter(int value)
        {
            inputBuffer.Enqueue(value);
        }

        public int Recieve()
        {
            var value = 0x7F;

            if (rcvState == 0)
            {
                if (inputBuffer.Count > 0)
                {
                    tempBuffer = inputBuffer.Dequeue();
                    rcvState++;
                    // Expects it to be negative??
                    value = 0xFF;
                }
            }
            else if (rcvState == 9)
            {
                rcvState = 0;
                value = 0x01;
            }
            else
            {
                //value = 0x7E | (tempBuffer >> rcvState - 1);
                value = (tempBuffer >> rcvState - 1) & 1;
                value = value << 1;
                value = value | 1;
                rcvState++;
            }

            return value;
        }

    }
}