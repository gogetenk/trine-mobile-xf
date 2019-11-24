using AutoMapper;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Plugin.DownloadManager.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Essentials;
using static Trine.Mobile.Dto.UserDto;

namespace Modules.Consultant.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Bindings

        private ActivityDto _activity;
        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        private bool _isEmptyState;
        public bool IsEmptyState { get => _isEmptyState; set { _isEmptyState = value; RaisePropertyChanged(); } }

        private bool _isCalendarShown = true;
        public bool IsCalendarShown { get => _isCalendarShown; set { _isCalendarShown = value; RaisePropertyChanged(); } }

        private string _currentUser;
        public string CurrentUser { get => _currentUser; set { _currentUser = value; RaisePropertyChanged(); } }

        public DelegateCommand SignActivityCommand { get; set; }
        public DelegateCommand SaveActivityCommand { get; set; }
        public DelegateCommand<GridDayDto> AbsenceCommand { get; set; }
        public DelegateCommand DayClickedCommand { get; set; }
        public DelegateCommand HistoryCommand { get; set; }
        public DelegateCommand DownloadActivityCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;
        private readonly IMissionService _missionService;
        private readonly IDialogService _dialogService;
        private MissionModel _mission;

        public HomeViewModel(INavigationService navigationService, IMapper mapper, Prism.Logging.ILogger logger, IPageDialogService pageDialogService, IActivityService activityService, IMissionService missionService, IDialogService dialogService, IDownloadManager downloadManager) : base(navigationService, mapper, logger, pageDialogService)
        {
            _activityService = activityService;
            _missionService = missionService;
            _dialogService = dialogService;

            SignActivityCommand = new DelegateCommand(() => OnSignActivity());
            SaveActivityCommand = new DelegateCommand(async () => await OnSaveActivity());
            AbsenceCommand = new DelegateCommand<GridDayDto>((gridDay) => OnAbsenceSettingsOpened(gridDay as GridDayDto));
            DownloadActivityCommand = new DelegateCommand(async () => await OnDownloadActivity());
            HistoryCommand = new DelegateCommand(async () => await OnHistoryTapped());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await LoadActivity();
        }

        private async Task LoadActivity()
        {
            try
            {
                IsLoading = true;
                IsEmptyState = false;

                CurrentUser = Mapper.Map<UserDto>(AppSettings.CurrentUser).DisplayName;

                // Getting current mission
                _mission = await _missionService.GetConsultantCurrentMission(AppSettings.CurrentUser.Id);
                if (_mission is null)
                {
                    IsEmptyState = true;
                    IsCalendarShown = false;
                }
                //throw new BusinessException("Vous n'avez pas encore été invité à une mission, vous ne pourrez donc pas remplir de CRA pour le moment.");

                // Check if there's already an activity report for this month
                var activity = Mapper.Map<ActivityDto>(await _activityService.GetFromMissionAndMonth(_mission.Id, DateTime.UtcNow));

                // If not, we just generate a new empty one
                if (activity is null)
                    Activity = Mapper.Map<ActivityDto>(await _activityService.CreateActivity(_mission.Id, DateTime.UtcNow));
                else
                    Activity = activity;
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

        #region Absence management

        private void OnAbsenceSettingsOpened(GridDayDto gridDay)
        {
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._Absence, gridDay);
            _dialogService.ShowDialog("AbsenceDialogView", dialogParams, result => OnAbsenceSettingsClosed(result.Parameters));
        }

        public void OnAbsenceSettingsClosed(IDialogParameters parameters)
        {
            var updatedDay = parameters.GetValue<GridDayDto>(NavigationParameterKeys._Absence);
            if (updatedDay is null)
                return;

            if (updatedDay.Absence is null)
                return;

            var dayIndex = Activity.Days.FindIndex(x => x.Day == updatedDay.Day);
            Activity.Days[dayIndex] = updatedDay;
        }

        #endregion

        #region Signing management

        private void OnSignActivity()
        {
            _dialogService.ShowDialog("SignActivityDialogView", null, async result => await OnSignDialogClosed(result.Parameters));
        }

        public async Task OnSignDialogClosed(IDialogParameters result)
        {
            try
            {
                if (!result.GetValue<bool>(NavigationParameterKeys._IsActivitySigned))
                    return;

                if (IsLoading)
                    return;

                IsLoading = true;

                var bytes = result.GetValue<byte[]>(NavigationParameterKeys._Bytes);
                if (bytes is null)
                    throw new TechnicalException("Bytes should not be null");

                // If the activity doesnt exist yet, we create it
                await CreateActivityIfNeeded();

                var activity = Mapper.Map<ActivityDto>(await _activityService.SignActivityReport(AppSettings.CurrentUser, Mapper.Map<ActivityModel>(Activity), new MemoryStream(bytes)));
                if (activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

                Activity = Mapper.Map<ActivityDto>(activity);

                // Sending notif to customer
                // TODO: a faire dans le back
                SendNotificationToCustomer();

                // Tracking event
                Logger.TrackEvent("[Retention] User signed an activity.", new Dictionary<string, string> {
                    { "UserId", activity.Consultant.Id },
                    { "ActivityId", activity.Id },
                    { "UserName", $"{AppSettings.CurrentUser.Firstname} {AppSettings.CurrentUser.Lastname}"},
                    { "UserType", Enum.GetName(typeof(GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole) }
                });
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

        private async Task CreateActivityIfNeeded()
        {
            string id;
            if (string.IsNullOrEmpty(Activity.Id))
            {
                id = Mapper.Map<ActivityDto>(await _activityService.CreateActivity(_mission.Id, DateTime.UtcNow))?.Id;
                Activity.Id = id;
            }
        }

        private void SendNotificationToCustomer()
        {
            var notification = new Dictionary<string, object>();
            notification["contents"] = new Dictionary<string, string>() { { "en", "Un rapport d'activité vient de vous être soumis !" } };
            notification["include_external_user_ids"] = new List<string>() { _mission.Customer.Id };

            OneSignal.Current.PostNotification(notification, (responseSuccess) =>
            {
                var oneSignalDebugMessage = "Notification posted successful! Delayed by about 30 secounds to give you time to press the home button to see a notification vs an in-app alert.\n" + Json.Serialize(responseSuccess);
                Logger.Log(oneSignalDebugMessage, new Dictionary<string, string>() { { "Notifcation Data", Json.Serialize(notification) } });
            }, (responseFailure) =>
            {
                var oneSignalDebugMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
                Logger.Log(oneSignalDebugMessage, new Dictionary<string, string>() { { "Notifcation Data", Json.Serialize(notification) } });
            });
        }

        #endregion

        #region Saving management

        private async Task OnSaveActivity()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;

                // If the activity doesnt exist yet, we create it
                await CreateActivityIfNeeded();

                var activity = await _activityService.UpdateActivity(Mapper.Map<ActivityModel>(Activity));
                if (activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

                Activity = Mapper.Map<ActivityDto>(activity);

                // Tracking event
                Logger.TrackEvent("[Retention] User updated an activity.", new Dictionary<string, string> {
                    { "UserId", activity.Consultant.Id },
                    { "ActivityId", activity.Id },
                    { "UserName", $"{AppSettings.CurrentUser.Firstname} {AppSettings.CurrentUser.Lastname}"},
                    { "UserType", Enum.GetName(typeof(GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole) }
                });
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

        #endregion

        private async Task OnDownloadActivity()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;

                await Browser.OpenAsync($"{AppSettings.ApiUrls.FirstOrDefault().Value}/api/activities/{Activity.Id}/export", BrowserLaunchMode.SystemPreferred);
                //var file = _downloadManager.CreateDownloadFile($"{AppSettings.ApiUrls.FirstOrDefault().Value}/api/activities/{Activity.Id}/export");
                //_downloadManager.Start(file);
                //if (file is null)
                //throw new BusinessException("Une erreur s'est produite lors du téléchargement du CRA");
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

        private async Task OnHistoryTapped()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/ActivityHistoryView");
        }
    }
}
