using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Xamarin.Essentials;
using static Trine.Mobile.Dto.UserDto;

namespace Modules.Consultant.ViewModels
{
    public class ActivityHistoryViewModel : ViewModelBase
    {
        #region Bindings

        private List<ActivityDto> _activities;
        public List<ActivityDto> Activities { get => _activities; set { _activities = value; RaisePropertyChanged(); } }

        private ActivityDto _activity;
        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        private bool _isEmptyState;
        public bool IsEmptyState { get => _isEmptyState; set { _isEmptyState = value; RaisePropertyChanged(); } }

        public DelegateCommand<string> DownloadActivityCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;

        public ActivityHistoryViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IActivityService activityService) : base(navigationService, mapper, logger, dialogService)
        {
            _activityService = activityService;

            DownloadActivityCommand = new DelegateCommand<string>(async (id) => await OnDownloadActivity(id));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await LoadActivities();
        }

        private async Task LoadActivities()
        {
            try
            {
                IsLoading = true;
                IsEmptyState = false;

                // Getting all signed activities for the user
                Activities = Mapper.Map<List<ActivityDto>>(await _activityService.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.CustomerSigned));
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

                if (Activities is null || !Activities.Any())
                    IsEmptyState = true;
            }
        }

        private async Task OnDownloadActivity(string id)
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;

                await Browser.OpenAsync($"{AppSettings.ApiUrls.FirstOrDefault().Value}/api/activities/{id}/export", BrowserLaunchMode.SystemPreferred);
                //var file = _downloadManager.CreateDownloadFile($"{AppSettings.ApiUrls.FirstOrDefault().Value}/api/activities/{Activity.Id}/export");
                //_downloadManager.Start(file);
                //if (file is null)
                //throw new BusinessException("Une erreur s'est produite lors du téléchargement du CRA");

                // Tracking event
                Logger.TrackEvent("[Retention] User downloaded an activity.", new Dictionary<string, string> {
                    { "UserId", AppSettings.CurrentUser.Id },
                    { "ActivityId", id },
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

    }
}
