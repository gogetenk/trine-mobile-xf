using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContextViewModel : CreateMissionViewModelBase
    {
        #region Bindings

        private bool _isTitleEmptyErrorVisible = false;
        public bool IsTitleEmptyErrorVisible { get => _isTitleEmptyErrorVisible; set { _isTitleEmptyErrorVisible = value; RaisePropertyChanged(); } }

        private bool _isOrganizationErrorVisible = false;
        public bool IsOrganizationErrorVisible { get => _isOrganizationErrorVisible; set { _isOrganizationErrorVisible = value; RaisePropertyChanged(); } }

        private bool _isOrganizationPickerVisible = false;
        public bool IsOrganizationPickerVisible { get => _isOrganizationPickerVisible; set { _isOrganizationPickerVisible = value; RaisePropertyChanged(); } }


        private List<PartialOrganizationDto> _organizations;
        public List<PartialOrganizationDto> Organizations { get => _organizations; set { _organizations = value; RaisePropertyChanged(); } }

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); NextCommand.RaiseCanExecuteChanged(); } }

        #endregion

        private readonly IDashboardService _dashboardService;

        public CreateMissionContextViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IDashboardService dashboardService) : base(navigationService, mapper, logger, dialogService)
        {
            NextCommand = new DelegateCommand(async () => await OnNextStep());
            CreateMissionRequest = new CreateMissionRequestDto();
            CreateMissionRequest.IsTripartite = true; // Default

            _dashboardService = dashboardService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            SelectedOrganization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
            if (SelectedOrganization != null)
            {
                CreateMissionRequest.OrganizationId = SelectedOrganization.Id;
                return;
            }

            // Load all organizations 
            IsOrganizationPickerVisible = true;
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;
                Organizations = Mapper.Map<List<PartialOrganizationDto>>(await _dashboardService.GetUserOrganizations(AppSettings.CurrentUser.Id));
            }
            catch (BusinessException bExc)
            {
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OnNextStep()
        {
            IsOrganizationErrorVisible = SelectedOrganization is null;
            IsUserErrorVisible = PickedUser is null;
            IsTitleEmptyErrorVisible = string.IsNullOrEmpty(CreateMissionRequest.ProjectName);
            if (IsUserErrorVisible || IsTitleEmptyErrorVisible || IsOrganizationErrorVisible)
                return;

            CreateMissionRequest.Customer = PickedUser;

            var navigationParams = new NavigationParameters();
            navigationParams.Add(NavigationParameterKeys._CreateMissionRequest, CreateMissionRequest);
            navigationParams.Add(NavigationParameterKeys._Organization, SelectedOrganization);
            await NavigationService.NavigateAsync("CreateMissionDatesView", navigationParams);
        }
    }
}
