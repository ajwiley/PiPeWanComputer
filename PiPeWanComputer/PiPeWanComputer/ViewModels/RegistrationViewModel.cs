using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.ViewModels {
    public class RegistrationViewModel : ViewModelBase {
        private string _FirstName;
        private string _LastName;
        private string _Email;
        private string _Password;
        private string _PasswordConfirm;

        public string FirstName { get => _FirstName; set => SetProperty(ref _FirstName, value); }
        public string LastName { get => _LastName; set => SetProperty(ref _LastName, value); }
        public string Email { get => _Email; set => SetProperty(ref _Email, value); }
        public string Password { get => _Password; set => SetProperty(ref _Password, value); }
        public string PasswordConfirm { get => _PasswordConfirm; set => SetProperty(ref _PasswordConfirm, value); }
    }
}
