using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class ContractDetailsViewModel : CreateMissionViewModelBase
    {
        private FrameContractDto _contract;
        public FrameContractDto Contract { get => _contract; set { _contract = value; RaisePropertyChanged(); } }

        public ContractDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Contract = parameters.GetValue<FrameContractDto>(NavigationParameterKeys._Contract);
        }
    }
}
