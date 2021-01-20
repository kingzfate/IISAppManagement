using System;

namespace IISAppManagement
{
    /// <summary>
    /// Start point of the program
    /// </summary>
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() => System.Windows.Forms.Application.Run(new Application());
    }
}