using AutoMapper;
using Modules.Mission.ViewModels;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class DummyCreateMissionViewModel : CreateMissionViewModelBase
    {
        public DummyCreateMissionViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }
    }
}
