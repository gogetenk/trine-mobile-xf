using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Dashboard.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly ISupportService _supportService;

        public DashboardViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, ISupportService supportService) : base(navigationService, mapper, logger, dialogService)
        {
            _supportService = supportService;

            // Tracking user 
            _supportService.RegisterUser(AppSettings.CurrentUser);
        }
    }
}
