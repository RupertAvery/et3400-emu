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
            emu.State.PC = 0;
        }

        [TestMethod]
        public void ADDA_IMMED()
        {
            Write("0000", "8B01");
            emu.ClearFlags();
            emu.State.PC = 0;
            emu.Execute();
            Assert.AreEqual(1, emu.State.A);
            //Assert.AreEqual(0, emu.Flags.H);
            //Assert.AreEqual(0, emu.Flags.I);
            //Assert.AreEqual(0, emu.Flags.N);
            //Assert.AreEqual(0, emu.Flags.Z);
            //Assert.AreEqual(0, emu.Flags.V);
            //Assert.AreEqual(0, emu.Flags.C);

            Write("0000", "860F8B01");
            emu.ClearFlags();
            emu.State.PC = 0;
            emu.Execute();
            emu.Execute();
            Assert.AreEqual(0x10, emu.State.A);
            Assert.AreEqual(0x20, emu.State.CC);

            Write("0000", "86FF8B01");
            emu.ClearFlags();
            emu.State.PC = 0;
            emu.Execute();
            emu.Execute();
            Assert.AreEqual(0x00, emu.State.A);
            Assert.AreEqual(0x21, emu.State.CC);
        }

        [TestMethod]
        public void ADDA_DIRECT()
        {
            Write("0000", "01");
            Write("0001", "9B00");
            emu.State.PC = 1;
            emu.Execute();
            Assert.AreEqual(1, emu.State.A);
            //Assert.AreEqual(0, emu.Flags.H);
            //Assert.AreEqual(0, emu.Flags.I);
            //Assert.AreEqual(0, emu.Flags.N);
            //Assert.AreEqual(0, emu.Flags.Z);
            //Assert.AreEqual(0, emu.Flags.V);
            //Assert.AreEqual(0, emu.Flags.C);
        }

    }
}
