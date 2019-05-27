using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Sharp6800.Common;
using Sharp6800.Debugger;

namespace Sharp6800.Trainer
{
    public partial class TrainerForm : Form
    {
        private RecentFilesCollection _recentFiles;

        private Trainer _trainer;
        private DebuggerView _debuggerView;

        public TrainerForm()
        {
            InitializeComponent();
        }

        private void TrainerForm_Load(object sender, EventArgs e)
        {
            InitKeys();
            InitTrainer();
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

        private void InitTrainer()
        {
            try
            {
                _trainer = new Trainer();
                _trainer.SetupDisplay(SegmentPictureBox);

                LoadDefaultRom();

                _trainer.OnStart += OnStart;
                _trainer.OnStop += OnStop;
                _trainer.BreakpointsEnabled = true;

                this.Resize += OnResize;
                this.Closing += OnClosing;
#if DEBUG
                Action<int> updateSpeed = delegate (int cyclesPerSecond)
                    { this.Text = string.Format("ET-3400 ({0:0}%)", ((float)cyclesPerSecond / (float)_trainer.DefaultClockSpeed) * 100); };

                _trainer.Runner.OnTimer += second => Invoke(updateSpeed, second);
#endif

                UpdateState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error initializing the emulator");
            }
        }
        #endregion

        #region Event Handlers

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
            var settings = new SettingsForm(_trainer.Settings);
            settings.Show();
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

            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _trainer.Stop();
                try
                {
                    var file = saveFileDialog.FileName;
                    var data = _trainer.ReadMemory(addressDialog.StartAddress, addressDialog.EndAddress - addressDialog.StartAddress);
                    var blocks = SrecHelper.ToSrecBlocks(addressDialog.StartAddress, data);

                    using (var stream = new FileStream(file, FileMode.Truncate, FileAccess.Write))
                    {
                        var writer = new SrecWriter(stream);
                        writer.Write(blocks);
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
                var state = _trainer.IsRunning;
                StatusToolStripStatusLabel.Text = state ? "Running" : "Stopped";
                stopToolStripMenuItem.Enabled = state;
                resetToolStripMenuItem.Enabled = state;
                stepToolStripMenuItem.Enabled = !state;
                startToolStripMenuItem.Enabled = !state;
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
                        var reader = new SrecReader(stream);
                        var records = reader.Read();
                        foreach (var srecBlock in records)
                        {
                            _trainer.WriteMemory(srecBlock.Address, srecBlock.Data, srecBlock.Data.Length);
                            bytes += srecBlock.Data.Length;
                        }
                    }

                    break;

                default:
                    byte[] ram = File.ReadAllBytes(file);
                    _trainer.WriteMemory(0, ram, ram.Length);
                    bytes = ram.Length;
                    break;
            }

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

        private void LoadDefaultRom()
        {
            _trainer.Stop(false);

            using (var stream = ResourceHelper.GetEmbeddedResourceStream(typeof(Trainer).Assembly, "ROM/ROM.HEX"))
            {
                var romLoader = new RomReader(stream);
                var data = romLoader.Read();
                _trainer.WriteMemory(Trainer.RomAddress, data, data.Length);
            }

            using (var stream = ResourceHelper.GetEmbeddedResourceStream(typeof(Trainer).Assembly, "ROM/ROM.map"))
            {
                _trainer.MemoryMaps = MemoryMapCollection.Load(stream);
            }

            _trainer.Restart();
        }

        #endregion

        private void ClearRecentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _recentFiles.Clear();
            _recentFiles.Save();
        }
    }
}