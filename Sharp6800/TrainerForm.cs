using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Sharp6800.Common;
using Sharp6800.Debugger;
using Timer = System.Threading.Timer;

namespace Sharp6800.Trainer
{
    public partial class TrainerForm : Form
    {
        enum Mode
        {
            ET3400,
            ETA3400
        }

        private RecentFilesCollection _recentFiles;
        private Timer timer;
        private Trainer _trainer;
        private TrainerSettings _trainerSettings;
        private DebuggerView _debuggerView;
        private Mode _emulationMode = Mode.ET3400;
        private bool isFirstLoad = true;
        private SettingsForm _settingsForm;

        public TrainerForm()
        {
            InitializeComponent();
        }

        private void TrainerForm_Load(object sender, EventArgs e)
        {
            InitKeys();
            InitTrainer();
            LoadDefaultMemoryMaps();
#if !DEBUG
            sendTerminalToolStripMenuItem.Visible = false;
            modeToolStripMenuItem.Visible = false;
#endif

            _recentFiles = new RecentFilesCollection(RecentToolStripMenuItem, GetAppFolderFile("recent.ini"), 10, LoadRam);
        }

        #region Initialization
        /// <summary>
        /// Bind picturebox mouse events and form key events to event handlers
        /// </summary>
        private void InitKeys()
        {
            button0.MouseDown += OnPressKey;
            button1.MouseDown += OnPressKey;
            button2.MouseDown += OnPressKey;
            button3.MouseDown += OnPressKey;
            button4.MouseDown += OnPressKey;
            button5.MouseDown += OnPressKey;
            button6.MouseDown += OnPressKey;
            button7.MouseDown += OnPressKey;
            button8.MouseDown += OnPressKey;
            button9.MouseDown += OnPressKey;
            buttonA.MouseDown += OnPressKey;
            buttonB.MouseDown += OnPressKey;
            buttonC.MouseDown += OnPressKey;
            buttonD.MouseDown += OnPressKey;
            buttonE.MouseDown += OnPressKey;
            buttonF.MouseDown += OnPressKey;
            buttonReset.MouseDown += OnPressKey;

            this.KeyDown += OnPressKey;

            button0.MouseUp += OnReleaseKey;
            button1.MouseUp += OnReleaseKey;
            button2.MouseUp += OnReleaseKey;
            button3.MouseUp += OnReleaseKey;
            button4.MouseUp += OnReleaseKey;
            button5.MouseUp += OnReleaseKey;
            button6.MouseUp += OnReleaseKey;
            button7.MouseUp += OnReleaseKey;
            button8.MouseUp += OnReleaseKey;
            button9.MouseUp += OnReleaseKey;
            buttonA.MouseUp += OnReleaseKey;
            buttonB.MouseUp += OnReleaseKey;
            buttonC.MouseUp += OnReleaseKey;
            buttonD.MouseUp += OnReleaseKey;
            buttonE.MouseUp += OnReleaseKey;
            buttonF.MouseUp += OnReleaseKey;
            buttonReset.MouseUp += OnReleaseKey;

            this.KeyUp += OnReleaseKey;
        }

        private string GetAppFolderFile(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sharp6800",
                filename);
        }

        private string _settingsPath = "Sharp6800.ini";

        private void InitTrainer()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    _trainerSettings = TrainerSettings.Load(_settingsPath);
                }
                else
                {
                    _trainerSettings = new TrainerSettings();
                }
                _trainer = new Trainer(_trainerSettings);
                _trainer.SetupDisplay(SegmentPictureBox);
                _trainer.OnStart += OnStart;
                _trainer.OnStop += OnStop;
                _trainer.BreakpointsEnabled = true;

                this.Resize += OnResize;
                this.Closing += OnClosing;
#if DEBUG

                _trainer.Runner.OnTimer += RunnerOnTimer;
