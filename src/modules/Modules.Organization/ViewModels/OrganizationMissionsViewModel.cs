using AutoMapper;
using Prism;
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
    public class OrganizationMissionsViewModel : ViewModelBase, IActiveAware
    {
        #region Bindings 

        private List<MissionDto> _missions;
        public List<MissionDto> Missions { get => _missions; set { _missions = value; RaisePropertyChanged(); } }

        private MissionDto _selectedMission;
        public MissionDto SelectedMission { get => _selectedMission; set { _selectedMission = value; RaisePropertyChanged(); OnSelectedMission(); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public ICommand AddMissionCommand { get; set; }

        #endregion

        public event EventHandler IsActiveChanged;
        private bool _isActive;
        private readonly IMissionService _missionService;

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        public OrganizationMissionsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService) : base(navigationService, mapper, logger, dialogService)
        {
            _missionService = missionService;
            IsActiveChanged += OrganizationMissionsViewModel_IsActiveChanged;
            AddMissionCommand = new DelegateCommand(async () => await OnAddMission());
        }

        private async void OrganizationMissionsViewModel_IsActiveChanged(object sender, EventArgs e)
        {
            try
            {
                IsLoading = true;
                Missions = Mapper.Map<List<MissionDto>>(await _missionService.GetFromOrganization("5ca5cab077e80c1344dbafec"));
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
            await NavigationService.NavigateAsync("MissionDetailsView", navParams);
        }
        private async Task OnAddMission()
        {
            await NavigationService.NavigateAsync("CreateMissionStartView", useModalNavigation: true);
        }

        // Triggered only on tabbed pages
        protected virtual async void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
