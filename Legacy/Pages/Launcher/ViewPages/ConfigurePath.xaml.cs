using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Security.Policy;
using System.Diagnostics;

namespace Arcane_Launcher.Pages.Launcher.ViewPages
{
    /// <summary>
    /// Interaction logic for ConfigurePath.xaml
    /// </summary>
    public partial class ConfigurePath : Page
    {
        public ConfigurePath()
        {
            InitializeComponent();
        }

        private void SavePath_Click(object sender, RoutedEventArgs e)
        {
            string gamePath = PathTextBox.Text.Trim();
            string seasonText = SeasonTextBox.Text;

            if (string.IsNullOrEmpty(gamePath))
            {
                MessageBox.Show("Please enter a valid game path.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else if (string.IsNullOrEmpty(seasonText))
            {
                MessageBox.Show("Please enter a valid season number.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (int.TryParse(seasonText, out int season))
            {
                Utils.Logger.good($"Game Path: {gamePath}");
                Utils.Logger.good($"Season: {season}");

                if (!File.Exists(System.IO.Path.Combine(gamePath, "FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe")))
                {
                    Utils.Logger.warn("Path Invalid, Make sure your path has the \"Engine\" and \"FortniteGame\" Folders inside!");
                    MessageBox.Show("Path Invalid, Make sure your path has the \"Engine\" and \"FortniteGame\" Folders inside!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Properties.Settings.Default.GamePath = gamePath;
                Properties.Settings.Default.Season = season;
                Properties.Settings.Default.Save();

                if (season <= 29 && season >= 5)
                {
                    File.Copy(
                        System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "GFSDK_Aftermath_Lib.x64.dll"),
                        System.IO.Path.Combine(gamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"),
                        overwrite: true
                    );
                }

                RestartApplication();
                /*if (File.Exists(System.IO.Path.Combine(gamePath, "FortniteGame\\Content\\Splash", "Splash.bmp")))
                {
                    DisableUI();
                    File.Copy(
                        System.IO.Path.Combine(gamePath, "FortniteGame\\Content\\Splash", "Splash.bmp"),
                        System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources"), "Splash.bmp"),
                        overwrite: true
                    );
                    EnableUI();
                } else
                {
                    Utils.Logger.warn("No splash image found!");
                }*/
            }
            else
            {
                MessageBox.Show("Please enter a valid season number");
                return;
            }

            Utils.Globals.MainView.ChangeView("Library");
        }

        private static void DisableUI()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var window in Application.Current.Windows)
                {
                    if (window is Window currentWindow)
                    {
                        currentWindow.IsEnabled = false;
                    }
                }
            });
        }

        private static void EnableUI()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var window in Application.Current.Windows)
                {
                    if (window is Window currentWindow)
                    {
                        currentWindow.IsEnabled = true;
                    }
                }
            });
        }

        private static void RestartApplication()
        {
            try
            {
                string exePath = Assembly.GetExecutingAssembly().Location;
                string exeFilePath = System.IO.Path.ChangeExtension(exePath, ".exe");
                Utils.Logger.good(exePath);
                Process.Start(exeFilePath);
                Thread.Sleep(20);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Utils.Logger.error("Error restarting application: " + ex.Message);
            }
        }
    }
}
