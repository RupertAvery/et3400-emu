using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Core6800;
using Sharp6800.Debugger;
using Sharp6800.Trainer;

namespace Sharp6800
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //{
            //    var emu = new Cpu6800() { ReadMem = i => 0, WriteMem = (i, i1) => { }, State = new Cpu6800State() };
            //    for (int i = 0; i <= 0xff; i++)
            //    {
            //        var state = emu.InterpretOpCode(i);
            //        Debug.Print("{0:X2} {1}", i, state);
            //    }
            //}
            //Disassembler.SelfTest();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrainerForm());
        }
    }
}