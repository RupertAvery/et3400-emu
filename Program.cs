using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrainerForm());
        }
    }
}