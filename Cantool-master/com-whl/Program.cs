using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace sf
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            AllocConsole();
            Console.WriteLine("123");
        }
    }
}