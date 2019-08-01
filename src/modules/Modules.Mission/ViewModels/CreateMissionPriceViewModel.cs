using System;
using System.Threading.Tasks;
using System.Windows.Input;
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

        private float _totalPrice;
        public float TotalPrice { get => _totalPrice; set { _totalPrice = value; RaisePropertyChanged(); } }

        public ICommand PriceChangedCommand { get; set; }

        #endregion

        public CreateMissionPriceViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
            PriceChangedCommand = new DelegateCommand(() => CalculateTotalPrice());
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
            IsDailyRateErrorVisible = CreateMissionRequest.DailyPrice == 0;
            if (IsDailyRateErrorVisible)
                return;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionContractView", navigationParams, useModalNavigation: false);
        }

        // Called every time the user changes the price
        private void CalculateTotalPrice()
        {
            TotalPrice = CreateMissionRequest.DailyPrice + (CreateMissionRequest.DailyPrice * CreateMissionRequest.CommercialFeePercentage * .01f);
        }

    }
}
