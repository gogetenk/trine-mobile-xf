using System;
using System.Threading.Tasks;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Mission.ViewModels
{
    public class MissionStartViewModel : ViewModelBase
    {
        public MissionDto Mission { get => _mission; set { _mission = value; RaisePropertyChanged(); } }
        private MissionDto _mission;

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); StartCommand.RaiseCanExecuteChanged(); } }
        private bool _isLoading = false;

        public DelegateCommand StartCommand { get; set; }

        private readonly IMissionService _missionService;
        
        public MissionStartViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService) : base(navigationService, mapper, logger, dialogService)
        {
            StartCommand = new DelegateCommand(async () => await OnStartMission(), () => !IsLoading);
            _missionService = missionService;
        }

        private async Task OnStartMission()
        {
            try
            {
                IsLoading = true;
                var updatedMission = Mapper.Map<MissionDto>(await _missionService.ActivateMissionAsync(Mapper.Map<MissionModel>(Mission)));
                if (updatedMission is null)
                    return;

                Mission = updatedMission; // Mise à jour de l'UI

                var navParams = new NavigationParameters();
                navParams.Add(NavigationParameterKeys._Mission, Mission);

                await NavigationService.NavigateAsync("../MissionDetailsView", navParams);
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
    }
}
