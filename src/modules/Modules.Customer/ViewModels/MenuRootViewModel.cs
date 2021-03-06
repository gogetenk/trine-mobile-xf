﻿using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Customer.ViewModels
{
    public class MenuRootViewModel : ViewModelBase
    {
        #region Bindings 

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        public ICommand SettingsCommand { get; set; }
        public ICommand ActivitiesCommand { get; set; }
        public ICommand HistoryCommand { get; set; }

        #endregion

        public MenuRootViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService, IDashboardService dashboardService) : base(navigationService, mapper, logger, dialogService)
        {
            SettingsCommand = new DelegateCommand(async () => await OnSettingsTapped());
            ActivitiesCommand = new DelegateCommand(async () => await OnActivitiesTapped());
            HistoryCommand = new DelegateCommand(async () => await OnHistoryTapped());
        }

        private async Task OnSettingsTapped()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/SettingsView");
        }

        private async Task OnActivitiesTapped()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/HomeView");
        }

        private async Task OnHistoryTapped()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/ActivityHistoryView");
        }
    }
}
