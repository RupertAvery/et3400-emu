using System;
using Core6800;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private Cpu6800 emu;
        int[] Memory = new int[65536];

        private void Write(string addr, string data)
        {
            int baseAddr = Convert.ToInt32(addr, 16);
            for (int p = 0; p < data.Length; p += 2)
            {
                Memory[baseAddr + p / 2] = Convert.ToInt32(data.Substring(p, 2), 16);
            }
        }

        [TestInitialize]
        public void Init()
        {
            emu = new Cpu6800
                {
                    State = new Cpu6800State(),
                    ReadMem = loc =>
                        {
                            loc = loc & 0xFFFF;
                            return Memory[loc];
                        },
                    WriteMem = (loc, value) =>
                        {
                            loc = loc & 0xFFFF;
                            if (loc >= 0xFC00)
                            {
                                return;
                            }
                            Memory[loc] = value;
                        }
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
            emu.State.IRQ = 1;
            emu.State.NMI = 1;
            emu.State.A = 0x00;
            emu.State.B = 0x00;
            emu.State.S = 0xd9;
            emu.State.X = 0x0000;
            // Clear first 255 bytes of memory
            for (int i = 0; i < 0xFF; i++)
            {
                Memory[i] = 0;
            }
        }

        private void RunToNop()
        {
            while (Memory[emu.State.PC] != 0x01)
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

    }
}
