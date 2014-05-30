using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Core6800;
using Timer = System.Threading.Timer;

namespace Sharp6800
{
    public partial class Form1 : Form
    {
        private Trainer _trainer;
        private MemoryView _memoryView;
        private DisassemblerView _disassemblerView;
        private Timer _updateTimer;
        private object _lockObject = new object();
        private List<DataRange> romDataRanges;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// bind button picturebox mouse button and form key events to event handlers
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
            bool bInit = false;
            _updateTimer = new Timer(state =>
                {
                    lock (_lockObject)
                    {
                        if (_memoryView != null)
                        {
                            _memoryView.MemDisplay.Display(_trainer.Memory);
                        }

                        if (_disassemblerView != null)
                        {
                            _disassemblerView.DasmDisplay.Display(_trainer.Memory);
                        }
                    }


                }, null, 0, 10);
            InitKeys();

            try
            {
                _trainer = new Trainer();
                _trainer.SetupDisplay(pictureBox1);
                _trainer.LoadROM("ROM.HEX");
                LoadRomDataRanges("ROM.HEX");
                Action<int> updateSpeed = delegate(int second)
                { this.Text = string.Format("ET-3400 ({0:0}%)", ((float)second / (float)_trainer.DefaultClockSpeed) * 100); };

                _trainer.OnTimer += second => Invoke(updateSpeed, second);

                bInit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error initializing the emulator");
            }

            if (bInit) _trainer.Start();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trainer.Quit();
        }

        private void PressKey(object sender, EventArgs args)
        {
            KeyEventArgs keyargs = new KeyEventArgs(Keys.None);

            if (sender == this)
            {
                keyargs = (KeyEventArgs)args;
            }

            // pull appropriate bit at mem location LOW
            if (sender == button0 || keyargs.KeyCode == Keys.NumPad0 || keyargs.KeyCode == Keys.D0)
                _trainer.PressKey(Trainer.Keys.Key0);

            else if (sender == button1 || keyargs.KeyCode == Keys.NumPad1 || keyargs.KeyCode == Keys.D1) // 1, ACCA
                _trainer.PressKey(Trainer.Keys.Key1);

            else if (sender == button2 || keyargs.KeyCode == Keys.NumPad2 || keyargs.KeyCode == Keys.D2) // 2
                _trainer.PressKey(Trainer.Keys.Key2);

            else if (sender == button3 || keyargs.KeyCode == Keys.NumPad3 || keyargs.KeyCode == Keys.D3) // 3
                _trainer.PressKey(Trainer.Keys.Key3);

            else if (sender == button4 || keyargs.KeyCode == Keys.NumPad4 || keyargs.KeyCode == Keys.D4) // 4, INDEX
                _trainer.PressKey(Trainer.Keys.Key4);

            else if (sender == button5 || keyargs.KeyCode == Keys.NumPad5 || keyargs.KeyCode == Keys.D5) // 5, CC
                _trainer.PressKey(Trainer.Keys.Key5);

            else if (sender == button6 || keyargs.KeyCode == Keys.NumPad6 || keyargs.KeyCode == Keys.D6) // 6
                _trainer.PressKey(Trainer.Keys.Key6);

            else if (sender == button7 || keyargs.KeyCode == Keys.NumPad7 || keyargs.KeyCode == Keys.D7) // 7, RTI;
                _trainer.PressKey(Trainer.Keys.Key7);

            else if (sender == button8 || keyargs.KeyCode == Keys.NumPad8 || keyargs.KeyCode == Keys.D8) // 8
                _trainer.PressKey(Trainer.Keys.Key8);

            else if (sender == button9 || keyargs.KeyCode == Keys.NumPad9 || keyargs.KeyCode == Keys.D9) // 9
                _trainer.PressKey(Trainer.Keys.Key9);

            else if (sender == buttonA || keyargs.KeyCode == Keys.A) // A, Auto
                _trainer.PressKey(Trainer.Keys.KeyA);

            else if (sender == buttonB || keyargs.KeyCode == Keys.B) // B
                _trainer.PressKey(Trainer.Keys.KeyB);

            else if (sender == buttonC || keyargs.KeyCode == Keys.C) // C
                _trainer.PressKey(Trainer.Keys.KeyC);

            else if (sender == buttonD || keyargs.KeyCode == Keys.D) // D, Do
                _trainer.PressKey(Trainer.Keys.KeyD);

            else if (sender == buttonE || keyargs.KeyCode == Keys.E) // E, Exam
                _trainer.PressKey(Trainer.Keys.KeyE);

            else if (sender == buttonF || keyargs.KeyCode == Keys.F) // F
                _trainer.PressKey(Trainer.Keys.KeyF);

            else if (sender == buttonReset || keyargs.KeyCode == Keys.Escape) // RESET
                _trainer.PressKey(Trainer.Keys.KeyReset);

        }

        private void ReleaseKey(object sender, EventArgs args)
        {
            _trainer.ReleaseKey(Trainer.Keys.Key0);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "S19 format files|*.obj;*.s19";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _trainer.Quit();
                _trainer.LoadSREC(openFileDialog1.FileName);
                _trainer.Start();
                MessageBox.Show("File was loaded successfully into RAM", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void loadROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ROM files|*.rom;*.bin;*.hex";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _trainer.Quit();
                _trainer.LoadROM(openFileDialog1.FileName);
                LoadRomDataRanges(openFileDialog1.FileName);
                _trainer.Start();
                MessageBox.Show("File was loaded successfully into ROM", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trainer.AddBreakPoint(0xFDBB);
            //trainer.SetProgramCounter(1);
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (_lockObject)
            {
                if (_memoryView == null)
                {
                    _memoryView = new MemoryView();
                    _memoryView.Show();
                    _memoryView.Closing += (o, args) =>
                    {
                        //_trainer.OnUpdate -= UpdateMemDisplay;
                        _memoryView = null;
                    };
                    // _trainer.OnUpdate += UpdateMemDisplay;

                }
            }
        }

        private void disassemblerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (_lockObject)
            {
                if (_disassemblerView == null)
                {
                    _disassemblerView = new DisassemblerView { State = _trainer.State, Memory = _trainer.Memory };
                    _disassemblerView.DasmDisplay.DataRanges = romDataRanges;
                    _disassemblerView.Show();
                    _disassemblerView.Closing += (o, args) =>
                        {
                            //_trainer.OnUpdate -= UpdateDasmDisplay;
                            _disassemblerView = null;
                        };
                    //_trainer.OnUpdate += UpdateDasmDisplay;
                }
            }
        }

        private void LoadRomDataRanges(string path)
        {
            romDataRanges = new List<DataRange>();
            path = Path.GetFileNameWithoutExtension(path) + ".dat";
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                var lines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    if (line.StartsWith(".data"))
                    {
                        var ranges = line.Split(new char[] { ' ' });
                        romDataRanges.Add(new DataRange() { Start = Convert.ToInt32(ranges[1], 16), End = Convert.ToInt32(ranges[2], 16) });
                    }
                }

            }

        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var settings = new SettingsForm { Trainer = _trainer };
            settings.Show();
        }

    }
}