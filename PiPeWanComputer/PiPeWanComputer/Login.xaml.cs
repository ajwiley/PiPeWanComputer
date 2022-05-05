using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using PiPeWanComputer.SQL_Stuff;
using PiPeWanComputer.Views;

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window {
        public Login() {
            InitializeComponent();
        }

        private void ExitApp(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void DoLogin(object sender, RoutedEventArgs e) {
            string username = txtUsername.Text.ToLower();
            string password = txtPassword.Password;

            password = password.PadRight(64, '0');
            var pBytes = Encoding.UTF8.GetBytes(password);

            // If no user with that username is fount, SelectUser() returns null
            User? user = PipeDB.SelectUser(username);

            if (user is null) {
                // The given username does not exist in the database
                MessageBox.Show($"No user with username \"{username}\" exists!");
            }
            else if (!pBytes.SequenceEqual(user.PasswordHash)) {
                // The incorrect password.
                MessageBox.Show($"Incorrect password for \"{username}\"");
            }
            else {
                // let them login
                MainWindow mainWindow = new MainWindow();
                Close();
                mainWindow.Show();
            }
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e) {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark) {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }

            paletteHelper.SetTheme(theme);
        }

        private void PasswordKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                bntLogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
        }
    }
}
