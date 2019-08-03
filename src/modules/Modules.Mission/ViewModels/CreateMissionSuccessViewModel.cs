using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionSuccessViewModel : CreateMissionViewModelBase
    {
        private readonly IMissionService _missionService;

        #region Bindings 

        public ICommand NavigateToMissionDetailsCommand { get; set; }
        public ICommand RetryCommand { get; set; }

        private bool _isLoading = true;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        private bool _isSuccessShown = false;
        public bool IsSuccessShown { get => _isSuccessShown; set { _isSuccessShown = value; RaisePropertyChanged(); } }

        private bool _isErrorShown = false;
        public bool IsErrorShown { get => _isErrorShown; set { _isErrorShown = value; RaisePropertyChanged(); } }

        private bool _isGridEnabled = false;
        public bool IsGridEnabled { get => _isGridEnabled; set { _isGridEnabled = value; RaisePropertyChanged(); } }

        private MissionDto _createdMission;
        public MissionDto CreatedMission { get => _createdMission; set { _createdMission = value; RaisePropertyChanged(); } }

        #endregion 

        public CreateMissionSuccessViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService) : base(navigationService, mapper, logger, dialogService)
        {
            NavigateToMissionDetailsCommand = new DelegateCommand(async () => await NavigateToMissionDetails());
            RetryCommand = new DelegateCommand(async () => await OnRetry());

            _missionService = missionService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await CreateMission();
        }

        private async Task OnRetry()
        {
            await CreateMission();
        }

        private async Task CreateMission()
        {
            try
            {
                IsLoading = true;

                CreatedMission = Mapper.Map<MissionDto>(await _missionService.CreateMission(Mapper.Map<CreateMissionRequestModel>(CreateMissionRequest)));
                if (CreatedMission is null)
                    throw new BusinessException(ErrorMessages.unknownError);

                IsErrorShown = false;
                IsSuccessShown = true;
                IsGridEnabled = true;
            }
            catch (BusinessException bExc)
            {
                IsErrorShown = false;
                IsSuccessShown = true;
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                IsErrorShown = false;
                IsSuccessShown = true;
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task NavigateToMissionDetails()
        {
            await NavigationService.NavigateAsync($"MenuRootView/TrineNavigationPage/DashboardView?MissionId={CreatedMission.Id}", useModalNavigation: false);
        }
    }
}
