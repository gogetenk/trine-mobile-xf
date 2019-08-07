using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Menu.ViewModels
{
    public class MenuRootViewModel : ViewModelBase
    {
        #region Bindings 

        private ObservableCollection<MissionDto> _missions;
        public ObservableCollection<MissionDto> Missions { get => _missions; set { _missions = value; RaisePropertyChanged(); } }

        private ObservableCollection<PartialOrganizationDto> _organizations;
        public ObservableCollection<PartialOrganizationDto> Organizations { get => _organizations; set { _organizations = value; RaisePropertyChanged(); } }

        private MissionDto _selectedMission;
        public MissionDto SelectedMission { get => _selectedMission; set { _selectedMission = value; RaisePropertyChanged(); OnMissionChanged(); } }


        private PartialOrganizationDto _selectedOrganization;
        public PartialOrganizationDto SelectedOrganization { get => _selectedOrganization; set { _selectedOrganization = value; RaisePropertyChanged(); OnOrganizationChanged(); } }

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }


        public ICommand CreateMissionCommand { get; set; }
        public ICommand CreateOrganizationCommand { get; set; }

        #endregion 

        private readonly IMissionService _missionService;
        private readonly IDashboardService _dashboardService;


        public MenuRootViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IMissionService missionService, IDashboardService dashboardService) : base(navigationService, mapper, logger, dialogService)
        {
            CreateMissionCommand = new DelegateCommand(async () => await OnCreateMission());
            CreateOrganizationCommand = new DelegateCommand(async () => await OnAddOrganization());

            _missionService = missionService;
            _dashboardService = dashboardService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            // Known issue in Xamarin forms where the MasterPage OnAppearing is raised twice
            base.OnNavigatedTo(parameters);

            await LoadUserMissions();
        }

        private async Task LoadUserMissions()
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;
                var getMissionsTask = _missionService.GetUserMissions(AppSettings.CurrentUser.Id);
                var getOrgasTask = _dashboardService.GetUserOrganizations(AppSettings.CurrentUser.Id);

                Organizations = Mapper.Map<ObservableCollection<PartialOrganizationDto>>(await getOrgasTask);
                Missions = Mapper.Map<ObservableCollection<MissionDto>>(await getMissionsTask);
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

        private async Task OnCreateMission()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/CreateMissionStartView", useModalNavigation: true);
        }

        private async Task OnAddOrganization()
        {
            await NavigationService.NavigateAsync("OrganizationChoiceView", useModalNavigation: true);
        }

        private async Task OnOrganizationChanged()
        {
            if (SelectedOrganization is null)
                return;

            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Organization, SelectedOrganization);
            await NavigationService.NavigateAsync("TrineNavigationPage/OrganizationDetailsView", navParams);
        }

        private async Task OnMissionChanged()
        {
            if (SelectedMission is null)
                return;

            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, SelectedMission);
            await NavigationService.NavigateAsync("TrineNavigationPage/MissionDetailsView", navParams);
        }
    }
}
