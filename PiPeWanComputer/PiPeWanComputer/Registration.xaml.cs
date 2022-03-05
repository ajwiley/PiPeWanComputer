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

namespace PiPeWanComputer
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();

        }

        private void SubmitRegistration(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length == 0 || txtFirstname.Text.Length == 0 || txtLastname.Text.Length == 0 ||
                txtPassword.Password.Length == 0 || txtConfirmPassword.Password.Length == 0)
            {
                //INSERT SOME SORT OF MESSAGE TO ENTER ALL MISSING REQUIRED INFORMATION
                txtFirstname.Focus();
                txtLastname.Focus();
                txtEmail.Focus();
                txtPassword.Focus();
                txtConfirmPassword.Focus();
            }
            else if (!txtEmail.Text.Contains('@'))
            {
                //INSERT SOME SOFT OF MESSAGE TO ENTER AN EMAIL
                txtEmail.Select(0, txtEmail.Text.Length);
                txtEmail.Focus();
            }
            else
            {
                string firstName = txtFirstname.Text;
                string lastName = txtLastname.Text;
                string email = txtEmail.Text;
                string password = txtPassword.Password;
                string confirmPassword = txtConfirmPassword.Password;

                if (password != confirmPassword)
                {
                    //INSERT MESSAGE 
                    txtConfirmPassword.Focus();
                }
                else
                {
                    //CODE THAT CONNECTS TO DATABASE AND INSERTS INFORMATION TO THE CORRECT TABLE WITH firstName, lastName, email, and password
                    Login loginWindow = new Login();
                    loginWindow.Show();
                    Close();
                }

            }
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
