using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContextViewModel : ViewModelBase
    {
        #region Bindings

        public ICommand NextCommand { get; set; }
        public ICommand PickCustomerCommand { get; set; }
        public ICommand RemovedCustomerCommand { get; set; }

        public bool IsClientErrorVisible { get; set; } = false;
        public bool IsTitleEmptyErrorVisible { get; set; } = false;

        private string _title;
        public string Title { get => _title; set { _title = value; RaisePropertyChanged(); } }

        private bool _canGoNext = true;
        public bool CanGoNext { get => _canGoNext; set { _canGoNext = value; RaisePropertyChanged(); } }

        private UserDto _pickedUser;
        public UserDto PickedUser { get => _pickedUser; set { _pickedUser = value; RaisePropertyChanged(); } }

        #endregion

        public CreateMissionContextViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
            PickCustomerCommand = new DelegateCommand(async () => await OnPickCustomer());
            RemovedCustomerCommand = new DelegateCommand(() => OnRemoveCustomer());
        }

        private async Task OnNextStep()
        {
            IsClientErrorVisible = PickedUser is null;
            IsTitleEmptyErrorVisible = string.IsNullOrEmpty(Title);
            CanGoNext = IsClientErrorVisible || IsTitleEmptyErrorVisible;
            if (IsClientErrorVisible || IsTitleEmptyErrorVisible)
                return;

            var createMissionRequest = new CreateMissionRequestDto();
            createMissionRequest.ProjectName = Title;
            createMissionRequest.Customer = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, createMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionDatesView", navigationParams, useModalNavigation: false);
        }

        private async Task OnPickCustomer()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._IsUserPickerModeEnabled, true);
            navigationParams.Add(NavigationParameterKeys._NavigatedFromUri, "CreateMissionContextView");

            await NavigationService.NavigateAsync("MemberPickerView", navigationParams, useModalNavigation: true);
        }

        private void OnRemoveCustomer()
        {
            PickedUser = null;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            PickedUser = parameters.GetValue<UserDto>(NavigationParameterKeys._User);
        }
    }
}
