using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Consultant.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }
    }
}