#endif

                UpdateState();

                // Delay drawing to the form before has loaded as it causes graphical glitches
                //timer = new Timer(StartDelay, null, TimeSpan.FromMilliseconds(500), Timeout.InfiniteTimeSpan);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error initializing the emulator");
            }
        }

        private void RunnerOnTimer(int cyclesPerSecond)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke((Action<int>)RunnerOnTimer, new object[] { cyclesPerSecond });
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                Text = string.Format("ET-3400 ({0:0}%)", ((float)cyclesPerSecond / (float)_trainer.Settings.BaseFrequency) * 100);
            }
        }

        #endregion

        #region Event Handlers

        //private void StartDelay(object obj)
        //{
        //    LoadDefaultRom();
        //    UpdateState();
        //}

        private void OnStart(object sender, EventArgs eventArgs)
        {
            UpdateState();
        }


        private void OnStop(object sender, EventArgs eventArgs)
        {
            UpdateState();
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (_debuggerView != null && !_debuggerView.IsDisposed)
                {
                    _debuggerView.WindowState = FormWindowState.Minimized;
                }
            }
            else
            if (WindowState == FormWindowState.Normal)
            {
                if (_debuggerView != null && !_debuggerView.IsDisposed)
                {
                    _debuggerView.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {

            if (_settingsForm != null && !_settingsForm.IsDisposed)
            {
                _settingsForm.Close();
                _settingsForm.Dispose();
            }

            if (_debuggerView != null && !_debuggerView.IsDisposed)
            {
                _debuggerView.Close();
                _debuggerView.Dispose();
            }

            _trainer.Runner.OnTimer -= RunnerOnTimer;
            _trainer.OnStart -= OnStart;
            _trainer.OnStop -= OnStop;
            _trainer.Dispose();

            button0.MouseDown -= OnPressKey;
            button1.MouseDown -= OnPressKey;
            button2.MouseDown -= OnPressKey;
            button3.MouseDown -= OnPressKey;
            button4.MouseDown -= OnPressKey;
            button5.MouseDown -= OnPressKey;
            button6.MouseDown -= OnPressKey;
            button7.MouseDown -= OnPressKey;
            button8.MouseDown -= OnPressKey;
            button9.MouseDown -= OnPressKey;
            buttonA.MouseDown -= OnPressKey;
            buttonB.MouseDown -= OnPressKey;
            buttonC.MouseDown -= OnPressKey;
            buttonD.MouseDown -= OnPressKey;
            buttonE.MouseDown -= OnPressKey;
            buttonF.MouseDown -= OnPressKey;
            buttonReset.MouseDown -= OnPressKey;

            this.KeyDown -= OnPressKey;

            button0.MouseUp -= OnReleaseKey;
            button1.MouseUp -= OnReleaseKey;
            button2.MouseUp -= OnReleaseKey;
            button3.MouseUp -= OnReleaseKey;
            button4.MouseUp -= OnReleaseKey;
            button5.MouseUp -= OnReleaseKey;
            button6.MouseUp -= OnReleaseKey;
            button7.MouseUp -= OnReleaseKey;
            button8.MouseUp -= OnReleaseKey;
            button9.MouseUp -= OnReleaseKey;
            buttonA.MouseUp -= OnReleaseKey;
            buttonB.MouseUp -= OnReleaseKey;
            buttonC.MouseUp -= OnReleaseKey;
            buttonD.MouseUp -= OnReleaseKey;
            buttonE.MouseUp -= OnReleaseKey;
            buttonF.MouseUp -= OnReleaseKey;
            buttonReset.MouseUp -= OnReleaseKey;

            this.KeyUp -= OnReleaseKey;

            TrainerSettings.Save(_trainerSettings, _settingsPath);

            Thread.Sleep(200);
        }


        private void OnPressKey(object sender, EventArgs args)
        {
            var keyargs = new KeyEventArgs(Keys.None);

            // We use this event handler for both PictureBox and Form events, so check who raised the call
            if (sender == this)
            {
                // this is a Form event, so override the keyargs variable with the actual data
                keyargs = (KeyEventArgs)args;
            }

            // Pull the appropriate bit at mem location LOW to simulate a trainer keydown
            if (sender == button0 || keyargs.KeyCode == Keys.NumPad0 || keyargs.KeyCode == Keys.D0)
                _trainer.PressKey(TrainerKeys.Key0);

            else if (sender == button1 || keyargs.KeyCode == Keys.NumPad1 || keyargs.KeyCode == Keys.D1) // 1, ACCA
                _trainer.PressKey(TrainerKeys.Key1);

            else if (sender == button2 || keyargs.KeyCode == Keys.NumPad2 || keyargs.KeyCode == Keys.D2) // 2
                _trainer.PressKey(TrainerKeys.Key2);

            else if (sender == button3 || keyargs.KeyCode == Keys.NumPad3 || keyargs.KeyCode == Keys.D3) // 3
                _trainer.PressKey(TrainerKeys.Key3);

            else if (sender == button4 || keyargs.KeyCode == Keys.NumPad4 || keyargs.KeyCode == Keys.D4) // 4, INDEX
                _trainer.PressKey(TrainerKeys.Key4);

            else if (sender == button5 || keyargs.KeyCode == Keys.NumPad5 || keyargs.KeyCode == Keys.D5) // 5, CC
                _trainer.PressKey(TrainerKeys.Key5);

            else if (sender == button6 || keyargs.KeyCode == Keys.NumPad6 || keyargs.KeyCode == Keys.D6) // 6
                _trainer.PressKey(TrainerKeys.Key6);

            else if (sender == button7 || keyargs.KeyCode == Keys.NumPad7 || keyargs.KeyCode == Keys.D7) // 7, RTI;
                _trainer.PressKey(TrainerKeys.Key7);

            else if (sender == button8 || keyargs.KeyCode == Keys.NumPad8 || keyargs.KeyCode == Keys.D8) // 8
                _trainer.PressKey(TrainerKeys.Key8);

            else if (sender == button9 || keyargs.KeyCode == Keys.NumPad9 || keyargs.KeyCode == Keys.D9) // 9
                _trainer.PressKey(TrainerKeys.Key9);

            else if (sender == buttonA || keyargs.KeyCode == Keys.A) // A, Auto
                _trainer.PressKey(TrainerKeys.KeyA);

            else if (sender == buttonB || keyargs.KeyCode == Keys.B) // B
                _trainer.PressKey(TrainerKeys.KeyB);

            else if (sender == buttonC || keyargs.KeyCode == Keys.C) // C
                _trainer.PressKey(TrainerKeys.KeyC);

            else if (sender == buttonD || keyargs.KeyCode == Keys.D) // D, Do
                _trainer.PressKey(TrainerKeys.KeyD);

            else if (sender == buttonE || keyargs.KeyCode == Keys.E) // E, Exam
                _trainer.PressKey(TrainerKeys.KeyE);

            else if (sender == buttonF || keyargs.KeyCode == Keys.F) // F
                _trainer.PressKey(TrainerKeys.KeyF);

            else if (sender == buttonReset || keyargs.KeyCode == Keys.Escape) // RESET
                _trainer.PressKey(TrainerKeys.KeyReset);

            //else if (keyargs.KeyCode == Keys.F4 && !_trainer.IsRunning)
            //    _trainer.Runner.Start();

            //else if (keyargs.KeyCode == Keys.F5 && !_trainer.IsRunning)
            //    _trainer.Runner.Start();

            //else if (keyargs.KeyCode == Keys.F10 && !_trainer.IsRunning)
            //    _trainer.StepOver();

            //else if (keyargs.KeyCode == Keys.F11 && _trainer.IsInBreak)
            //    if (keyargs.Shift)
            //        _trainer.StepOutOf();
            //    else
            //        _trainer.StepInto();

        }

        private void OnReleaseKey(object sender, EventArgs args)
        {
            var keyargs = new KeyEventArgs(Keys.None);

            // We use this event handler for both PictureBox and Form events, so check who raised the call
            if (sender == this)
            {
                // this is a Form event, so override the keyargs variable with the actual data
                keyargs = (KeyEventArgs)args;
            }

            if (sender == buttonReset || keyargs.KeyCode == Keys.Escape)
                _trainer.ReleaseKey(TrainerKeys.KeyReset);
            else
                _trainer.ReleaseKey(TrainerKeys.Key0);
        }

        #endregion

        #region ToolStripMenuItem Event Handlers

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadRAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "SREC format files|*.obj;*.s19|All files|*.*";
            openFileDialog.FileName = "";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadRam(openFileDialog.FileName);
            }
        }

        private void LoadRam(string fileName)
        {
            try
            {
                _trainer.Stop(false);
                try
                {
                    var bytes = LoadRamInternal(fileName);

                    var mapFile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".map");
                    if (File.Exists(mapFile))
                    {
                        var region = _trainer.MemoryMapManager.GetRegion("RAM");
                        region.MemoryMapCollection.Clear();
                        region.MemoryMapCollection.AddRange(MemoryMapCollection.Load(mapFile));
                    }
                    _recentFiles.AddItem(fileName);
                    MessageBox.Show(this, $"Loaded {bytes} bytes", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    _trainer.Restart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while loading file {Path.GetFileName(fileName)}\n\n{ex.Message}", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.Filter = "ROM files|*.rom;*.bin;*.hex|All files|*.*";
                openFileDialog.FileName = "";
                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _trainer.Stop();
                    try
                    {
                        LoadROMInternal(openFileDialog.FileName);

                    }
                    finally
                    {
                        _trainer.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file {Path.GetFileName(openFileDialog.FileName)}\n\n{ex.Message}", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void DebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDebugger();
        }

        private void NMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.NMI();
        }

        private void IRQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.IRQ();
        }

        private void SettingsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (_settingsForm == null || _settingsForm.IsDisposed)
            {
                _settingsForm = new SettingsForm(_trainer.Settings);
                _settingsForm.Show();
            }
            else
            {
                _settingsForm.BringToFront();
            }
        }

        private void ResetROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("This will load the default ROM. Are you sure you want to continue?",
                "Reset ROM", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                LoadDefaultRom();
            }
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Start();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Stop();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Reset();
        }

        private void SaveRAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addressDialog = new Address();
            addressDialog.ShowDialog();
            if (addressDialog.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            saveFileDialog.Filter = "SREC format files|*.obj;*.s19|All files|*.*";
            saveFileDialog.FileName = "";

            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _trainer.Stop();
                try
                {
                    var data = _trainer.ReadMemory(addressDialog.StartAddress, addressDialog.EndAddress - addressDialog.StartAddress);
                    var blocks = SrecHelper.ToSrecBlocks(addressDialog.StartAddress, data);

                    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        using (var writer = new SrecWriter(stream))
                        {
                            writer.WriteAll(blocks);
                        }
                    }
                }
                finally
                {
                    _trainer.Restart();
                }
            }
        }

        private void StepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Step();
        }


        private void TrainerForm_Activated(object sender, EventArgs e)
        {
            if (isFirstLoad)
            {
                LoadDefaultRom();
                UpdateState();
                isFirstLoad = false;
            }

            //    if (_debuggerView != null && !_debuggerView.IsDisposed)
            //    {
            //        _debuggerView.BringToFront();
            //    }

            //    Focus();
        }

        private void BreakpointsEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.BreakpointsEnabled = breakpointsEnabledToolStripMenuItem.Checked;
        }

        #endregion

        #region Private methods


        private void ShowDebugger()
        {
            if (_debuggerView == null || _debuggerView.IsDisposed)
            {
                _debuggerView = new DebuggerView(_trainer);
                _debuggerView.Load += (o, args) =>
                {
                    _debuggerView.Top = this.Top;
                    if (this.Left + this.Width + _debuggerView.Width > Screen.FromControl(this).Bounds.Width)
                    {
                        _debuggerView.Left = this.Left - _debuggerView.Width;
                    }
                    else
                    {
                        _debuggerView.Left = this.Left + this.Width;
                    }
                };

                _debuggerView.Show();
            }
            else
            {
                _debuggerView.BringToFront();
            }
        }

        private void UpdateState()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)UpdateState, null);
            }
            else
            {
                var isRunning = _trainer != null && _trainer.IsRunning;
                StatusToolStripStatusLabel.Text = isRunning ? "Running" : "Stopped";
                stopToolStripMenuItem.Enabled = isRunning;
                resetToolStripMenuItem.Enabled = isRunning;
                stepToolStripMenuItem.Enabled = !isRunning;
                startToolStripMenuItem.Enabled = !isRunning;
                clearRAMToolStripMenuItem.Enabled = !isRunning;
            }
        }


        private int LoadRamInternal(string file)
        {
            var bytes = 0;

            var extension = Path.GetExtension(file).ToLower();

            switch (extension)
            {
                case ".obj":
                case ".s19":
                    using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new SrecReader(stream))
                        {
                            var records = reader.ReadAll();
                            foreach (var srecBlock in records)
                            {
                                _trainer.WriteMemory(srecBlock.Address, srecBlock.Data, srecBlock.Data.Length);
                                bytes += srecBlock.Data.Length;
                            }
                        }
                    }

                    break;

                default:
                    byte[] ram = File.ReadAllBytes(file);
                    _trainer.WriteMemory(0, ram, ram.Length);
                    bytes = ram.Length;
                    break;
            }

            _trainer.MemoryMapManager.GetRegion("RAM").MemoryMapCollection.Clear();

            return bytes;
        }

        private void LoadROMInternal(string path)
        {
            var ext = Path.GetExtension(path).ToLower();

            if (ext == ".rom")
            {
                byte[] rom = File.ReadAllBytes(path);
                _trainer.WriteMemory(Trainer.RomAddress, rom, rom.Length);
            }
            else if (ext == ".hex")
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var romLoader = new RomReader(stream);
                    var data = romLoader.Read();
                    _trainer.WriteMemory(Trainer.RomAddress, data, data.Length);
                }
            }

            //var datPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".dat");

            //if (File.Exists(datPath))
            //{
            //    using (var datfile = new FileStream(datPath, FileMode.Open, FileAccess.Read))
            //    {
            //        var dat = new XmlSerializer(typeof(MemoryMap), new XmlRootAttribute("memorymap"));
            //        var map = (MemoryMap)dat.Deserialize(datfile);
            //        memoryMaps.Add(map);
            //    }
            //}
        }

        private void LoadRomFromResource(string key, int address, int length)
        {
            using (var stream = ResourceHelper.GetEmbeddedResourceStream(typeof(Trainer).Assembly, key))
            {
                var buffer = new byte[length];
                stream.Read(buffer, 0, length);
                _trainer.WriteMemory(address, buffer, buffer.Length);
            }
        }

        private void LoadMapFromResource(string key, int start, int length)
        {
            using (var stream = ResourceHelper.GetEmbeddedResourceStream(typeof(Trainer).Assembly, key))
            {
                _trainer.MemoryMapManager.RemoveRegionByName(key);
                var region = new MemoryMapRegion(_trainer.MemoryMapEventBus) { Name = key, Start = start, End = start + length };
                _trainer.MemoryMapManager.AddRegion(region);
                region.MemoryMapCollection.AddRange(MemoryMapCollection.Load(stream));
            }
        }

        private void LoadDefaultMemoryMaps()
        {
            var region = new MemoryMapRegion(_trainer.MemoryMapEventBus) { Name = "RAM", Start = 0x0, End = 0xFFF };
            _trainer.MemoryMapManager.AddRegion(region);
            LoadMapFromResource("ROM/Monitor.map", Trainer.RomAddress, 1024);
        }

        private void LoadDefaultRom()
        {
            _trainer.Stop(false);

            LoadRomFromResource("ROM/MonitorKeyHack.bin", Trainer.RomAddress, 1024);

            if (_emulationMode == Mode.ET3400)
            {
                _trainer.Settings.BaseFrequency = 100_000; // 100kHz
                //_trainer.Settings.ClockSpeed = 100_000; 
                //_trainer.Settings.CpuPercent = 100; 
                var buffer = new byte[2048];
                // Clear ROMs
                _trainer.WriteMemory(0x1400, buffer, buffer.Length);
                _trainer.WriteMemory(0x1C00, buffer, buffer.Length);
                _trainer.MemoryMapManager.RemoveRegionByName("ROM/FantomII.map");
            }
            else
            {
                _trainer.Settings.BaseFrequency = 1_000_000; // 1MHz
                //_trainer.Settings.ClockSpeed = 1_000_000; 
                //_trainer.Settings.CpuPercent = 100; 
                LoadRomFromResource("ROM/FantomII.bin", 0x1400, 2048);
                LoadRomFromResource("ROM/TinyBasic.bin", 0x1C00, 2048);
                LoadMapFromResource("ROM/FantomII.map", 0x1400, 2048);
            }



            _trainer.Restart();
        }

        #endregion

        private void ClearRecentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _recentFiles.Clear();
            _recentFiles.Save();
        }

        private void HardResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Stop(false);
            var empty = new byte[0x1000];
            _trainer.WriteMemory(0x0000, empty, empty.Length);
            _trainer.Restart();
        }

        private void sendTerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.SendTerminal("G 1C00\r");
        }

        private void ModeET3400ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModeETA3400ToolStripMenuItem.Checked = false;
            ModeET3400ToolStripMenuItem.Checked = true;
            _emulationMode = Mode.ET3400;
            LoadDefaultRom();
        }

        private void ModeETA3400ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModeET3400ToolStripMenuItem.Checked = false;
            ModeETA3400ToolStripMenuItem.Checked = true;
            _emulationMode = Mode.ETA3400;
            LoadDefaultRom();
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Memory Map Files|*.map";
            openFileDialog.FileName = "";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var region = _trainer.MemoryMapManager.GetRegion("RAM");
                region.MemoryMapCollection.Clear();
                region.MemoryMapCollection.AddRange(MemoryMapCollection.Load(openFileDialog.FileName));
            }
        }

        private void saveMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Memory Map Files|*.map";
            saveFileDialog.FileName = "";
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var region = _trainer.MemoryMapManager.GetRegion("RAM");

                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    MemoryMapCollection.Save(stream, region.MemoryMapCollection);
                }
            }
        }

        private void clearRAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var empty = new byte[4096];
            _trainer.WriteMemory(0x0000, empty, empty.Length);
        }
    }
}