using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OracleDbCompilerService
{
    static class Program
    {
        const string regLocation = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string appName = "NexgenCompilerService";
        [STAThread]
        static void Main()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(regLocation, true);
            rkApp.SetValue(appName, Application.ExecutablePath.ToString());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

    }
}
