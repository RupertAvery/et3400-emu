using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Core6800;

namespace Sharp6800
{
    public delegate void OnUpdateDelegate(Cpu6800 emu);

    /// <summary>
    /// Implementation of a ET-3400 Trainer simulation. Wraps the core emulator in the trainer hardware (keys + display) 
    /// </summary>
    class Trainer
    {
        public enum Keys
        {
            Key0,
            Key1,
            Key2,
            Key3,
            Key4,
            Key5,
            Key6,
            Key7,
            Key8,
            Key9,
            KeyA,
            KeyB,
            KeyC,
            KeyD,
            KeyE,
            KeyF,
            KeyReset
        }

        Thread runner;
        Cpu6800 emu;
        SegDisplay disp;
        public int[] Memory = new int[65536];

        public Trainer()
        {
            State = new Cpu6800State();
            emu = new Cpu6800
                {
                    State = State,
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

            // Set keyboard mapped memory 'high'
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;

        }

        public Cpu6800State State { get; set; }
        public void AddBreakPoint(int address)
        {
            emu.Breakpoint.Add(address);
        }

        public void SetupDisplay(PictureBox target)
        {
            disp = new SegDisplay(target);
        }

        public void Start()
        {
            emu.Reset();
            runner = new Thread(EmuThread);
            runner.Start();
        }

        private bool _running;
        private readonly object _lockobject = new object();

        public event OnUpdateDelegate OnUpdate;

        public void EmuThread()
        {
            int cycles = 0;
            _running = true;

            while (_running)
            {

                lock (_lockobject)
                {
                    cycles += emu.Execute();
                }

                if (cycles % 128 == 0)
                {
                    IntHandler();
                    Thread.Sleep(1);
                }

                //1,048,576 cycles/sec = 1MHz 
                //17476 = 1MHz / 60 = 60Hz interrupt rate
                if (cycles >= 17476)
                {
                    //var tms = stopwatch.Elapsed.TotalMilliseconds;
                    //stopwatch.Reset();
                    IntHandler();
                    cycles = 0;
                    Thread.Sleep(40);
                }

            }
        }



        public void IntHandler()
        {
            // Update the 7-seg display
            disp.Display(Memory);
            if (OnUpdate != null) OnUpdate(emu);
        }

        /// <summary>
        /// Sets the quit flag on the emulator and waits for execution of the current opcode to complete
        /// </summary>
        public void Quit()
        {
            // wait for emulation thread to terminate
            _running = false;

            if (runner != null)
            {
                while (runner.IsAlive)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// Simulate keypress through memory-mapped locations C003-C006
        /// </summary>
        /// <param name="key"></param>
        public void PressKey(Keys key)
        {
            switch (key)
            {
                // pull appropriate bit at mem location LOW
                case Keys.Key0:
                    Memory[0xC006] &= 0xDF;
                    break;
                case Keys.Key1:// 1, ACCA
                    Memory[0xC006] &= 0xEF;
                    break;
                case Keys.Key2:// 2
                    Memory[0xC005] &= 0xEF;
                    break;
                case Keys.Key3:// 3
                    Memory[0xC003] &= 0xEF;
                    break;
                case Keys.Key4:// 4, INDEX
                    Memory[0xC006] &= 0xF7;
                    break;
                case Keys.Key5:// 5, CC
                    Memory[0xC005] &= 0xF7;
                    break;
                case Keys.Key6:// 6
                    Memory[0xC003] &= 0xF7;
                    break;
                case Keys.Key7:// 7, RTI;
                    Memory[0xC006] &= 0xFB;
                    break;
                case Keys.Key8:// 8
                    Memory[0xC005] &= 0xFB;
                    break;
                case Keys.Key9:// 9
                    Memory[0xC003] &= 0xFB;
                    break;
                case Keys.KeyA:// A, Auto
                    Memory[0xC006] &= 0xFD;
                    break;
                case Keys.KeyB:// B
                    Memory[0xC005] &= 0xFD;
                    break;
                case Keys.KeyC:// C
                    Memory[0xC003] &= 0xFD;
                    break;
                case Keys.KeyD:// D, Do
                    Memory[0xC006] &= 0xFE;
                    break;
                case Keys.KeyE:// E, Exam
                    Memory[0xC005] &= 0xFE;
                    break;
                case Keys.KeyF:// F
                    Memory[0xC003] &= 0xFE;
                    break;
                case Keys.KeyReset:// RESET
                    lock (_lockobject)
                    {
                        emu.Reset();
                    }
                    break;
            }
        }

        /// <summary>
        /// Simulate releasing of keys
        /// </summary>
        /// <param name="key"></param>
        public void ReleaseKey(Keys key)
        {
            // just pull everything high. 
            // we're not monitoring multiple presses anyway
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
        }


        /// <summary>
        /// Loads the specified binary file into the upper end of memory
        /// </summary>
        /// <param name="file"></param>
        public void LoadROM(string file)
        {
            try
            {
                var ext = Path.GetExtension(file).ToLower();
                if (ext == ".rom")
                {
                    byte[] rom = File.ReadAllBytes(file);
                    int offset = 65536 - rom.Length;
                    for (var i = 0; i < rom.Length; i++)
                    {
                        Memory[offset + i] = rom[i];
                    }
                    //using (var fs = new StreamWriter("zenith.hex"))
                    //{
                    //    int j = 0;
                    //    foreach (var b in rom)
                    //    {
                    //        fs.Write(string.Format("{0:X2}", b));
                    //        j++;
                    //        if (j == 32)
                    //        {
                    //            fs.Write("\r\n");
                    //            j = 0;
                    //        }
                    //    }
                    //}
                }
                else if (ext == ".hex")
                {
                    string content = File.ReadAllText(file);
                    var lines = content.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    byte[] rom = new byte[lines.Length * 32];

                    int j = 0;
                    foreach (var line in lines)
                    {
                        for (int i = 0; i < 32; i++)
                        {
                            rom[j * 32 + i] = (byte)Convert.ToInt32(line.Substring(i * 2, 2), 16);
                        }
                        j++;
                    }

                    int offset = 65536 - rom.Length;
                    for (var i = 0; i < rom.Length; i++)
                    {
                        Memory[offset + i] = rom[i];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while loading the ROM file " + file + ".", ex);
            }
        }

        /// <summary>
        /// Load binary file into RAM area at the specified offset
        /// </summary>
        /// <param name="file"></param>
        /// <param name="offset"></param>
        public void LoadRAM(string file, int offset = 0)
        {
            try
            {
                byte[] ram = File.ReadAllBytes(file);
                for (var i = 8; i < ram.Length; i++)
                {
                    Memory[offset + i - 8] = ram[i];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while loading the RAM file " + file + ".", ex);
            }

        }


        public void LoadSREC(string file)
        {
            try
            {
                var content = File.ReadAllText(file);
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
                            case "S1":
                                addr = line.Substring(4, 4);
                                data = line.Substring(8, bytecount * 2 - 6);
                                Write(addr, data);
                                checksum = line.Substring(bytecount * 2 + 2, 2);
                                break;
                            case "S2":
                                break;
                            case "S3":
                                break;
                            case "S7":
                                break;
                            case "S8":
                                break;
                            case "S9":
                                break;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while loading the OBJ file " + file + ".", ex);
            }
        }

        public void Write(string addr, string data)
        {
            int baseAddr = Convert.ToInt32(addr, 16);
            for (int p = 0; p < data.Length; p += 2)
            {
                Memory[baseAddr + p / 2] = Convert.ToInt32(data.Substring(p, 2), 16);
            }
        }

        public void SetProgramCounter(int i)
        {
            emu.State.PC = i;
        }
    }

}
