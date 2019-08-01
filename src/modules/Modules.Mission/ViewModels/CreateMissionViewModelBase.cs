using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public abstract class CreateMissionViewModelBase : ViewModelBase
    {
        #region Bindings 

        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PickUserCommand { get; set; }
        public ICommand RemovedUserCommand { get; set; }

        private CreateMissionRequestDto _createMissionRequest;
        public CreateMissionRequestDto CreateMissionRequest { get => _createMissionRequest; set { _createMissionRequest = value; RaisePropertyChanged(); } }

        private bool _canGoNext = true;
        public bool CanGoNext { get => _canGoNext; set { _canGoNext = value; RaisePropertyChanged(); } }

        private UserDto _pickedUser;
        public UserDto PickedUser { get => _pickedUser; set { _pickedUser = value; RaisePropertyChanged(); } }

        private bool _isUserErrorVisible = false;
        public bool IsUserErrorVisible { get => _isUserErrorVisible; set { _isUserErrorVisible = value; RaisePropertyChanged(); } }

        #endregion


        public CreateMissionViewModelBase(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            PickUserCommand = new DelegateCommand(async () => await OnPickUser());
            RemovedUserCommand = new DelegateCommand(() => OnRemoveUser());
            BackCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            PickedUser = parameters.GetValue<UserDto>(NavigationParameterKeys._User);

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

        protected virtual async Task OnPickUser()
        {
            await NavigationService.NavigateAsync("MemberPickerView", useModalNavigation: true);
        }

        protected virtual void OnRemoveUser()
        {
            PickedUser = null;
        }
    }
}
