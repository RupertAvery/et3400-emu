using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core6800;
using ET3400.Common;
using ET3400.Debugger;
using ET3400.Debugger.Breakpoints;
using ET3400.Debugger.MemoryMaps;
using ET3400.Threading;

namespace ET3400.Trainer
{
    /// <summary>
    /// Implementation of a ET-3400 Trainer simulation. Wraps the core emulator in the trainer hardware (keys + display) 
    /// </summary>
    public class Trainer : ITrainer, IDisposable
    {
        private LedDisplay _display;

        public const int RomAddress = 0xFC00;

        public List<Watch> Watches { get; }
        public BreakpointCollection Breakpoints { get; }

        public EventHandler OnStop { get; set; }
        public EventHandler OnStart { get; set; }
        public EventHandler OnStep { get; set; }

        public MemoryMapManager MemoryMapManager { get; private set; }
        public MemoryMapEventBus MemoryMapEventBus { get; private set; }
        public ITrainerRunner Runner { get; private set; }
        public Cpu6800 Emulator { get; private set; }
        public int[] Memory { get; }
        public ET3400Settings Settings { get; set; }
        public Cpu6800State State { get; set; }

        public bool IsRunning
        {
            get { return Runner.Running; }
        }

        public void AddWatch(Watch watch)
        {
            Watches.Add(watch);
        }

        private MC6820 _mc6820;

        private DebugConsoleAdapter _debugConsoleAdapter;

        private void OnPeripheralWrite(object sender, PeripheralEventArgs e)
        {
            _debugConsoleAdapter.Read(e.Value & 1);
        }

        private void OnPeripheralRead(object sender, PeripheralEventArgs e)
        {
            e.Value = _debugConsoleAdapter.Write();
        }


        public Trainer(ET3400Settings settings, PictureBox displayTarget)
        {
            Memory = new int[65536];
            Breakpoints = new BreakpointCollection();
            Settings = settings;
            Watches = new List<Watch>();
            MemoryMapManager = new MemoryMapManager();
            MemoryMapEventBus = new MemoryMapEventBus();


            _mc6820 = new MC6820();
            _debugConsoleAdapter = new DebugConsoleAdapter();
            _mc6820.OnPeripheralWrite += OnPeripheralWrite;
            _mc6820.OnPeripheralRead += OnPeripheralRead;

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

                        if (address >= 0x1000 && address <= 0x1003)
                        {
                            //_mc6820.RegisterSelect(address & 3);
                            //return _mc6820.Get();
                            return _mc6820.Get(address & 3);
                        }

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

                        if (address >= 0x1000 && address <= 0x1003)
                        {
                            //_mc6820.RegisterSelect(address & 3);
                            //_mc6820.Set(value);
                            _mc6820.Set(address & 3, value);
                            return;
                        }

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
            };

            //Runner = new StandardRunner(this);
            Runner = new CycleExactRunner(this);

            _display = new LedDisplay(displayTarget, this);
            Runner.OnSleep += OnSleep;

            // Set keyboard mapped memory 'high'
            Memory[0xC003] = 0xFF;
            Memory[0xC005] = 0xFF;
            Memory[0xC006] = 0xFF;
        }

        private void OnSleep(object sender, EventArgs e)
        {
            _display.Redraw();
        }

        public void ToggleBreakPoint(int address)
        {
            var breakpoint = Breakpoints[address];
            var memoryMap = MemoryMapManager.GetMemoryMap(address);
            if (memoryMap != null && memoryMap.Type == RangeType.Data) return;

            if (breakpoint != null)
            {
                Breakpoints.Remove(breakpoint.Address);
            }
            else
            {
                Breakpoints.Add(address);
            }
        }

        public void ToggleBreakPointEnabled(int address)
        {
            var breakpoint = Breakpoints[address];
            if (breakpoint != null)
            {
                Breakpoints.Toggle(breakpoint.Address);
            }
        }

        public void RaiseStopEvent()
        {
            OnStop?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Pulls the Nom-maskable Interrupt line low
        /// </summary>
        public void NMI()
        {
            Emulator.NMI();
        }

        /// <summary>
        /// Pulls the Interrupt Request line low
        /// </summary>
        public void IRQ()
        {
            Emulator.IRQ();
        }

        /// <summary>
        /// Resets the CPU, causing execution to start from the 16-bit address stored at $FFFE
        /// </summary>
        public void Reset()
        {
            Emulator.Reset();
        }

        /// <summary>
        /// Resumes execution
        /// </summary>
        public void Start(bool raiseEvent = true)
        {
            Runner.Start();
            if (raiseEvent)
            {
                OnStart?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Halts execution
        /// </summary>
        /// <param name="raiseEvent"></param>
        public void Stop(bool raiseEvent = true)
        {
            Runner.Stop();
            if (raiseEvent)
            {
                OnStop?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Resets the CPU and resumes execution
        /// </summary>
        public void Restart()
        {
            Reset();
            Start();
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
        /// Writes a byte array directly to the specified memory address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void WriteMemory(int address, byte[] data, int length)
        {
            for (var i = 0; i < length; i++)
            {
                Memory[address + i] = data[i];
            }
        }

        /// <summary>
        /// Writes an int array directly to the specified memory address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void WriteMemory(int address, int[] data, int length)
        {
            for (var i = 0; i < length; i++)
            {
                Memory[address + i] = data[i];
            }
        }

        /// <summary>
        /// Reads an int array from the specified address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public int[] ReadMemory(int address, int length)
        {
            var data = new int[length];

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = Memory[address + i];
            }

            return data;
        }


        /// <summary>
        /// For debugging purposes only
        /// </summary>
        /// <param name="i"></param>
        public void SetProgramCounter(int i)
        {
            State.PC = i;
        }

        public int DefaultClockSpeed
        {
            get { return 100000; }
        }

        /// <summary>
        /// Gets or sets whether execution will stop when a breakpoint is reached
        /// </summary>
        public bool BreakpointsEnabled { get; set; }

        public bool AtBreakPoint
        {
            get
            {
                var breakpoint = Breakpoints[State.PC];
                return breakpoint != null && breakpoint.IsEnabled;
            }
        }

        #region Debugger Methods

        public void Step()
        {
            Emulator.Execute();
            OnStep?.Invoke(this, EventArgs.Empty);
        }

        public void StepOver()
        {
            if (StepInto()) StepOutOf();
            OnStep?.Invoke(this, EventArgs.Empty);
        }

        public void StepOutOf()
        {
            var nextOpCode = Memory[Emulator.State.PC] & 0xFF;
            do
            {
                Emulator.Execute();
            } while (!Disassembler.IsReturn(nextOpCode));
        }

        private bool StepInto()
        {
            var nextOpCode = Memory[Emulator.State.PC] & 0xFF;
            Emulator.Execute();
            return Disassembler.IsSubroutine(nextOpCode);
        }

        #endregion

        public void Dispose()
        {
            _mc6820.OnPeripheralWrite -= OnPeripheralWrite;
            Runner.OnSleep -= OnSleep;
            Runner.Dispose();
            _display.Dispose();
        }

        public void SendTerminal(string value)
        {
            _debugConsoleAdapter.WriteString(value);
        }
    }

}
