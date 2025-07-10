using Arcane_Launcher.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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

namespace Arcane_Launcher.Pages.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            AuthorizationCodeBox.TextChanged += AuthorizationCodeBox_TextChanged;
            MessageBox.Show($"You will be redirected to the Epic Games website to login. After logging in please get the \"authorizationCode\".", "Legacy | Authenticate", MessageBoxButton.OK, MessageBoxImage.Information);
            Process.Start(new ProcessStartInfo("https://www.epicgames.com/id/api/redirect?clientId=ec684b8c687f479fadea3cb2ad83f5c6&responseType=code") { UseShellExecute = true });
        }

        private void AuthorizationCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuthorizationCodeBox.Text))
            {
                LoginButton.Background = new SolidColorBrush(Color.FromRgb(68, 67, 72));
            }
            else
            {
                LoginButton.Background = new SolidColorBrush(Color.FromRgb(38, 187, 255));
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuthorizationCodeBox.Text)) { } else
            {
                await GetAccessToken(this, AuthorizationCodeBox.Text);
            }
        }

        static async Task GetAccessToken(Login loginPage, string AuthorizationCode)
        {
            JObject json = await Authentication.GetAccessToken(loginPage.AuthorizationCodeBox.Text);
            if (json.ContainsKey("access_token"))
            {
                loginPage.LoadingBar.Visibility = Visibility.Visible;
                loginPage.LoadingText.Text = $"Welcome {json["displayName"].ToString()}...";
                Utils.Globals.MainFrame.Navigate(new Pages.Launcher.MainView());
            }
            else
            {
                Utils.Logger.error("Could not get access token!");
                MessageBox.Show($"Unable to login, If you are having issues please make sure you are logged into epic games on your browser then try again!", "Legacy | Authenticate", MessageBoxButton.OK, MessageBoxImage.Warning);
                MessageBox.Show($"You will be redirected to the Epic Games website to login. After logging in please get the \"authorizationCode\".", "Legacy | Authenticate", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start(new ProcessStartInfo("https://www.epicgames.com/id/api/redirect?clientId=ec684b8c687f479fadea3cb2ad83f5c6&responseType=code") { UseShellExecute = true });
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
    }
}
