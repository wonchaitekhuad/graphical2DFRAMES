using System;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
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
            // Temporary change to test DecimalPointsForm
            Application.Run(new DecimalPointsForm());
            // Original: Application.Run(new Form1());
        }
    }
}
