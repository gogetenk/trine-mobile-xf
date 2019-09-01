using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Components.Navigation;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionDatesViewModel : CreateMissionViewModelBase
    {
        #region Bindings 

        private bool _isStartDateNull = false;
        public bool IsStartDateNull { get => _isStartDateNull; set { _isStartDateNull = value; RaisePropertyChanged(); } }

        private bool _isEndDateNull = false;
        public bool IsEndDateNull { get => _isEndDateNull; set { _isEndDateNull = value; RaisePropertyChanged(); } }

        private bool _isStartDateSuperior = false;
        public bool IsStartDateSuperior { get => _isStartDateSuperior; set { _isStartDateSuperior = value; RaisePropertyChanged(); } }

        #endregion


        public CreateMissionDatesViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await base.InitializeAsync(parameters);

            if (CreateMissionRequest.StartDate == default(DateTime))
                CreateMissionRequest.StartDate = DateTime.UtcNow;

            if (CreateMissionRequest.EndDate == default(DateTime))
                CreateMissionRequest.EndDate = CreateMissionRequest.StartDate.AddMonths(3);
        }

        private async Task OnNextStep()
        {
            IsStartDateNull = CreateMissionRequest.StartDate == default(DateTime);
            IsEndDateNull = CreateMissionRequest.EndDate == default(DateTime);
            IsStartDateSuperior = CreateMissionRequest.StartDate > CreateMissionRequest.EndDate;
            CanGoNext = !IsStartDateNull && !IsStartDateSuperior && !IsEndDateNull;
            if (!CanGoNext)
                return;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._Organization, SelectedOrganization);
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            await NavigationService.NavigateAsync("CreateMissionConsultantView", navigationParams);
        }
    }
}
