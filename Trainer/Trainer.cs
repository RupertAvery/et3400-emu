using System;
using System.IO;
using System.Windows.Forms;
using Core6800;
using Sharp6800.Common;
using Sharp6800.Debugger;
using Sharp6800.Trainer.Threads;

namespace Sharp6800.Trainer
{
    /// <summary>
    /// Implementation of a ET-3400 Trainer simulation. Wraps the core emulator in the trainer hardware (keys + display) 
    /// </summary>
    public class Trainer
    {
        private SegDisplay _disp;

        public ITrainerRunner Runner { get; private set; }
        public Cpu6800 Emulator { get; private set; }
        public int[] Memory = new int[65536];
        public TrainerSettings Settings { get; set; }
        public Cpu6800State State { get; set; }

        public Trainer()
        {
            Settings = new TrainerSettings();

            State = new Cpu6800State();

            Emulator = new Cpu6800
                {
                    State = State,

                    ReadMem = address =>
                        {
                            address = address & 0xFFFF;

                            return Memory[address];
                        },

                    WriteMem = (address, value) =>
                        {
                            address = address & 0xFFFF;

                            if (address >= 0xFC00)
                            {
                                // Prevent writing to ROM-mapped space
                                return;
                            }

                            // For accurate emulation we should probably NOT write to memory mapped addresses
                            Memory[address] = value;

                            // Check if we're writing to memory-mapped display
                            // quick test - just check if we are in C100-C1FF
                            if ((address & 0xC100) == 0xC100)
                            {
                                var displayNo = (address & 0xF0) >> 4;
                                if (displayNo >= 1 && displayNo <= 6)
                                {
                                    // OUTCH flicker hack - assumes original OUTCH routine is intact
                                    if (Settings.EnableOUTCHHack && ((address & 0x08) == 0x08) && (Emulator.State.PC == 0xFE46))
                                    {
                                        // don't write to upper bits if in OUTCH routine
                                        return;
                                    }
                                    _disp.Write(address, value);
                                }
                            }
                        }
                };

            Runner = new StandardRunner(this);
            //Runner = new CycleExactRunner(this);

            // Set keyboard mapped memory 'high'
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
        }

        public void AddBreakPoint(int address)
        {
            Emulator.Breakpoint.Add(address);
        }

        public void SetupDisplay(PictureBox target)
        {
            _disp = new SegDisplay(target) { Memory = Memory };
        }

        public void Stop()
        {
            Runner.Quit();
        }

        public void Initialize()
        {
            Emulator.BootStrap();
            Runner.Continue();
        }

        /// <summary>
        /// Simulate keypress through memory-mapped locations C003-C006
        /// </summary>
        /// <param name="trainerKey"></param>
        public void PressKey(TrainerKeys trainerKey)
        {
            switch (trainerKey)
            {
                // pull appropriate bit at mem location LOW
                case TrainerKeys.Key0:
                    Memory[0xC006] &= 0xDF;
                    break;
                case TrainerKeys.Key1:// 1, ACCA
                    Memory[0xC006] &= 0xEF;
                    break;
                case TrainerKeys.Key2:// 2
                    Memory[0xC005] &= 0xEF;
                    break;
                case TrainerKeys.Key3:// 3
                    Memory[0xC003] &= 0xEF;
                    break;
                case TrainerKeys.Key4:// 4, INDEX
                    Memory[0xC006] &= 0xF7;
                    break;
                case TrainerKeys.Key5:// 5, CC
                    Memory[0xC005] &= 0xF7;
                    break;
                case TrainerKeys.Key6:// 6
                    Memory[0xC003] &= 0xF7;
                    break;
                case TrainerKeys.Key7:// 7, RTI;
                    Memory[0xC006] &= 0xFB;
                    break;
                case TrainerKeys.Key8:// 8
                    Memory[0xC005] &= 0xFB;
                    break;
                case TrainerKeys.Key9:// 9
                    Memory[0xC003] &= 0xFB;
                    break;
                case TrainerKeys.KeyA:// A, Auto
                    Memory[0xC006] &= 0xFD;
                    break;
                case TrainerKeys.KeyB:// B
                    Memory[0xC005] &= 0xFD;
                    break;
                case TrainerKeys.KeyC:// C
                    Memory[0xC003] &= 0xFD;
                    break;
                case TrainerKeys.KeyD:// D, Do
                    Memory[0xC006] &= 0xFE;
                    break;
                case TrainerKeys.KeyE:// E, Exam
                    Memory[0xC005] &= 0xFE;
                    break;
                case TrainerKeys.KeyF:// F
                    Memory[0xC003] &= 0xFE;
                    break;
                case TrainerKeys.KeyReset:// RESET
                    Emulator.State.Reset = 0;
                    break;
            }
        }

        /// <summary>
        /// Simulate releasing of keys
        /// </summary>
        /// <param name="trainerKey"></param>
        public void ReleaseKey(TrainerKeys trainerKey)
        {
            // just pull everything high. 
            // we're not monitoring multiple presses anyway
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
            switch (trainerKey)
            {
                case TrainerKeys.KeyReset:// RESET
                    Emulator.State.Reset = 1;
                    break;
            }
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
        public void LoadRamFile(string file, int offset = 0)
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


        public void LoadRam(string file)
        {
            try
            {
                if (file.ToLower().EndsWith(".ram"))
                {
                    LoadRamFile(file, 0);
                    return;
                }
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
            Emulator.State.PC = i;
        }

        public int DefaultClockSpeed
        {
            get { return 100000; }
        }

        public bool IsInBreak { get; set; }

        public void Break()
        {
            Runner.Quit();
            IsInBreak = true;
        }

        public void StepOver()
        {
            if (StepInto()) StepOutOf();
        }

        public void StepOutOf()
        {
            var nextOpCode = Memory[Emulator.State.PC] & 0xFF;
            do
            {
                Emulator.Execute();
            } while (!Disassembler.IsReturn(nextOpCode));
        }

        public bool StepInto()
        {
            var nextOpCode = Memory[Emulator.State.PC] & 0xFF;
            Emulator.Execute();
            return Disassembler.IsSubroutine(nextOpCode);
        }

    }

}
