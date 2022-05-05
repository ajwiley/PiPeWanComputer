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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using PiPeWanComputer.SQL_Stuff;

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window {
        public Registration() {
            InitializeComponent();
            PipeDB.CreateDB();
        }

        private void SubmitRegistration(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(txtFirstname.Text)) {
                // TODO: username shouldn't be longer than 12 characters (username is first letter of first name + lastname)
                MessageBox.Show("Must enter a first name!");
                txtFirstname.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtLastname.Text)) {
                MessageBox.Show("Must enter a last name!");
                txtLastname.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains('@')) {
                // TODO: Regex Check email

                MessageBox.Show("Must enter a valid email!");
                txtEmail.Select(0, txtEmail.Text.Length);
                txtEmail.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(txtPassword.Text)) {
                // TODO: password shouldn't be longer than 12 characters
                MessageBox.Show("Must enter a password!");
                txtPassword.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(txtConfirmPassword.Text)) {
                MessageBox.Show("Please confirm password!");
                txtConfirmPassword.Focus();
                return;
            }
            else if (txtPassword.Text != txtConfirmPassword.Text) {
                MessageBox.Show("Passwords do not match!");
                txtPassword.Focus();
                return;
            }

            string username = txtFirstname.Text.ToLower().First() + txtLastname.Text.ToLower();

            if (PipeDB.SelectUser(username) is null) {
                string password = txtPassword.Text.PadRight(64, '0');
                var pBytes = Encoding.UTF8.GetBytes(password);
                PipeDB.AddUser(username, pBytes);
                Login loginWindow = new Login();
                loginWindow.Show();
                Close();
            }
            else {
                MessageBox.Show($"Username {username} already exists!");
                txtFirstname.Focus();
            }
        }

        private void ExitApp(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            DragMove();
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

        private void btnLoginWindow_Click(object sender, RoutedEventArgs e) {
            Login loginWindow = new Login();
            loginWindow.Show();
            Close();
        }
    }
}
