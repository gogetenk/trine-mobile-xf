﻿using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Authentication.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        #region Bindings

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); SubmitCommand.RaiseCanExecuteChanged(); } }
        private bool _isLoading = false;

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

        public bool _isUnknownUserTextVisible = false;
        public bool IsUnknownUserTextVisible { get => _isUnknownUserTextVisible; set { _isUnknownUserTextVisible = value; RaisePropertyChanged(); } }


        public DelegateCommand SignUpCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand EmailUnfocusedCommand { get; set; }

        #endregion

        private readonly IAccountService _accountService;

        public ForgotPasswordViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            _accountService = accountService;

            SignUpCommand = new DelegateCommand(async () => await OnSignUp());
            SubmitCommand = new DelegateCommand(async () => await OnSubmit());
            GoBackCommand = new DelegateCommand(async () => await OnGoBack());
            EmailUnfocusedCommand = new DelegateCommand(async () => await OnEmailEntered());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Email = AppSettings.CurrentUser?.Mail;
        }

        private async Task OnSignUp()
        {
            await NavigationService.NavigateAsync("SignupView");
        }

        private async Task OnGoBack()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task OnEmailEntered()
        {
            if (string.IsNullOrEmpty(Email))
            {
                IsUnknownUserTextVisible = false;
                return;
            }

            try
            {
                var userToComplete = new RegisterUserDto()
                {
                    Email = Email
                };
                var exists = await _accountService.DoesUserExist(Mapper.Map<RegisterUserModel>(userToComplete));
                IsUnknownUserTextVisible = !exists;
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

        private async Task OnSubmit()
        {
            IsEmailErrorVisible = string.IsNullOrEmpty(Email);
            IsPasswordErrorVisible = string.IsNullOrEmpty(Password);
            IsNotSamePassword = Password != Password2;

            if (IsEmailErrorVisible || IsPasswordErrorVisible || IsNotSamePassword || IsUnknownUserTextVisible)
                return;

            try
            {
                IsLoading = true;

                var passwordUpdate = new PasswordUpdateDto();
                passwordUpdate.Email = Email;
                passwordUpdate.NewPassword = Password;
                await _accountService.RecoverPasswordAsync(Mapper.Map<PasswordUpdateModel>(passwordUpdate));
                await NavigationService.NavigateAsync("../ForgotPasswordConfirmationView");
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
