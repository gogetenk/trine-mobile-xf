using System.Threading.Tasks;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionConsultantViewModel : CreateMissionViewModelBase
    {
        public CreateMissionConsultantViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
        }

        protected async Task OnNextStep()
        {
            IsUserErrorVisible = PickedUser is null;
            if (IsUserErrorVisible)
                return;

            var createMissionRequest = new CreateMissionRequestDto();
            createMissionRequest.Consultant = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, createMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionCommercialView", navigationParams, useModalNavigation: false);
        }
    }
}
