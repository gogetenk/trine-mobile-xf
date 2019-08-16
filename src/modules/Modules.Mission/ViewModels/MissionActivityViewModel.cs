using AutoMapper;
using Prism;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
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
    public class MissionActivityViewModel : ViewModelBase, IActiveAware
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; RaisePropertyChanged(); OnSearchChanged(value); } }

        public ObservableCollection<ActivityDto> Activities { get => _activities; set { _activities = value; RaisePropertyChanged(); } }
        private ObservableCollection<ActivityDto> _activities;

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        public ICommand RefreshCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;
        private bool _hasBeenLoadedOnce;
        public event EventHandler IsActiveChanged;
        private MissionDto _mission;
        // Liste non filtrée
        protected List<ActivityDto> _totalActivities;

        public MissionActivityViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IActivityService activityService) : base(navigationService, mapper, logger, dialogService)
        {
            _activityService = activityService;

            RefreshCommand = new DelegateCommand(async () => await LoadData());
        }

        // Triggered only on tabbed pages
        protected virtual async void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);

            //// We dont load the data each time we navigate on the tab
            //if (_hasBeenLoadedOnce)
            //    return;

            //await LoadData();
            //_hasBeenLoadedOnce = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // We dont load the data each time we navigate on the tab
            //if (_hasBeenLoadedOnce)
            //    return;

            _mission = parameters.GetValue<MissionDto>(NavigationParameterKeys._Mission);
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
                Activities.RemoveAll(x => !x.StartDate.ToString("MMMM YYYY").ToLower().Contains(searchText.ToLower()));
        }
    }
}
