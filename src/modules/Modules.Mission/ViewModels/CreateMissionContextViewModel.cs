using System.Threading.Tasks;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContextViewModel : CreateMissionViewModelBase
    {
        #region Bindings

        public bool IsTitleEmptyErrorVisible { get; set; } = false;

        private string _title;
        public string Title { get => _title; set { _title = value; RaisePropertyChanged(); } }

        #endregion

        public CreateMissionContextViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
        }

        private async Task OnNextStep()
        {
            IsUserErrorVisible = PickedUser is null;
            IsTitleEmptyErrorVisible = string.IsNullOrEmpty(Title);
            CanGoNext = IsUserErrorVisible || IsTitleEmptyErrorVisible;
            if (IsUserErrorVisible || IsTitleEmptyErrorVisible)
                return;

            var createMissionRequest = new CreateMissionRequestDto();
            createMissionRequest.ProjectName = Title;
            createMissionRequest.Customer = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, createMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionDatesView", navigationParams, useModalNavigation: false);
        }

    }
}
