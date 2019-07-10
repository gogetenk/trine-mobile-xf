using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class OrganizationChoiceViewModel : ViewModelBase
    {
        public ICommand CreateOrgaCommand { get; set; }
        public ICommand JoinOrgaCommand { get; set; }

        private RegisterUserDto _userToCreate;
        // Only for the subscribe workflow so it can get the user back if navigating back
        private INavigationParameters _lastPageParams;

        public OrganizationChoiceViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            CreateOrgaCommand = new DelegateCommand(async () => await OnCreateOrga());
            JoinOrgaCommand = new DelegateCommand(async () => await OnJoinOrga());
        }

        private async Task OnCreateOrga()
        {
            await NavigationService.NavigateAsync("CreateOrganizationView");
        }

        private async Task OnJoinOrga()
        {
            await NavigationService.NavigateAsync("JoinOrganizationView");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // Only for the subscribe workflow so it can get the user back if navigating back
            _lastPageParams = parameters;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

        // Only for the subscribe workflow so it can get the user back if navigating back
            parameters = _lastPageParams;
        }
    }
}
