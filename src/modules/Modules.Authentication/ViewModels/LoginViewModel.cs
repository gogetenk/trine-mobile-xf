using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Authentication.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Bindings

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); } }

        public bool _isPasswordErrorVisible = false;
        public bool IsPasswordErrorVisible { get => _isPasswordErrorVisible; set { _isPasswordErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isEmailErrorVisible = false;
        public bool IsEmailErrorVisible { get => _isEmailErrorVisible; set { _isEmailErrorVisible = value; RaisePropertyChanged(); } }

        public string _email = "ytocreau@trine.com";
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }

        public string _password = "123";
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }

        public DelegateCommand SignupCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ForgotPasswordCommand { get; set; }

        #endregion

        private readonly IAccountService _accountService;

        public LoginViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            _accountService = accountService;

            LoginCommand = new DelegateCommand(async () => await OnLogin(), () => !IsEmailErrorVisible && !IsPasswordErrorVisible && !IsLoading);
            ForgotPasswordCommand = new DelegateCommand(async () => await OnForgotPassword());
            SignupCommand = new DelegateCommand(async () => await OnSignup());
        }

        private async Task OnForgotPassword()
        {
            await NavigationService.NavigateAsync("ForgotPasswordView");
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

            try
            {
                IsLoading = true;
                var userId = await _accountService.Login(Email, Password);

                if (string.IsNullOrEmpty(userId))
                    return;

                // Setting the user id to app center
                //AppCenter.SetUserId(userId);

                await NavigationService.NavigateAsync("MenuRootView/TrineNavigationPage/DashboardView");
            }
            catch (BusinessException bExc)
            {
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
