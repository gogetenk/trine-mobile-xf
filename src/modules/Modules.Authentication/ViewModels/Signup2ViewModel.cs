using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Authentication.ViewModels
{
    public class Signup2ViewModel : ViewModelBase
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

        public ICommand NextCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        #endregion


        public Signup2ViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
        {
            NextCommand = new DelegateCommand(async () => await OnLogin());
            LoginCommand = new DelegateCommand(async () => await OnSubmit());
        }

        private async Task OnLogin()
        {
            await NavigationService.NavigateAsync("LoginView");
        }

        private async Task OnSubmit()
        {
            await NavigationService.NavigateAsync("Signup3View");
        }
    }
}
