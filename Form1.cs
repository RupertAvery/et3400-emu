using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Sharp6800
{
    public partial class Form1 : Form
    {
        Trainer trainer;

        public Form1()
        {
            InitializeComponent();
        }
              
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
            try
            {
                trainer = new Trainer();
                trainer.SetupDisplay(pictureBox1);
                InitKeys();
                trainer.LoadROM("zenith.rom");
                bInit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error initializing the emulator");
            }
            if(bInit) trainer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            trainer.Quit();
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
                trainer.PressKey(Trainer.Keys.Key0);

            else if (sender == button1 || keyargs.KeyCode == Keys.NumPad1 || keyargs.KeyCode == Keys.D1) // 1, ACCA
                trainer.PressKey(Trainer.Keys.Key1);

            else if (sender == button2 || keyargs.KeyCode == Keys.NumPad2 || keyargs.KeyCode == Keys.D2) // 2
                trainer.PressKey(Trainer.Keys.Key2);

            else if (sender == button3 || keyargs.KeyCode == Keys.NumPad3 || keyargs.KeyCode == Keys.D3) // 3
                trainer.PressKey(Trainer.Keys.Key3);

            else if (sender == button4 || keyargs.KeyCode == Keys.NumPad4 || keyargs.KeyCode == Keys.D4) // 4, INDEX
                trainer.PressKey(Trainer.Keys.Key4);

            else if (sender == button5 || keyargs.KeyCode == Keys.NumPad5 || keyargs.KeyCode == Keys.D5) // 5, CC
                trainer.PressKey(Trainer.Keys.Key5);

            else if (sender == button6 || keyargs.KeyCode == Keys.NumPad6 || keyargs.KeyCode == Keys.D6) // 6
                trainer.PressKey(Trainer.Keys.Key6);

            else if (sender == button7 || keyargs.KeyCode == Keys.NumPad7 || keyargs.KeyCode == Keys.D7) // 7, RTI;
                trainer.PressKey(Trainer.Keys.Key7);

            else if (sender == button8 || keyargs.KeyCode == Keys.NumPad8 || keyargs.KeyCode == Keys.D8) // 8
                trainer.PressKey(Trainer.Keys.Key8);

            else if (sender == button9 || keyargs.KeyCode == Keys.NumPad9 || keyargs.KeyCode == Keys.D9) // 9
                trainer.PressKey(Trainer.Keys.Key9);

            else if (sender == buttonA || keyargs.KeyCode == Keys.A) // A, Auto
                trainer.PressKey(Trainer.Keys.KeyA);

            else if (sender == buttonB || keyargs.KeyCode == Keys.B) // B
                trainer.PressKey(Trainer.Keys.KeyB);

            else if (sender == buttonC || keyargs.KeyCode == Keys.C) // C
                trainer.PressKey(Trainer.Keys.KeyC);

            else if (sender == buttonD || keyargs.KeyCode == Keys.D) // D, Do
                trainer.PressKey(Trainer.Keys.KeyD);

            else if (sender == buttonE || keyargs.KeyCode == Keys.E) // E, Exam
                trainer.PressKey(Trainer.Keys.KeyE);

            else if (sender == buttonF || keyargs.KeyCode == Keys.F) // F
                trainer.PressKey(Trainer.Keys.KeyF);

            else if (sender == buttonReset || keyargs.KeyCode == Keys.Escape) // RESET
                trainer.PressKey(Trainer.Keys.KeyReset);

        }

        private void ReleaseKey(object sender, EventArgs args)
        {
            trainer.ReleaseKey(Trainer.Keys.Key0);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "S19 format files|*.obj;*.s19";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName.Length > 0)
            {
                trainer.Quit();
                trainer.LoadSREC(openFileDialog1.FileName);
                trainer.Start();
                MessageBox.Show("File was loaded successfully into RAM", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void loadROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ROM files|*.rom;*.bin";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName.Length > 0)
            {
                trainer.Quit();
                trainer.LoadROM(openFileDialog1.FileName);
                trainer.Start();
                MessageBox.Show("File was loaded successfully into ROM", "Sharp6800", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

    }
}