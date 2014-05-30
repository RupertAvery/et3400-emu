using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Core6800;
using Timer = System.Threading.Timer;

namespace Sharp6800
{
    public delegate void OnUpdateDelegate(Cpu6800 emu);
    public delegate void OnTimerDelegate(int cyclePerSecond);

    /// <summary>
    /// Implementation of a ET-3400 Trainer simulation. Wraps the core emulator in the trainer hardware (keys + display) 
    /// </summary>
    public class Trainer
    {
        private bool _running;
        private readonly object _lockobject = new object();
        private Thread _runner;
        private readonly Cpu6800 _emu;
        private SegDisplay _disp;
        public int CyclesPerSecond { get; private set; }

        public int[] Memory = new int[65536];
        public event OnUpdateDelegate OnUpdate;
        public event OnTimerDelegate OnTimer;

        public Cpu6800State State { get; set; }
        private int _cycles;
        private int _lastCycles;
        private Timer _timer;
        private Timer _displayTimer;
        private int limit = 100;
        public int ClockSpeed { get; set; }
        public int sleeps;

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

        public Trainer()
        {
            ClockSpeed = 100000;

            State = new Cpu6800State();

            _emu = new Cpu6800
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

                            if (loc >= 0xC110 && loc <= 0xC16F && ((loc & 0x0F) == 0x00))
                            {
                                _disp.Write(loc, value);
                                //                                _disp.Display(Memory);
                            }
                            //if (OnUpdate != null) OnUpdate(_emu);
                        }
                };



            // Set keyboard mapped memory 'high'
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
        }

        public void AddBreakPoint(int address)
        {
            _emu.Breakpoint.Add(address);
        }

        public void SetupDisplay(PictureBox target)
        {
            _disp = new SegDisplay(target);
        }

        public void Start()
        {
            _emu.Reset();
            _runner = new Thread(EmuThread);
            _timer = new Timer(state => CheckSpeed(), null, 0, 1000);
            //_displayTimer = new Timer(state => _disp.Display(Memory), null, 0, 50);
            _runner.Start();
        }

        private void CheckSpeed()
        {
            CyclesPerSecond = _cycles - _lastCycles;
            if (OnTimer != null) OnTimer(CyclesPerSecond);
            if (CyclesPerSecond > ClockSpeed)
            {
                limit -= (CyclesPerSecond - ClockSpeed) / 1000;
            }
            else if (CyclesPerSecond < ClockSpeed)
            {
                limit += (ClockSpeed - CyclesPerSecond) / 1000;
            }
            Debug.WriteLine(limit);
            Debug.WriteLine(sleeps);
            sleeps = 0;
            _lastCycles = _cycles;
        }

        private void EmuThread()
        {
            _running = true;
            var loopCycles = 0;

            while (_running)
            {
                var cycles = 0;
                lock (_lockobject)
                {
                    cycles = _emu.Execute();
                    loopCycles += cycles;
                    _cycles += cycles;
                }

                if (loopCycles > limit)
                {
                    loopCycles = 0;
                    Thread.Sleep(1);
                    sleeps++;
                }
            }
        }

        public void IntHandler()
        {
            // Update the 7-seg display
            _disp.Display(Memory);
            if (OnUpdate != null) OnUpdate(_emu);
        }

        /// <summary>
        /// Sets the quit flag on the emulator and waits for execution of the current opcode to complete
        /// </summary>
        public void Quit()
        {
            // wait for emulation thread to terminate
            _running = false;

            if (_runner != null)
            {
                while (_runner.IsAlive)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
            }
            if (_timer != null)
            {
                _timer.Dispose();
            }
            if (_displayTimer != null)
            {
                _displayTimer.Dispose();
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
                        _emu.Reset();
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
                    var content = File.ReadAllText(file);
                    var lines = content.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    var rom = new byte[lines.Length * 32];

                    var j = 0;
                    foreach (var line in lines)
                    {
                        for (var i = 0; i < 32; i++)
                        {
                            rom[j * 32 + i] = (byte)Convert.ToInt32(line.Substring(i * 2, 2), 16);
                        }
                        j++;
                    }

                    var offset = 65536 - rom.Length;
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
            var baseAddr = Convert.ToInt32(addr, 16);
            for (var p = 0; p < data.Length; p += 2)
            {
                Memory[baseAddr + p / 2] = Convert.ToInt32(data.Substring(p, 2), 16);
            }
        }

        public void SetProgramCounter(int i)
        {
            _emu.State.PC = i;
        }

        public int DefaultClockSpeed {
            get { return 100000; } 
        }
    }

}
