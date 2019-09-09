using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Components.Navigation;

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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var request = CreateMissionRequest;

            if (request.DailyPrice == 0)
                request.DailyPrice = 400;

            if (request.CommercialFeePercentage == 0 && request.IsTripartite)
                request.CommercialFeePercentage = 15f;

            CreateMissionRequest = request;
        }

        private async Task OnNextStep()
        {
            IsDailyRateErrorVisible = CreateMissionRequest.DailyPrice == 0;
            if (IsDailyRateErrorVisible)
                return;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionContractView", navigationParams);
        }

        // Called every time the user changes the price
        private void CalculateTotalPrice()
        {
            TotalPrice = CreateMissionRequest.DailyPrice + (CreateMissionRequest.DailyPrice * CreateMissionRequest.CommercialFeePercentage * .01f);
        }

    }
}
