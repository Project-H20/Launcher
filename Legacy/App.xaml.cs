using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Arcane_Launcher.Utils;

namespace Arcane_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleTitleA(string title);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        private bool IsDebugMode()
        {
            #if DEVELOPER
                return true;
            #else
                return false;
            #endif
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string[] args = Environment.GetCommandLineArgs();
            if (IsDebugMode() || args.Contains("--console"))
            {
                if (GetConsoleWindow() == IntPtr.Zero)
                {
                    AllocConsole();
                    SetConsoleTitleA("Legacy Developer Console.");
                }
                Utils.Logger.Legacy("Developer console started.");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            FreeConsole();
        }
    }

}
