﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Microsoft.AppCenter;
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
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }


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

        private readonly IAccountService _accountService;
        private readonly IPageDialogService _dialogService;

        public LoginViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService) : base(navigationService, mapper, logger)
        {
            _accountService = accountService;
            _dialogService = dialogService;

            LoginCommand = new DelegateCommand(async () => await OnLogin(), () => !IsEmailErrorVisible && !IsPasswordErrorVisible);
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

                await NavigationService.NavigateAsync("../DashboardView");
                //TODO à enlever quand on aura le dashboard
                await _dialogService.DisplayAlertAsync("Vous êtes connecté à Trine !", "Un peu de patience, le reste arrive bientôt...", "J'ai hâte !");
            }
            catch (BusinessException bExc)
            {
                Logger.Log(bExc.Message);
                await _dialogService.DisplayAlertAsync(ErrorMessages.error, bExc.Message, "Ok");
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
