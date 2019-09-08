using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class OrganizationChoiceViewModel : ViewModelBase
    {
        public ICommand CreateOrgaCommand { get; set; }
        public ICommand JoinOrgaCommand { get; set; }
        public ICommand LaterCommand { get; set; }

        public bool IsLaterTextVisible { get => _isLaterTextVisible; set { _isLaterTextVisible = value; RaisePropertyChanged(); } }
        private bool _isLaterTextVisible;

        public bool IsBackVisible { get => _isBackVisible; set { _isBackVisible = value; RaisePropertyChanged(); } }
        private bool _isBackVisible;

        private readonly RegisterUserDto _userToCreate;
        // Only for the subscribe workflow so it can get the user back if navigating back
        private INavigationParameters _lastPageParams;

        public OrganizationChoiceViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            CreateOrgaCommand = new DelegateCommand(async () => await OnCreateOrga());
            JoinOrgaCommand = new DelegateCommand(async () => await OnJoinOrga());
            LaterCommand = new DelegateCommand(async () => await NavigateToDashboard());
        }

        private async Task NavigateToDashboard()
        {
            if (IsLaterTextVisible)
                await NavigationService.NavigateAsync("MenuRootView/TrineNavigationPage/DashboardView");
            else
                await NavigationService.GoBackAsync();
        }

        private async Task OnCreateOrga()
        {
            await NavigationService.NavigateAsync("CreateOrganizationView");
        }

        private async Task OnJoinOrga()
        {
            await NavigationService.NavigateAsync("JoinOrganizationView");
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // Only for the subscribe workflow so it can get the user back if navigating back
            _lastPageParams = parameters;
            IsBackVisible = !parameters.GetValue<bool>(NavigationParameterKeys._IsLaterTextVisible);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            // Only for the subscribe workflow so it can get the user back if navigating back
            parameters = _lastPageParams;
        }
    }
}
