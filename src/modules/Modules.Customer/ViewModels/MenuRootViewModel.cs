using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Customer.ViewModels
{
    public class MenuRootViewModel : ViewModelBase
    {
        #region Bindings 

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        #endregion 

        public MenuRootViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService, IDashboardService dashboardService) : base(navigationService, mapper, logger, dialogService)
        {

        }
    }
}
