using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Authentication.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        #region Bindings

        public bool _isPasswordErrorVisible = false;
        public bool IsPasswordErrorVisible { get => _isPasswordErrorVisible; set { _isPasswordErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isEmailErrorVisible = false;
        public bool IsEmailErrorVisible { get => _isEmailErrorVisible; set { _isEmailErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isNotSamePassword = false;
        public bool IsNotSamePassword { get => _isNotSamePassword; set { _isNotSamePassword = value; RaisePropertyChanged(); } }

        public string _email;
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }

        public string _password;
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }

        public string _password2;
        public string Password2 { get => _password2; set { _password2 = value; RaisePropertyChanged(); } }

        public ICommand SubmitCommand { get; set; }

        #endregion

        public ForgotPasswordViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
        {
            SubmitCommand = new DelegateCommand(async () => await OnSubmit());
        }

        private async Task OnSubmit()
        {
            IsEmailErrorVisible = string.IsNullOrEmpty(Email);
            IsPasswordErrorVisible = string.IsNullOrEmpty(Password);
            IsNotSamePassword = Password != Password2;

            if (IsEmailErrorVisible || IsPasswordErrorVisible || IsNotSamePassword)
                return;

            await NavigationService.NavigateAsync("ForgotPasswordConfirmationView");
        }
    }
}
