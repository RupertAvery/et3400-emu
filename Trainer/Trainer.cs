using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Core6800;
using Sharp6800.Common;
using Sharp6800.Debugger;
using Sharp6800.Trainer.Threads;
using Timer = System.Threading.Timer;

namespace Sharp6800.Trainer
{
    public struct WatchEventArgs
    {
        public Cpu6800State State { get; set; }
        public int Address { get; set; }
        public int Value { get; set; }
    }

    public struct Watch
    {
        public EventType EventType { get; set; }
        public int Address { get; set; }
        public int Value { get; set; }
        public Action<WatchEventArgs> Action { get; set; }
    }

    public interface IIODevice
    {
        int Read(int address);
        void Write(int address, int value);
    }


    /// <summary>
    /// Implementation of a ET-3400 Trainer simulation. Wraps the core emulator in the trainer hardware (keys + display) 
    /// </summary>
    public class Trainer : ITrainer, IDisposable
    {
        private SegDisplay _disp;

        public List<Watch> Watches { get; }

        public List<int> Breakpoints { get; }

        public EventHandler OnStop { get; set; }
        public EventHandler OnStart { get; set; }

        public ITrainerRunner Runner { get; private set; }
        public Cpu6800 Emulator { get; private set; }

        public int[] Memory { get; }

        public TrainerSettings Settings { get; set; }
        public Cpu6800State State { get; set; }

        public bool Running
        {
            get { return Runner.Running; }
        }

        public void AddWatch(Watch watch)
        {
            Watches.Add(watch);
        }

        public Trainer()
        {
            Memory = new int[65536];
            Breakpoints = new List<int>();
            Settings = new TrainerSettings();
            Watches = new List<Watch>();
            Settings.SettingsUpdated += (sender, args) =>
            {
                Runner.Recalibrate();
            };

            State = new Cpu6800State();

            Emulator = new Cpu6800
            {
                State = State,

                ReadMem = address =>
                    {
                        address = address & 0xFFFF;

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
                    },

                WriteMem = (address, value) =>
                    {
                        address = address & 0xFFFF;

                        foreach (var watch in Watches.ToList())
                        {
                            if (watch.EventType == EventType.Write)
                            {
                                //if (watch.Address == address)
                                //{

                                //}

                                watch.Action(new WatchEventArgs() { Address = address });
                            }
                        }

                        if (address >= 0xFC00)
                        {
                            // Prevent writing to ROM-mapped space
                            return;
                        }

                        // For accurate emulation we should probably NOT write to memory mapped addresses
                        Memory[address] = value;

                        // Check if we're writing to memory-mapped display
                        // quick test - just check if we are in C100-C1FF
                        //if ((address & 0xC100) == 0xC100)
                        //{
                        //    var displayNo = (address & 0xF0) >> 4;
                        //    if (displayNo >= 1 && displayNo <= 6)
                        //    {
                        //        // OUTCH flicker hack - assumes original OUTCH routine is intact
                        //        if (Settings.EnableOUTCHHack && ((address & 0x08) == 0x08) && (Emulator.State.PC == 0xFE46))
                        //        {
                        //            // don't write to upper bits if in OUTCH routine
                        //            return;
                        //        }
                        //        _disp.Write(address, value);
                        //    }
                        //}
                    }
            };

            Runner = new StandardRunner(this);
            //Runner = new CycleExactRunner(this);

            // Set keyboard mapped memory 'high'
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
        }

        public void ToggleBreakPoint(int address)
        {
            var index = Breakpoints.IndexOf(address);
            if (index > -1)
            {
                Breakpoints.RemoveAt(index);
            }
            else
            {
                Breakpoints.Add(address);
            }
        }

        public void SetupDisplay(PictureBox target)
        {
            _disp = new SegDisplay(target, this);

        }

        public void Stop()
        {
            Runner.Stop();
            OnStop?.Invoke(this, EventArgs.Empty);
        }

        public void StopExternal()
        {
            OnStop?.Invoke(this, EventArgs.Empty);
        }

        public void NMI()
        {
            Emulator.NMI();
        }

        public void IRQ()
        {
            Emulator.IRQ();
        }

        public void Reset()
        {
            Emulator.Reset();
        }

