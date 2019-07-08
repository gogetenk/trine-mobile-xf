using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Modules.Authentication.Navigation;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Authentication.ViewModels
{
    public class OrganizationChoiceViewModel : ViewModelBase
    {
        public ICommand CreateOrgaCommand { get; set; }
        public ICommand JoinOrgaCommand { get; set; }


        public OrganizationChoiceViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
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
            await NavigationService.NavigateAsync("CreateOrganizationView");
        }
    }
}
