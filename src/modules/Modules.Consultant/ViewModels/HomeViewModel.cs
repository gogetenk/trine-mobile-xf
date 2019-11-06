using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Consultant.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(INavigationService navigationService, global::AutoMapper.IMapper mapper, Prism.Logging.ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }
    }
}
