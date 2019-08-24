using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class OrganizationTabbedViewModel : ViewModelBase
    {
        private MembersViewModel _membersViewModel;
        public MembersViewModel MembersViewModel
        {
            get => _membersViewModel;
            set { _membersViewModel = value; RaisePropertyChanged(); }
        }

        private OrganizationMissionsViewModel _organizationMissionsViewModel;
        public OrganizationMissionsViewModel OrganizationMissionsViewModel
        {
            get => _organizationMissionsViewModel;
            set { _organizationMissionsViewModel = value; RaisePropertyChanged(); }
        }

        private int _selectedViewModelIndex = 0;
        public int SelectedViewModelIndex
        {
            get => _selectedViewModelIndex;
            set { _selectedViewModelIndex = value; RaisePropertyChanged(); TriggerOnNavigatedTo(value); }
        }

        private PartialOrganizationDto _organization;
        public PartialOrganizationDto Organization { get => _organization; set { _organization = value; RaisePropertyChanged(); } }


        public OrganizationTabbedViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService, IMissionService missionService) : base(navigationService, mapper, logger, dialogService)
        {
            MembersViewModel = new MembersViewModel(navigationService, mapper, logger, dialogService, organizationService);
            OrganizationMissionsViewModel = new OrganizationMissionsViewModel(navigationService, mapper, logger, dialogService, missionService);
        }

        private async Task TriggerOnNavigatedTo(int value)
        {
            if (Organization is null)
            {
                await NavigationService.GoBackAsync();
                return;
            }

            var parameters = new NavigationParameters();
            parameters.Add(NavigationParameterKeys._Organization, Organization);

            switch (value)
            {
                case 0:
                    OrganizationMissionsViewModel.OnNavigatedTo(parameters);
                    break;
                case 1:
                    MembersViewModel.OnNavigatedTo(parameters);
                    MembersViewModel.IsActionButtonShown = true;
                    break;
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (Organization != null)
                return;

            Organization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
            if (Organization is null)
                await NavigationService.GoBackAsync();

            await TriggerOnNavigatedTo(0);
        }
    }
}