        public void Start()
        {
            Runner.Start();
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        public void Restart()
        {
            Emulator.Reset();
            Runner.Start();
            OnStart?.Invoke(this, EventArgs.Empty);
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



        public void AddMemoryMap(int startAddress, int endAddress, RangeType rangeType, string description)
        {
            MemoryMaps.Add(new MemoryMap()
            {
                Start = startAddress,
                End = endAddress,
                Type = rangeType,
                Description = description
            });
        }

        public MemoryMap GetMemoryMap(int startAddress)
        {
            return MemoryMaps.FirstOrDefault(map => map.Start <= startAddress && map.End >= startAddress);
        }

        public void RemoveMemoryMap(MemoryMap memoryMap)
        {
            MemoryMaps.Remove(memoryMap);
        }

        //public void RemoveMemoryMap(int startAddress)
        //{
        //    var mmap = MemoryMaps.FirstOrDefault(map => map.Start == startAddress);
        //    if (mmap != null)
        //    {
        //        MemoryMaps.Remove(mmap);
        //    }
        //}

        public List<MemoryMap> MemoryMaps { get; private set; }

        public void LoadMemoryMap(string data)
        {
            var maps = new List<MemoryMap>();

            var lines = data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            char[] buffer = new char[256];

            foreach (var line in lines)
            {
                var ptr = 0;
                var parts = new List<string>();
                var inQuotes = false;
                var bufferPtr = 0;
                var quoteCount = 0;
                while (ptr < line.Length)
                {
                    var chr = line[ptr];
                    if (chr == ',' && !inQuotes)
                    {
                        parts.Add(new string(buffer, 0, bufferPtr));
                        bufferPtr = 0;
                        quoteCount = 0;
                    }
                    else
                    {
                        if (chr == '"')
                        {
                            if (quoteCount == 0)
                            {
                                bufferPtr = 0;
                            }
                            inQuotes = !inQuotes;
                            quoteCount++;
                        }
                        else
                        {
                            buffer[bufferPtr] = chr;
                            bufferPtr++;
                        }
                    }
                    ptr++;
                }
                if (inQuotes) { throw new Exception("Unterminated quote"); }
                parts.Add(new string(buffer, 0, bufferPtr));

                if (parts.Count == 0) continue;
                if (parts.Count == 4)
                {
                    maps.Add(new MemoryMap()
                    {
                        Start = Convert.ToInt32(parts[0], 16),
                        End = Convert.ToInt32(parts[1], 16),
                        Type = parts[2] == "CODE" ? RangeType.Code : RangeType.Data,
                        Description = parts[3],
                    });
                }
                else
                {
                    throw new Exception("Error reading map file");
                }
            }

            MemoryMaps = maps;
        }

        /// <summary>
        /// Loads a rom from a text hex dump
        /// </summary>
        /// <param name="data"></param>
        public int LoadRom(string data)
        {
            var lines = data.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
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


            LoadRom(rom);

            return rom.Length;
        }

        /// <summary>
        /// Loads a rom from a byte array
        /// </summary>
        /// <param name="data"></param>
        public void LoadRom(byte[] data)
        {
            int offset = 65536 - data.Length;
            for (var i = 0; i < data.Length; i++)
            {
                Memory[offset + i] = data[i];
            }
        }

        /// <summary>
        /// Load binary file into RAM area at the specified offset
        /// </summary>
        /// <param name="file"></param>
        /// <param name="offset"></param>
        public void LoadRam(byte[] ram, int offset = 0)
        {
            for (var i = 8; i < ram.Length; i++)
            {
                Memory[offset + i - 8] = ram[i];
            }
        }

        public int LoadS19Obj(string content)
        {
            var reader = new S19Reader(this);
            return reader.Read(content);
        }

        public string GetS19Obj(int address, int length)
        {
            var reader = new S19Reader(this);
            return reader.Write(address, length);
        }

        public int Write(string address, string data)
        {
            var bytes = 0;
            var baseAddr = Convert.ToInt32(address, 16);

            for (var p = 0; p < data.Length; p += 2)
            {
                Memory[baseAddr + p / 2] = Convert.ToInt32(data.Substring(p, 2), 16);
                bytes++;
            }

            return bytes;
        }

        public void Write(int address, byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                Memory[address + i] = data[i];
            }
        }

        public byte[] Read(int address, int length)
        {
            var data = new byte[length];

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)Memory[address + i];
            }

            return data;
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
        public bool BreakpointsEnabled { get; set; }
        public bool AtBreakPoint
        {
            get { return Breakpoints.Contains(State.PC); }
        }

        #region Debugger Methods

        public void Break()
        {
            Runner.Stop();
            IsInBreak = true;
        }

        public void Step()
        {
            Emulator.Execute();
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

        #endregion

        public void Dispose()
        {
            Runner.Stop();
            _disp.Dispose();
        }
    }

}
