using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Extensions;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class MissionActivityViewModel : ViewModelBase
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; RaisePropertyChanged(); OnSearchChanged(value); } }

        public ObservableCollection<ActivityDto> Activities { get => _activities; set { _activities = value; RaisePropertyChanged(); } }
        private ObservableCollection<ActivityDto> _activities;

        public ActivityDto SelectedActivity { get => _selectedActivity; set { _selectedActivity = value; RaisePropertyChanged(); OnActivitySelected(); } }
        private ActivityDto _selectedActivity;

        public ICommand RefreshCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;
        private readonly IDialogService _dialogService;
        private bool _hasBeenLoadedOnce;
        private MissionDto _mission;
        private List<ActivityDto> _totalActivities; // Liste non filtrée

        public MissionActivityViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService pageDialogService, IActivityService activityService, IDialogService dialogService) : base(navigationService, mapper, logger, pageDialogService)
        {
            _activityService = activityService;
            _dialogService = dialogService;
            RefreshCommand = new DelegateCommand(async () => await LoadData());
            AddMemberCommand = new DelegateCommand(() => OnCreateActivity());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // If we already have data, we dont reload each time we navigate on the tab
            if (_hasBeenLoadedOnce && Activities != null && Activities.Any())
                return;

            _mission = parameters.GetValue<MissionDto>(NavigationParameterKeys._Mission);
            if (_mission is null)
            {
                await NavigationService.GoBackAsync();
                return;
            }

            await LoadData();
            _hasBeenLoadedOnce = true;
        }

        private async Task LoadData()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;
                Activities = Mapper.Map<ObservableCollection<ActivityDto>>(await _activityService.GetFromMission(_mission.Id));
                _totalActivities = Activities?.ToList();
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

        private void OnCreateActivity()
        {
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._Mission, _mission);
            _dialogService.ShowDialog("PeriodChoiceDialogView", dialogParams, async result => await OnPeriodChoiceDialogClosed(result.Parameters));
        }

        private async Task OnPeriodChoiceDialogClosed(IDialogParameters parameters)
        {
            try
            {
                var period = parameters.GetValue<DateTime>(NavigationParameterKeys._Period);
                if (period == default)
                    return;

                IsLoading = true;
                var createdActivity = Mapper.Map<ActivityDto>(await _activityService.CreateActivity(_mission.Id, period));
                if (createdActivity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la création du rapport d'activité.");

                if (Activities is null)
                    Activities = new ObservableCollection<ActivityDto>();

                Activities.Add(createdActivity);
                _totalActivities = Activities.ToList();
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

        private void OnSearchChanged(string searchText)
        {
            if (_totalActivities is null || !_totalActivities.Any())
                return;

            Activities = new ObservableCollection<ActivityDto>(_totalActivities);
            if (!string.IsNullOrEmpty(searchText))
                Activities.RemoveAll(x => !x.StartDate.ToString("MMMM yyyy").ToLower().Contains(searchText.ToLower()));
        }

        private async Task OnActivitySelected()
        {
            if (SelectedActivity == null)
                return;

            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._Activity, SelectedActivity);
            await NavigationService.NavigateAsync("ActivityDetailsView", navparams);
        }

    }
}
