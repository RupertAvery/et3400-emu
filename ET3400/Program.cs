using System;
using System.Windows.Forms;

namespace ET3400
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