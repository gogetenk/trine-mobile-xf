﻿using AutoMapper;
using Com.OneSignal.Abstractions;
using Microsoft.AppCenter;
using Prism.Commands;
using Prism.Logging;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Model;
using static Trine.Mobile.Dto.UserDto;

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

#if DEBUG
        public string _email = "ytocreau@trine.com";
#else
        public string _email;
#endif
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }


#if DEBUG
        public string _password = "123";
#else
        public string _password;
#endif
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }

        public DelegateCommand SignupCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ForgotPasswordCommand { get; set; }

        #endregion

        private readonly IAccountService _accountService;
        private readonly IModuleManager _moduleManager;
        private readonly ISupportService _supportService;
        private readonly IOneSignal _oneSignal;

        public LoginViewModel(
            INavigationService navigationService,
            IMapper mapper,
            ILogger logger,
            IAccountService accountService,
            IPageDialogService dialogService,
            IModuleManager moduleManager,
            ISupportService supportService,
            IOneSignal oneSignal) : base(navigationService, mapper, logger, dialogService)
        {
            _accountService = accountService;
            _moduleManager = moduleManager;
            _supportService = supportService;
            _oneSignal = oneSignal;
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

                // Tracking user in intercom
                _supportService.RegisterUser(AppSettings.CurrentUser);

                // Tracking event
                var userRole = Enum.GetName(typeof(GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole);
                Logger.TrackEvent("[Retention] User logged in to the app.", new Dictionary<string, string> {
                    { "UserId", userId },
                    { "UserName", $"{AppSettings.CurrentUser.Firstname} {AppSettings.CurrentUser.Lastname}"},
                    { "UserType", userRole }
                });
                // Setting the user id to app center
                AppCenter.SetUserId(userId);
                // Setting the user id to One Signal, and assigning tag
                _oneSignal.SetExternalUserId(userId);
                _oneSignal.SendTag("user_type", userRole);
                // Loading the corresponding module depending on user type
                LoadModuleFromUserType();
                await NavigationService.NavigateAsync("MenuRootView/TrineNavigationPage/HomeView");
            }
            catch (BusinessException bExc)
            {
                await DialogService.DisplayAlertAsync("erreur", bExc.Message, "ok");
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


        private void LoadModuleFromUserType()
        {
            string moduleName;
            switch (AppSettings.CurrentUser.GlobalRole)
            {
                case UserModel.GlobalRoleEnum.Admin:
                    moduleName = "CommercialModule";
                    break;
                case UserModel.GlobalRoleEnum.Consultant:
                    moduleName = "ConsultantModule";
                    break;
                case UserModel.GlobalRoleEnum.Customer:
                    moduleName = "CustomerModule";
                    break;
                default:
                    throw new BusinessException("Votre compte utilisateur n'est pas adapté à cette version de Trine. Veuillez créer un nouveau compte ou contacter le support client.");
            }
            _moduleManager.LoadModule(moduleName);
        }
    }
}
