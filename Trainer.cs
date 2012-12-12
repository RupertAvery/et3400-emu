using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Sharp6800
{

    /// <summary>
    /// Implementation of a ET-3400 Trainer simulation
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
        core6800 emu;
        SegDisplay disp;

        public Trainer()
        {
            emu = new core6800();
            // Set keyboard mapped memory 'high'
            emu.Memory[0xC003] = 0xFF;
            emu.Memory[0xC005] = 0xFF;
            emu.Memory[0xC006] = 0xFF;
            emu.Reset();
        }

        public void SetupDisplay(PictureBox target)
        {
            disp = new SegDisplay(target);
        }

        public void Start()
        {
            emu.Interrupt += IntHandler;

            runner = new Thread(new ThreadStart(emu.Run));
            runner.Start();
        }


        public void IntHandler()
        {
            // Update the 7-seg display
            disp.Display(emu.Memory);
        }

        /// <summary>
        /// Sets the quit flag on the emulator and waits for execution of the current opcode to complete
        /// </summary>
        public void Quit()
        {
            emu.Quit();
            // wait for emulation thread to terminate
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
                    emu.Memory[0xC006] &= 0xDF;
                    break;
                case Keys.Key1:// 1, ACCA
                    emu.Memory[0xC006] &= 0xEF;
                    break;
                case Keys.Key2:// 2
                    emu.Memory[0xC005] &= 0xEF;
                    break;
                case Keys.Key3:// 3
                    emu.Memory[0xC003] &= 0xEF;
                    break;
                case Keys.Key4:// 4, INDEX
                    emu.Memory[0xC006] &= 0xF7;
                    break;
                case Keys.Key5:// 5, CC
                    emu.Memory[0xC005] &= 0xF7;
                    break;
                case Keys.Key6:// 6
                    emu.Memory[0xC003] &= 0xF7;
                    break;
                case Keys.Key7:// 7, RTI;
                    emu.Memory[0xC006] &= 0xFB;
                    break;
                case Keys.Key8:// 8
                    emu.Memory[0xC005] &= 0xFB;
                    break;
                case Keys.Key9:// 9
                    emu.Memory[0xC003] &= 0xFB;
                    break;
                case Keys.KeyA:// A, Auto
                    emu.Memory[0xC006] &= 0xFD;
                    break;
                case Keys.KeyB:// B
                    emu.Memory[0xC005] &= 0xFD;
                    break;
                case Keys.KeyC:// C
                    emu.Memory[0xC003] &= 0xFD;
                    break;
                case Keys.KeyD:// D, Do
                    emu.Memory[0xC006] &= 0xFE;
                    break;
                case Keys.KeyE:// E, Exam
                    emu.Memory[0xC005] &= 0xFE;
                    break;
                case Keys.KeyF:// F
                    emu.Memory[0xC003] &= 0xFE;
                    break;
                case Keys.KeyReset:// RESET
                    emu.Reset();
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
            emu.Memory[0xC003] = 0xFF;
            emu.Memory[0xC005] = 0xFF;
            emu.Memory[0xC006] = 0xFF;
        }


        /// <summary>
        /// Loads the specified binary file into the upper end of memory
        /// </summary>
        /// <param name="file"></param>
        public void LoadROM(string file)
        {
            try
            {
                byte[] rom = File.ReadAllBytes(file);
                int offset = 65536 - rom.Length;
                for (var i = 0; i < rom.Length; i++)
                {
                    emu.Memory[offset + i] = rom[i];
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
                    emu.Memory[offset + i - 8] = ram[i];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while loading the ROM file " + file + ".", ex);
            }

        }

    }
}
