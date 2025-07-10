using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Arcane_Launcher.Pages.Launcher.ViewPages
{
    /// <summary>
    /// Interaction logic for Library.xaml
    /// </summary>
    public partial class Library : Page
    {
        public Library()
        {
            InitializeComponent();
        }

        private void ChangePathButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.Globals.MainView.ChangeView("ConfigurePath");
        }

        private void LaunchFN_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.GamePath))
            {
                Utils.Globals.MainView.ChangeView("ConfigurePath");
            } else
            {
                Services.LaunchService.InitializeLaunching(Properties.Settings.Default.GamePath, Properties.Settings.Default.Season);
            }
        }
    }
}
