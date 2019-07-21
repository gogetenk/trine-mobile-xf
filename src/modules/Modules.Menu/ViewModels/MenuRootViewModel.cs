using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Menu.ViewModels
{
    public class MenuRootViewModel : ViewModelBase
    {
        public ICommand CreateMissionCommand { get; set; }

        public MenuRootViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            CreateMissionCommand = new DelegateCommand(async () => await OnCreateMission());
        }

        private async Task OnCreateMission()
        {
            await NavigationService.NavigateAsync("CreateMissionStartView", useModalNavigation: true);
        }
    }
}
