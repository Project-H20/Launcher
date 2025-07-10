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
using Arcane_Launcher.Utils;
using Wpf.Ui.Controls;

namespace Arcane_Launcher.Pages.Launcher
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();
            Utils.Globals.MainView = this;
            Utils.Logger.good($"displayName: {Properties.Settings.Default.displayName}");
            ProfileFlyout.Content = Properties.Settings.Default.displayName[0];
            Utils.Globals.ViewFrame = ViewFrame;
            ChangeView("Home");
        }

        public void ChangeView(string view)
        {
            if (view == "Home")
            {
                TitleBar.Text = "Home";
                HomeButton.Opacity = 1;
                HomeButton.Background = (Brush)new BrushConverter().ConvertFromString("#343437");
                LibraryButton.Opacity = 0.7;
                LibraryButton.Background = Brushes.Transparent;
                ViewFrame.NavigationService.Navigate(new ViewPages.Home());
            } else if (view == "Library")
            {
                TitleBar.Text = "Library";
                HomeButton.Opacity = 0.7;
                HomeButton.Background = Brushes.Transparent;
                LibraryButton.Opacity = 1;
                LibraryButton.Background = (Brush)new BrushConverter().ConvertFromString("#343437");
                ViewFrame.NavigationService.Navigate(new ViewPages.Library());
            }
            else if (view == "ConfigurePath")
            {
                TitleBar.Text = "Configure GamePath!";
                HomeButton.Opacity = 0.7;
                HomeButton.Background = Brushes.Transparent;
                LibraryButton.Opacity = 0.7;
                LibraryButton.Background = Brushes.Transparent;
                ViewFrame.NavigationService.Navigate(new ViewPages.ConfigurePath());
            } else
            {
                Utils.Logger.error("Invalid View");
            }
        }

        public void ShowErrorOverlay(string errorTitle, string errorMessage)
        {
            Dispatcher.Invoke(() =>
            {
                ErrorTitle.Text = errorTitle;
                ErrorMessage.Text = errorMessage;
                ErrorOverlay.Visibility = Visibility.Visible;
            });
        }

        private void CloseErrorOverlay(object sender, RoutedEventArgs e)
        {
            ErrorOverlay.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewFrame.CanGoBack)
            {
                ViewFrame.GoBack();
            } else
            {
                Utils.Logger.warn("No Past History!");
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeView("Home");
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeView("Library");
        }

        private void QuickLaunchFN_Click(object sender, RoutedEventArgs e)
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
