using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionPriceView : CreateMissionViewModelBase
    {
        #region Bindings

        #endregion

        public CreateMissionPriceView(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }
    }
}
