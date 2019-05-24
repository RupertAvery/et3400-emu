using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Sharp6800.Common;
using Sharp6800.Debugger;
using Timer = System.Threading.Timer;

namespace Sharp6800.Trainer
{
    public partial class TrainerForm : Form
    {
        private Trainer _trainer;
        private DebuggerView _debuggerView;
        private Timer _updateTimer;

        public TrainerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bind picturebox mouse events and form key events to event handlers
        /// </summary>
        private void InitKeys()
        {
            button0.MouseDown += PressKey;
            button1.MouseDown += PressKey;
            button2.MouseDown += PressKey;
            button3.MouseDown += PressKey;
            button4.MouseDown += PressKey;
            button5.MouseDown += PressKey;
            button6.MouseDown += PressKey;
            button7.MouseDown += PressKey;
            button8.MouseDown += PressKey;
            button9.MouseDown += PressKey;
            buttonA.MouseDown += PressKey;
            buttonB.MouseDown += PressKey;
            buttonC.MouseDown += PressKey;
            buttonD.MouseDown += PressKey;
            buttonE.MouseDown += PressKey;
            buttonF.MouseDown += PressKey;
            buttonReset.MouseDown += PressKey;

            this.KeyDown += PressKey;

            button0.MouseUp += ReleaseKey;
            button1.MouseUp += ReleaseKey;
            button2.MouseUp += ReleaseKey;
            button3.MouseUp += ReleaseKey;
            button4.MouseUp += ReleaseKey;
            button5.MouseUp += ReleaseKey;
            button6.MouseUp += ReleaseKey;
            button7.MouseUp += ReleaseKey;
            button8.MouseUp += ReleaseKey;
            button9.MouseUp += ReleaseKey;
            buttonA.MouseUp += ReleaseKey;
            buttonB.MouseUp += ReleaseKey;
            buttonC.MouseUp += ReleaseKey;
            buttonD.MouseUp += ReleaseKey;
            buttonE.MouseUp += ReleaseKey;
            buttonF.MouseUp += ReleaseKey;
            buttonReset.MouseUp += ReleaseKey;

            this.KeyUp += ReleaseKey;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            InitKeys();
            UpdateState(false);

            try
            {
                _trainer = new Trainer();
                _trainer.SetupDisplay(SegmentPictureBox);
                _trainer.LoadRom(ResourceHelper.GetEmbeddedResource(typeof(Trainer).Assembly, "ROM/ROM.HEX"));
                _trainer.LoadMemoryMap(ResourceHelper.GetEmbeddedResource(typeof(Trainer).Assembly, "ROM/ROM.map"));
                _trainer.OnStop += (o, args) => UpdateState(false);
                _trainer.OnStart += (o, args) => UpdateState(true);
                _trainer.BreakpointsEnabled = true;

#if DEBUG
                Action<int> updateSpeed = delegate (int second)
                { this.Text = string.Format("ET-3400 ({0:0}%)", ((float)second / (float)_trainer.DefaultClockSpeed) * 100); };

                _trainer.Runner.OnTimer += second => Invoke(updateSpeed, second);
#endif

#if !DEBUG
                //settingsToolStripMenuItem.Visible = false;
#endif

                // delay emulation to ensure that the form is completely visible, otherwise 
                // some segments will not be lit
                var timer = new System.Timers.Timer() { Interval = 100 };
                timer.Elapsed += timer_Elapsed;
                this.Shown += (o, args) => timer.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error initializing the emulator");
            }

        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = (System.Timers.Timer)sender;
            timer.Stop();
            timer.Elapsed -= timer_Elapsed;
            _trainer.Restart();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trainer.Runner.Quit();
        }

        private void PressKey(object sender, EventArgs args)
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

            else if (keyargs.KeyCode == Keys.F5 && _trainer.IsInBreak)
                _trainer.Runner.Continue();

            else if (keyargs.KeyCode == Keys.F10 && _trainer.IsInBreak)
                _trainer.StepOver();

