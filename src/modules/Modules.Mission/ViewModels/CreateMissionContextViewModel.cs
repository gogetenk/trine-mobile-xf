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
    public class CreateMissionContextViewModel : CreateMissionViewModelBase
    {
        #region Bindings

        public bool IsTitleEmptyErrorVisible { get; set; } = false;

        #endregion

        public CreateMissionContextViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
            CreateMissionRequest = new CreateMissionRequestDto();
            CreateMissionRequest.IsTripartite = true; // Default
        }

        private async Task OnNextStep()
        {
            IsUserErrorVisible = PickedUser is null;
            IsTitleEmptyErrorVisible = string.IsNullOrEmpty(CreateMissionRequest.ProjectName);
            CanGoNext = IsUserErrorVisible || IsTitleEmptyErrorVisible;
            if (IsUserErrorVisible || IsTitleEmptyErrorVisible)
                return;

            CreateMissionRequest.Customer = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionDatesView", navigationParams, useModalNavigation: false);
        }

    }
}
