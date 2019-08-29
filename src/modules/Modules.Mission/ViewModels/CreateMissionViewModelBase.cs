using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public abstract class CreateMissionViewModelBase : ViewModelBase
    {
        #region Bindings 

        public DelegateCommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PickUserCommand { get; set; }
        public ICommand RemovedUserCommand { get; set; }

        private static CreateMissionRequestDto _createMissionRequest;
        public CreateMissionRequestDto CreateMissionRequest { get => _createMissionRequest; set { _createMissionRequest = value; RaisePropertyChanged(); } }

        private bool _canGoNext = true;
        public bool CanGoNext { get => _canGoNext; set { _canGoNext = value; RaisePropertyChanged(); } }

        private UserDto _pickedUser;
        public UserDto PickedUser { get => _pickedUser; set { _pickedUser = value; RaisePropertyChanged(); } }

        private bool _isUserErrorVisible = false;
        public bool IsUserErrorVisible { get => _isUserErrorVisible; set { _isUserErrorVisible = value; RaisePropertyChanged(); } }

        private bool _isInvitedUser = false;
        public bool IsInvitedUser { get => _isInvitedUser; set { _isInvitedUser = value; RaisePropertyChanged(); } }

        private PartialOrganizationDto _selectedOrganization;
        public PartialOrganizationDto SelectedOrganization { get => _selectedOrganization; set { _selectedOrganization = value; RaisePropertyChanged(); } }

        #endregion

        public CreateMissionViewModelBase(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            PickUserCommand = new DelegateCommand(async () => await OnPickUser());
            RemovedUserCommand = new DelegateCommand(() => OnRemoveUser());

            var parameters = new NavigationParameters();
            parameters.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            BackCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync(parameters));
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await base.InitializeAsync(parameters);

            // If the current Picked user is null, we reset the UI of the picker
            if (PickedUser is null)
                OnRemoveUser();

            SelectedOrganization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);

            // We get the user from navigation (from picker) only if it was null or if he has not the same id
            var pickedUser = parameters.GetValue<UserDto>(NavigationParameterKeys._User);
            if (pickedUser != null)
            {
                IsInvitedUser = pickedUser.Id == null; // So we can show the information bubble 
                PickedUser = pickedUser;
            }

            // If the page was already populated, or if we are on the first form page, we stop here.
            if (CreateMissionRequest != null || base.GetType().Name == "CreateMissionContextViewModel")
                return;

            CreateMissionRequest = parameters.GetValue<CreateMissionRequestDto>(NavigationParameterKeys._CreateMissionRequest);
            if (CreateMissionRequest is null)
            {
                await DialogService.DisplayAlertAsync("Oops...", ErrorMessages.unknownError, "Ok");
                await NavigationService.NavigateAsync("CreateMissionStartView");
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // Used for member picker
            parameters.Add(NavigationParameterKeys._Organization, SelectedOrganization);

            base.OnNavigatedFrom(parameters);
        }

        protected virtual async Task OnPickUser()
        {
            var parameters = new NavigationParameters();
            parameters.Add(NavigationParameterKeys._Organization, SelectedOrganization);
            await NavigationService.NavigateAsync("MemberPickerView", parameters, useModalNavigation: true);
        }

        protected virtual void OnRemoveUser()
        {
            PickedUser = null;
        }
    }
}
