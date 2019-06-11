using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ET3400.Trainer
{
    public enum Peripheral
    {
        PRA,
        PRB
    }

    public enum RegisterSelected
    {
        PRA,
        DDRA,
        CRA,
        PRB,
        DDRB,
        CRB,
    }

    public class PeripheralEventArgs
    {
        public Peripheral Peripheral { get; set; }
        public int Value { get; set; }

        public PeripheralEventArgs(Peripheral peripheral)
        {
            Peripheral = peripheral;
            Value = 0;
        }

        public PeripheralEventArgs(Peripheral peripheral, int value)
        {
            Peripheral = peripheral;
            Value = value;
        }

    }

    public class DebugConsoleAdapter : RS232Adapder
    {
        public override void WriteByte(int value)
        {
            Debug.Write($"{new string((char)value, 1)}");
        }
    }

    public abstract class RS232Adapder
    {
        private int sendState = 0;
        private int sendBuffer = 0;
        private int rcvState = 0;
        private int rcvBuffer = 0x7F;
        private Queue<int> inputBuffer = new Queue<int>();
        private int tempBuffer;

        public abstract void WriteByte(int value);

        public void WriteString(string value)
        {
            foreach (var chr in value)
            {
                inputBuffer.Enqueue(chr);
            }
        }

        public void WriteCharacter(int value)
        {
            inputBuffer.Enqueue(value);
        }

        public int Write()
        {
            var value = 0x7F;

            if (rcvState == 0)
            {
                if (inputBuffer.Count > 0)
                {
                    tempBuffer = inputBuffer.Dequeue();
                    rcvState++;
                    value = 0xFF;
                }
            }
            else if (rcvState == 1)
            {
                value = 0xFF;
                rcvState++;
            }
            else if (rcvState == 2)
            {
                value = 0x7F;
                rcvState++;
            }
            else if (rcvState == 11)
            {
                rcvState = 0;
                value = 0x7F;
            }
            else
            {
                //value = 0x7E | (tempBuffer >> rcvState - 1);
                value = tempBuffer << 7 - (rcvState - 3);
                value = value & 0x80;
                value = value | 0b1110;
                rcvState++;
            }

            return value;
        }


        public void Read(int value)
        {
            //value &= 1;

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
                        WriteByte(sendBuffer);
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
    }

    public class MC6820
    {
        /**
         * RS0/1 = Register Select 0/1
         * CRA/B = Control Register A/B
         * DDRA/B = Data Direction Register A/B
         * PRA/B = Peripheral Register A/B
         */

        private int CRA = 0;
        private int CRB = 0;

        private int DDRA = 0;
        private int DDRB = 0;

        private int PRA = 0;
        private int PRB = 0;


        private RegisterSelected registerSelected = RegisterSelected.DDRA;

        public EventHandler<PeripheralEventArgs> OnPeripheralWrite { get; set; }
        public EventHandler<PeripheralEventArgs> OnPeripheralRead { get; set; }

        public void Set(int registerSelect, int value)
        {
            switch (registerSelect)
            {
                // RS1 = 0, RS0 = 0
                case 0:
                    switch (CRA & 4)
                    {
                        // CRA-B4 = 1
                        case 4:
                            PRA = value;
                            OnPeripheralWrite?.Invoke(this, new PeripheralEventArgs(Peripheral.PRA, value));
                            return;
                        // CRA-B4 = 0
                        case 0:
                            DDRA = value;
                            return;
                    }
                    break;
                // RS1 = 0, RS0 = 1
                case 1:
                    CRA = value;
                    return;
                // RS1 = 1, RS0 = 0
                case 2:
                    switch (CRB & 4)
                    {
                        // CRB-B4 = 1
                        case 4:
                            PRB = value;
                            OnPeripheralWrite?.Invoke(this, new PeripheralEventArgs(Peripheral.PRB, value));
                            return;
                        // CRB-B4 = 0
                        case 0:
                            DDRB = value;
                            return;
                    }
                    break;
                // RS1 = 1, RS0 = 1
                case 3:
                    CRB = value;
                    return;
            }
            throw new Exception("Invalid state");
        }

        public void Set(int value)
        {
            switch (registerSelected)
            {
                case RegisterSelected.PRA:
                    PRA = value;
                    OnPeripheralWrite?.Invoke(this, new PeripheralEventArgs(Peripheral.PRA, value));
                    break;
                case RegisterSelected.DDRA:
                    DDRA = value;
                    break;
                case RegisterSelected.CRA:
                    CRA = value;
                    break;
                case RegisterSelected.PRB:
                    PRB = value;
                    OnPeripheralWrite.Invoke(this, new PeripheralEventArgs(Peripheral.PRB, value));
                    break;
                case RegisterSelected.DDRB:
                    DDRB = value;
                    break;
                case RegisterSelected.CRB:
                    CRB = value;
                    break;
            }
            //Debug.WriteLine("WRITE: {0:X4}", value);
        }


        public int Get(int registerSelect)
        {
            switch (registerSelect)
            {
                // RS1 = 0, RS0 = 0
                case 0:
                    switch (CRA & 4)
                    {
                        // CRA-B4 = 1
                        case 4:
                            var eventArgs = new PeripheralEventArgs(Peripheral.PRA);
                            OnPeripheralRead.Invoke(this, eventArgs);
                            return eventArgs.Value;
                        // CRA-B4 = 0
                        case 0:
                            return DDRA;
                    }
                    break;
                // RS1 = 0, RS0 = 1
                case 1:
                    return CRA;
                // RS1 = 1, RS0 = 0
                case 2:
                    switch (CRB & 4)
                    {
                        // CRB-B4 = 1
                        case 4:
                            var eventArgs = new PeripheralEventArgs(Peripheral.PRB);
                            OnPeripheralRead.Invoke(this, eventArgs);
                            return eventArgs.Value;
                        // CRB-B4 = 0
                        case 0:
                            return DDRB;
                    }
                    break;
                // RS1 = 0, RS0 = 1
                case 3:
                    return CRB;
            }
            throw new Exception("Invalid state");
        }

        public int Get()
        {
            var value = 0x7F;

            switch (registerSelected)
            {
                case RegisterSelected.PRA:
                    return PRA;
                case RegisterSelected.DDRA:
                    return DDRA;
                case RegisterSelected.CRA:
                    return CRA;
                case RegisterSelected.PRB:
                    return PRB;
                case RegisterSelected.DDRB:
                    return DDRB;
                case RegisterSelected.CRB:
                    return CRB;
            }

            return value;
        }

        public void RegisterSelect(int value)
        {
            switch (value & 3)
            {
                case 0:
                    switch (CRA & 4)
                    {
                        case 4:
                            registerSelected = RegisterSelected.PRA;
                            break;
                        case 0:
                            registerSelected = RegisterSelected.DDRA;
                            break;
                    }
                    break;
                case 1:
                    registerSelected = RegisterSelected.CRA;
                    break;
                case 2:
                    switch (CRB & 4)
                    {
                        case 4:
                            registerSelected = RegisterSelected.PRB;
                            break;
                        case 0:
                            registerSelected = RegisterSelected.DDRB;
                            break;
                    }
                    break;
                case 3:
                    registerSelected = RegisterSelected.CRB;
                    break;
            }
        }
    }
}