using System.Threading.Tasks;
using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionCommercialViewModel : CreateMissionViewModelBase
    {
        public CreateMissionCommercialViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }

        protected async Task OnNextStep()
        {
            IsUserErrorVisible = PickedUser is null;
            if (IsUserErrorVisible)
                return;

            var createMissionRequest = new CreateMissionRequestDto();
            createMissionRequest.Commercial = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, createMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionPriceView", navigationParams, useModalNavigation: false);
        }
    }
}
