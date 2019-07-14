﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Modules.Authentication.Navigation;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Authentication.ViewModels
{
    public class SignupViewModel : ViewModelBase
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); SubmitCommand.RaiseCanExecuteChanged(); } }
        private bool _isLoading = false;

        public bool _isPasswordErrorVisible = false;
        public bool IsPasswordErrorVisible { get => _isPasswordErrorVisible; set { _isPasswordErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isEmailErrorVisible = false;
        public bool IsEmailErrorVisible { get => _isEmailErrorVisible; set { _isEmailErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isUserExistTextVisible = false;
        public bool IsUserExistTextVisible { get => _isUserExistTextVisible; set { _isUserExistTextVisible = value; RaisePropertyChanged(); } }

        public string _email;
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }

        public string _password;
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(); } }

        public DelegateCommand SubmitCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand EmailUnfocusedCommand { get; set; }

        #endregion

        private readonly IAccountService _accountService;
        private readonly IPageDialogService _dialogService;

        public SignupViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            _accountService = accountService;
            _dialogService = dialogService;

            SubmitCommand = new DelegateCommand(async () => await OnSubmit(), () => !IsLoading);
            LoginCommand = new DelegateCommand(async () => await OnLogin());
            EmailUnfocusedCommand = new DelegateCommand(async () => await OnEmailEntered());
        }

        private async Task OnEmailEntered()
        {
            if (string.IsNullOrEmpty(Email))
            {
                IsUserExistTextVisible = false;
                return;
            }

            try
            {
                var userToComplete = new RegisterUserDto()
                {
                    Email = Email
                };
                var exists = await _accountService.DoesUserExist(Mapper.Map<RegisterUserModel>(userToComplete));
                IsUserExistTextVisible = exists;
            }
            catch (BusinessException bExc)
            {
                Logger.Log(bExc.Message);
            }
            catch (Exception exc)
            {
                Logger.Report(exc, null);
            }
        }

        private async Task OnLogin()
        {
            await NavigationService.NavigateAsync("../LoginView");
        }

        private async Task OnSubmit()
        {
            IsEmailErrorVisible = string.IsNullOrEmpty(Email);
            IsPasswordErrorVisible = string.IsNullOrEmpty(Password);

            if (IsEmailErrorVisible || IsPasswordErrorVisible)
                return;


            var userToComplete = new RegisterUserDto()
            {
                Email = Email,
                Password = Password
            };

            try
            {
                IsLoading = true;

                await _accountService.DoesUserExist(Mapper.Map<RegisterUserModel>(userToComplete));

                var navParams = new NavigationParameters();
                navParams.Add(NavigationParameterKeys._User, userToComplete);
                await NavigationService.NavigateAsync("Signup2View", navParams);
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
