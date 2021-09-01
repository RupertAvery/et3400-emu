using System;
using System.Runtime.CompilerServices;

namespace Core6800
{
    public abstract class Memory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract int ReadMem(int address);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void WriteMem(int address, int value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void SetMem(int address, int value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void And(int address, int value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Or(int address, int value);

        public virtual void Init()
        {

        }

        public abstract int Length { get; }

        public abstract int[] Data { get; }
    }

    public class Memory6800 : Memory
    {
        readonly int[] Memory = new int[65536];



        public override void Init()
        {
            // Set keyboard mapped memory 'high'
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
        }

        public override int Length
        {
            get { return Memory.Length; }
        }

        public override int[] Data
        {
            get { return Memory; }
        }

        public override void And(int address, int value)
        {
            Memory[address] &= value;
        }

        public override void Or(int address, int value)
        {
            Memory[address] |= value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int ReadMem(int address)
        {
            address = address & 0xFFFF;

            //if (address >= 0x1000 && address <= 0x1003)
            //{
            //    //_mc6820.RegisterSelect(address & 3);
            //    //return _mc6820.Get();
            //    Debug.WriteLine($"Read ${this.State.PC:X4}: ${address:X4}");
            //    return _mc6820.Get(address & 3);
            //}

            //foreach (var watch in Watches.ToList())
            //{
            //    if (watch.EventType == EventType.Read)
            //    {
            //        //if (watch.Address == address)
            //        //{

            //        //}
            //        watch.Action(new WatchEventArgs() { Address = address });
            //    }
            //}

            return Memory[address];
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetMem(int address, int value)
        {
            Memory[address & 0xFFFF] = value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteMem(int address, int value)
        {
            address = address & 0xFFFF;

            //foreach (var watch in Watches.ToList())
            //{
            //    if (watch.EventType == EventType.Write)
            //    {
            //        //if (watch.Address == address)
            //        //{

            //        //}

            //        watch.Action(new WatchEventArgs() { Address = address });
            //    }
            //}

            //if (address >= 0x1000 && address <= 0x1003)
            //{
            //    //_mc6820.RegisterSelect(address & 3);
            //    //_mc6820.Set(value);
            //    _mc6820.Set(address & 3, value);
            //    return;
            //}

            if (address >= 0x1400 && address <= 0x1BFF)
            {
                // Prevent writing to ROM-mapped space
                return;
            }

            if (address >= 0x1C00 && address <= 0x23FF)
            {
                // Prevent writing to ROM-mapped space
                return;
            }

            if (address >= 0xFC00)
            {
                // Prevent writing to ROM-mapped space
                return;
            }

            // Limit writing to RAM addresses only?
            Memory[address] = value;
        }
    }


    public partial class Cpu6800
    {
        //public Func<int, int> ReadMem { get; set; }
        //public Action<int, int> WriteMem { get; set; }

    }
}
