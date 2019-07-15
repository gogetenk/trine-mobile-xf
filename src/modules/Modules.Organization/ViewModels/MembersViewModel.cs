using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Organization.ViewModels
{
    public class MembersViewModel : ViewModelBase
    {
        public string OrganizationName { get; set; } = "Panda Services";
        public string PageTitle { get; set; } = "Membres";
        public ICommand AddMemberCommand { get; set; }

        public MembersViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            AddMemberCommand = new DelegateCommand(async () => await OnAddMember());
        }

        private async Task OnAddMember()
        {
            await NavigationService.NavigateAsync("AddMemberView");
        }
    }
}
