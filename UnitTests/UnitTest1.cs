using System;
using Core6800;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    public class TestMemory : Memory
    {
        readonly int[] _memory = new int[65536];

        public override int ReadMem(int address)
        {
            address = address & 0xFFFF;
            return _memory[address];
        }

        public override void WriteMem(int address, int value)
        {
            address = address & 0xFFFF;
            if (address >= 0xFC00)
            {
                return;
            }
            _memory[address] = value;
        }

        public override void SetMem(int address, int value)
        {
            address = address & 0xFFFF;
            if (address >= 0xFC00)
            {
                return;
            }
            _memory[address] = value;
        }


        public override void And(int address, int value)
        {
            throw new NotImplementedException();
        }

        public override void Or(int address, int value)
        {
            throw new NotImplementedException();
        }

        public override int Length => _memory.Length;

        public override int[] Data => _memory;
    }

    [TestClass]
    public class UnitTest1
    {
        private Cpu6800 emu;
        private Memory mem;



        private void Write(string addr, string data)
        {
            int baseAddr = Convert.ToInt32(addr, 16);
            for (int p = 0; p < data.Length; p += 2)
            {
                mem.WriteMem(baseAddr + p / 2, Convert.ToInt32(data.Substring(p, 2), 16));
            }
        }

        [TestInitialize]
        public void Init()
        {
            mem = new Memory6800();

            emu = new Cpu6800
            {
                State = new Cpu6800State(),
                Memory = mem
            };
            emu.State.CC = 0;
            emu.State.PC = 0;
        }

        private void ExpectAToBe(int value)
        {
            Assert.AreEqual(value, emu.State.A);
        }

        private void ExpectBToBe(int value)
        {
            Assert.AreEqual(value, emu.State.A);
        }

        private void ExpectCCToBe(int value)
        {
            Assert.AreEqual(value, emu.State.CC);
        }

        private void ExpectCCToBe(string value)
        {
            Assert.AreEqual(value, emu.State.CC);
        }


        private void ResetEmu(string addr = "0000")
        {
            int baseAddr = Convert.ToInt32(addr, 16);
            emu.State.CC = 0x00;
            emu.State.PC = baseAddr;
            emu.State.InterruptRequest = 1;
            emu.State.NonMaskableInterrupt = 1;
            emu.State.A = 0x00;
            emu.State.B = 0x00;
            emu.State.S = 0xd9;
            emu.State.X = 0x0000;
            // Clear first 255 bytes of memory
            for (int i = 0; i < 0xFF; i++)
            {
                mem.WriteMem(i, 0);
            }
        }

        private void RunToNop()
        {
            while (mem.ReadMem(emu.State.PC) != 0x01)
            {
                emu.Execute();
            }
        }

        private void LoadProgram(string code, string progstart = "0000")
        {
            ResetEmu(progstart);
            Write("0000", code + "01");
        }

        [TestMethod]
        public void ADDA_IMMED()
        {
            LoadProgram("860F8B01");
            RunToNop();
            ExpectAToBe(0x10);
            ExpectCCToBe(0x20);

            LoadProgram("86FF8B01");
            RunToNop();
            ExpectAToBe(0x00);
            // HINZVC-100101
            //ExpectCCToBe(0x21);
            ExpectCCToBe(0x25);
        }

        [TestMethod]
        public void ADDA_DIRECT()
        {
            LoadProgram("019B00", "0001");
            RunToNop();
            ExpectAToBe(1);
        }

        [TestMethod]
        public void BLS()
        {
            LoadProgram("860081042302860101");
            RunToNop();
            ExpectAToBe(0);
        }

    }
}
