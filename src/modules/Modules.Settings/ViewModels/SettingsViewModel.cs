﻿using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Xamarin.Essentials;

namespace Modules.Settings.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Bindings 

        public UserDto User { get => _user; set { _user = value; RaisePropertyChanged(); } }
        private UserDto _user;

        public ICommand DisconnectCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand NotificationsCommand { get; set; }
        public ICommand IntegrationsCommand { get; set; }
        public ICommand EditUserCommand { get; set; }

        #endregion

        public SettingsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            DisconnectCommand = new DelegateCommand(async () => await OnDisconnect());
            EditUserCommand = new DelegateCommand(async () => await OnEditUser());
        }

        private async Task OnEditUser()
        {
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._User, User);
            await NavigationService.NavigateAsync("EditUserView", navparams);
        }

        private async Task OnDisconnect()
        {
            await NavigationService.NavigateAsync("../LoginView");
            AppSettings.CurrentUser = null;
            SecureStorage.Remove(CacheKeys._CurrentUser);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            User = Mapper.Map<UserDto>(AppSettings.CurrentUser);
        }
    }
}
