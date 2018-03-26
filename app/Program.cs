using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace OracleDbCompilerService
{
    static class Program
    {
        const string regLocation = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string appName = "OracleDbCompilerService";
        [STAThread]
        static void Main()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(regLocation, true);
            rkApp.SetValue(appName,  "\"" + Application.ExecutablePath.ToString() + "\" /autostart");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

    }
}
