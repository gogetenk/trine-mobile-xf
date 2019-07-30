using System;
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
    public class CreateMissionPriceViewModel : CreateMissionViewModelBase
    {
        #region Bindings

        public bool IsDailyRateErrorVisible { get; set; } = false;

        #endregion

        public CreateMissionPriceViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (CreateMissionRequest.DailyPrice == 0)
                CreateMissionRequest.DailyPrice = 400;

            if (CreateMissionRequest.CommercialFeePercentage == 0 && CreateMissionRequest.IsTripartite)
                CreateMissionRequest.CommercialFeePercentage = 15;
        }

        private async Task OnNextStep()
        {
            IsUserErrorVisible = PickedUser is null;
            if (IsUserErrorVisible)
                return;

            var createMissionRequest = new CreateMissionRequestDto();
            createMissionRequest.Commercial = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, createMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionContractView", navigationParams, useModalNavigation: false);
        }
    }
}
