using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace Modules.Authentication.ViewModels
{
    public class SignupViewModel : BindableBase
    {
        #region Bindings 

        public bool _isPasswordErrorVisible = false;
        public bool IsPasswordErrorVisible { get => _isPasswordErrorVisible; set { _isPasswordErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isEmailErrorVisible = false;
        public bool IsEmailErrorVisible { get => _isEmailErrorVisible; set { _isEmailErrorVisible = value; RaisePropertyChanged(); } }

        public string _email;
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }

        public string _password;
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }

        public ICommand SubmitCommand { get; set; }

        #endregion


        public SignupViewModel()
        {
            SubmitCommand = new DelegateCommand(async () => await OnSubmit(), () => !IsEmailErrorVisible && !IsPasswordErrorVisible);
        }

        private async Task OnSubmit()
        {
            IsEmailErrorVisible = string.IsNullOrEmpty(Email);
            IsPasswordErrorVisible = string.IsNullOrEmpty(Password);

            if (IsEmailErrorVisible || IsPasswordErrorVisible)
                return;

        }
    }
}
