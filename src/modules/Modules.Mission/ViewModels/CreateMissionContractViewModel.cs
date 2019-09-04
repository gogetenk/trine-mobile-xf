using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContractViewModel : CreateMissionViewModelBase
    {
        #region Bindings 

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); ReadCommand.RaiseCanExecuteChanged(); } }

        private FrameContractDto _contract;
        public FrameContractDto Contract { get => _contract; set { _contract = value; RaisePropertyChanged(); } }

        public DelegateCommand ReadCommand { get; set; }

        #endregion

        private readonly IMissionService _missionService;

        public CreateMissionContractViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService) : base(navigationService, mapper, logger, dialogService)
        {
            _missionService = missionService;

            NextCommand = new DelegateCommand(async () => await OnNextStep());
            ReadCommand = new DelegateCommand(async () => await OnReadContract(), () => !IsLoading);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await GetContract();
        }

        private async Task OnNextStep()
        {
            if (IsLoading)
                return;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionSuccessView", navigationParams);
        }

        private async Task GetContract()
        {
            try
            {
                IsLoading = true;
                var contractModel = await _missionService.GetContractPreview(Mapper.Map<CreateMissionRequestModel>(CreateMissionRequest));
                if (contractModel == null)
                    throw new BusinessException("Une erreur s'est produite lors de la récuparation du modèle de contrat.");

                Contract = Mapper.Map<FrameContractDto>(contractModel);
            }
            catch (BusinessException bExc)
            {
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OnReadContract()
        {
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Contract, Contract);
            await NavigationService.NavigateAsync("ContractDetailsView", navParams, true);
        }
    }
}
