using AutoMapper;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Consultant.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Bindings

        private ActivityDto _activity;
        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        public DelegateCommand SignActivityCommand { get; set; }
        public DelegateCommand SaveActivityCommand { get; set; }
        public DelegateCommand<GridDayDto> AbsenceCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;
        private readonly IMissionService _missionService;
        private readonly IDialogService _dialogService;

        public HomeViewModel(INavigationService navigationService, IMapper mapper, Prism.Logging.ILogger logger, IPageDialogService pageDialogService, IActivityService activityService, IMissionService missionService, IDialogService dialogService) : base(navigationService, mapper, logger, pageDialogService)
        {
            _activityService = activityService;
            _missionService = missionService;
            _dialogService = dialogService;

            SignActivityCommand = new DelegateCommand(() => OnSignActivity());
            SaveActivityCommand = new DelegateCommand(async () => await OnSaveActivity());
            AbsenceCommand = new DelegateCommand<GridDayDto>((gridDay) => OnAbsenceSettingsOpened(gridDay as GridDayDto));
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

                // Getting current mission
                var mission = await _missionService.GetConsultantCurrentMission(AppSettings.CurrentUser.Id);
                if (mission is null)
                    throw new BusinessException("Vous n'avez pas encore été invité à une mission, vous ne pourrez donc pas remplir de CRA pour le moment.");

                // Check if there's already an activity report for this month
                var activities = Mapper.Map<List<ActivityDto>>(await _activityService.GetFromMissionAndMonth(mission.Id, DateTime.UtcNow));

                // If not, we just generate a new empty one
                if (Activity is null)
                    Activity = Mapper.Map<ActivityDto>(await _activityService.GenerateNewActivityReport());
                else
                    Activity = activities.OrderBy(x => x.StartDate).FirstOrDefault();

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

                var activity = Mapper.Map<ActivityDto>(await _activityService.SignActivityReport(AppSettings.CurrentUser, Mapper.Map<ActivityModel>(Activity)));
                if (activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

                Activity = Mapper.Map<ActivityDto>(activity);

                // Sending notif to customer
                // TODO: a faire dans le back
                SendNotificationToCustomer();
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

        private void SendNotificationToCustomer()
        {
            var notification = new Dictionary<string, object>();
            notification["contents"] = new Dictionary<string, string>() { { "fr", "Un rapport d'activité vient de vous être soumis !" } };
            notification["include_player_ids"] = new List<string>() { Activity.Customer.Id };

            OneSignal.Current.PostNotification(notification, (responseSuccess) =>
            {
                var oneSignalDebugMessage = "Notification posted successful! Delayed by about 30 secounds to give you time to press the home button to see a notification vs an in-app alert.\n" + Json.Serialize(responseSuccess);
            }, (responseFailure) =>
            {
                var oneSignalDebugMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
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
                var activity = await _activityService.UpdateActivity(Mapper.Map<ActivityModel>(Activity));
                if (activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

                Activity = Mapper.Map<ActivityDto>(activity);
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
    }
}
