using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using Trine.Mobile.Components.Navigation;

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

            CreateMissionRequest.Consultant = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            navigationParams.Add(NavigationParameterKeys._Organization, SelectedOrganization);
            await NavigationService.NavigateAsync("CreateMissionCommercialView", navigationParams);
        }
    }
}