            else if (keyargs.KeyCode == Keys.F11 && _trainer.IsInBreak)
                if (keyargs.Shift)
                    _trainer.StepOutOf();
                else
                    _trainer.StepInto();

        }

        private void ReleaseKey(object sender, EventArgs args)
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadRam(string file)
        {
            try
            {
                var bytes = 0;
                if (file.ToLower().EndsWith(".ram"))
                {
                    byte[] ram = File.ReadAllBytes(file);
                    _trainer.LoadRam(ram, 0);
                    bytes = ram.Length;
                }
                else
                {
                    var content = File.ReadAllText(file);
                    bytes = _trainer.LoadS19Obj(content);
                }

                MessageBox.Show($"Loaded {bytes} bytes", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while loading the OBJ file " + file + ".", ex);
            }
        }

        private void LoadRAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.Filter = "S19 format files|*.obj;*.s19|All files|*.*";
                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _trainer.Stop();
                    try
                    {
                        LoadRam(openFileDialog.FileName);
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


        private void LoadROM(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            if (ext == ".rom")
            {
                byte[] rom = File.ReadAllBytes(path);
                _trainer.LoadRom(rom);
            }
            else if (ext == ".hex")
            {
                var content = File.ReadAllText(path);
                _trainer.LoadRom(content);
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

        private void loadROMToolStripMenuItem_Click(object sender, EventArgs e)
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
                        LoadROM(openFileDialog.FileName);

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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void breakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Break();
        }

        private void debuggerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_debuggerView == null)
            {
                _debuggerView = new DebuggerView(_trainer);

                _debuggerView.Closing += (o, args) =>
                    {
                        //lock (_lockObject)
                        //{
                        _updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        _updateTimer.Dispose();
                        _updateTimer = null;
                        _debuggerView = null;
                        //}
                    };

                _updateTimer = new Timer(state =>
                {
                    //lock (_lockObject)
                    //{
                    if (_debuggerView != null)
                    {
                        _debuggerView.UpdateDisplay();
                    }
                    //}
                }, null, 0, 100);

                _debuggerView.Show();
            }
            else
            {
                _debuggerView.BringToFront();
            }
        }

        private void NMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.NMI();
        }

        private void IRQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.IRQ();
        }

        private void settingsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var settings = new SettingsForm { Settings = _trainer.Settings };
            settings.Show();
        }

        private void ResetROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.Stop();
            _trainer.LoadRom(ResourceHelper.GetEmbeddedResource(typeof(Trainer).Assembly, "ROM/ROM.HEX"));
            _trainer.Restart();
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

        private void UpdateState(bool isRunning)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action<bool>)UpdateState, new object[] { isRunning });
            }
            else
            {
                stopToolStripMenuItem.Enabled = isRunning;
                resetToolStripMenuItem.Enabled = isRunning;
                stepToolStripMenuItem.Enabled = !isRunning;
                startToolStripMenuItem.Enabled = !isRunning;
            }
        }

        private void SaveRAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addressDialog = new Address();
            addressDialog.ShowDialog();
            if (addressDialog.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            saveFileDialog.Filter = "S19 format files|*.obj;*.s19|All files|*.*";

            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _trainer.Stop();
                try
                {
                    var file = saveFileDialog.FileName;
                    var data = _trainer.GetS19Obj(addressDialog.StartAddress, addressDialog.EndAddress - addressDialog.StartAddress);
                    File.WriteAllText(file, data);
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
            _debuggerView?.UpdateDebuggerState();
        }

        private void TrainerForm_Activated(object sender, EventArgs e)
        {
            //    if (_debuggerView != null && !_debuggerView.IsDisposed)
            //    {
            //        _debuggerView.BringToFront();
            //    }

            //    Focus();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {

        }

        private void HaltOnBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.BreakpointsEnabled = haltOnBreakpointsToolStripMenuItem.Checked;
        }
    }
}