using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContextViewModel : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand PickClientCommand { get; set; }

        public CreateMissionContextViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("CreateMissionDatesView", useModalNavigation: false));
            PickClientCommand = new DelegateCommand(async () => await OnPickClient());
        }

        private async Task<INavigationResult> OnPickClient()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._IsUserPickerModeEnabled, true);
            navigationParams.Add(NavigationParameterKeys._NavigatedFromUri, "CreateMissionContextView");

            return await NavigationService.NavigateAsync("MembersView", navigationParams, useModalNavigation: true);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            
        }
    }
}
