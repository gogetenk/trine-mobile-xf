using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContractViewModel : CreateMissionViewModelBase
    {
        public CreateMissionContractViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }
    }
}
