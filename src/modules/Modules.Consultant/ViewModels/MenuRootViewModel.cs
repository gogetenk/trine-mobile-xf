using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Consultant.ViewModels
{
    public class MenuRootViewModel : ViewModelBase
    {
        #region Bindings 

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        public ICommand SettingsCommand { get; set; }

        #endregion 

        public MenuRootViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService, IDashboardService dashboardService) : base(navigationService, mapper, logger, dialogService)
        {
            SettingsCommand = new DelegateCommand(async () => await OnSettingsTapped());
        }

        private async Task OnSettingsTapped()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/SettingsView");
        }
    }
}
