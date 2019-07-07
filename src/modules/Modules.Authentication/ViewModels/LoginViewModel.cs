using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Authentication.ViewModels
{
    public class LoginViewModel : ViewModelBase
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

        public ICommand SignupCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        #endregion

        public LoginViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
        {
            LoginCommand = new DelegateCommand(async () => await OnLogin(), () => !IsEmailErrorVisible && !IsPasswordErrorVisible);
            ForgotPasswordCommand = new DelegateCommand(async () => await OnForgotPassword());
            SignupCommand = new DelegateCommand(async () => await OnSignup());
        }

        private async Task OnForgotPassword()
        {
            await NavigationService.NavigateAsync("../ForgotPasswordView");
        }

        private async Task OnSignup()
        {
            await NavigationService.NavigateAsync("../SignupView");
        }

        private async Task OnLogin()
        {
            IsEmailErrorVisible = string.IsNullOrEmpty(Email);
            IsPasswordErrorVisible = string.IsNullOrEmpty(Password);

            if (IsEmailErrorVisible || IsPasswordErrorVisible)
                return;

            await NavigationService.NavigateAsync("../");
        }
    }
}
