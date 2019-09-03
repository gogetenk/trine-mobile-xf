using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class OrganizationMissionsViewModel : ViewModelBase
    {
        #region Bindings 

        private List<MissionDto> _missions;
        public List<MissionDto> Missions { get => _missions; set { _missions = value; RaisePropertyChanged(); } }

        private MissionDto _selectedMission;
        public MissionDto SelectedMission { get => _selectedMission; set { _selectedMission = value; RaisePropertyChanged(); OnSelectedMission(); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public ICommand AddMissionCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        #endregion

        private readonly IMissionService _missionService;
        private bool _hasBeenLoadedOnce;
        private PartialOrganizationDto _organization;

        public OrganizationMissionsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService) : base(navigationService, mapper, logger, dialogService)
        {
            _missionService = missionService;

            AddMissionCommand = new DelegateCommand(async () => await OnAddMission());
            RefreshCommand = new DelegateCommand(async () => await LoadData());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // We dont load the data each time we navigate on the tab
            if (_hasBeenLoadedOnce)
                return;

            _organization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
            if (_organization is null)
            {
                await NavigationService.GoBackAsync();
                return;
            }

            await LoadData();
            _hasBeenLoadedOnce = true;
        }

        private async void OrganizationMissionsViewModel_IsActiveChanged(object sender, EventArgs e)
        {
            OnNavigatedTo(new NavigationParameters());
        }

        private async Task LoadData()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;
                Missions = Mapper.Map<List<MissionDto>>(await _missionService.GetFromOrganization(_organization.Id));
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

        private async Task OnSelectedMission()
        {
            if (SelectedMission is null)
                return;

            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, SelectedMission);
            await NavigationService.NavigateAsync($"MissionDetailsView", navParams);
            // Deselecting item
            SelectedMission = null;
        }

        private async Task OnAddMission()
        {
            if (_organization is null)
                return;

            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Organization, _organization);
            await NavigationService.NavigateAsync("CreateMissionStartView", navParams, true);
        }

    }
}
