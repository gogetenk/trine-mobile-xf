using AutoMapper;
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
using static Trine.Mobile.Dto.UserDto;

namespace Modules.Customer.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Bindings

        private List<ActivityDto> _activities;
        public List<ActivityDto> Activities { get => _activities; set { _activities = value; RaisePropertyChanged(); } }

        private ActivityDto _activity;
        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        private string _currentUser;
        public string CurrentUser { get => _currentUser; set { _currentUser = value; RaisePropertyChanged(); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        private bool _isEmptyState;
        public bool IsEmptyState { get => _isEmptyState; set { _isEmptyState = value; RaisePropertyChanged(); } }

        public DelegateCommand<string> AcceptActivityCommand { get; set; }
        public DelegateCommand<string> RefuseActivityCommand { get; set; }
        public DelegateCommand HistoryCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;
        private readonly IDialogService _dialogService;

        public HomeViewModel(
            INavigationService navigationService,
            IMapper mapper,
            Prism.Logging.ILogger logger,
            IPageDialogService pageDialogService,
            IActivityService activityService,
            IDialogService dialogService)
            : base(navigationService, mapper, logger, pageDialogService)
        {
            _activityService = activityService;
            _dialogService = dialogService;
            RefuseActivityCommand = new DelegateCommand<string>((id) => OnRefuseActivity(id));
            AcceptActivityCommand = new DelegateCommand<string>((id) => OnAcceptActivity(id));
            HistoryCommand = new DelegateCommand(async () => await OnHistoryTapped());
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

                CurrentUser = Mapper.Map<UserDto>(AppSettings.CurrentUser).DisplayName;
                // Getting all active activities for the user
                Activities = Mapper.Map<List<ActivityDto>>(await _activityService.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.ConsultantSigned));
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

        #region Accepting management 

        private void OnAcceptActivity(string id)
        {
            // Setting the selected activity
            Activity = Activities.FirstOrDefault(x => x.Id == id);
            _dialogService.ShowDialog("AcceptActivityDialogView", null, async result => await OnSignDialogClosed(result.Parameters));
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

                Activities.Remove(Activity);
                Activity = null;

                // Tracking event
                Logger.TrackEvent("[Retention] User signed an activity.", new Dictionary<string, string> {
                    { "UserId", AppSettings.CurrentUser.Id },
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

        #region Refusing management

        private void OnRefuseActivity(string id)
        {
            // Setting the selected activity
            Activity = Activities.FirstOrDefault(x => x.Id == id);
            _dialogService.ShowDialog("RefuseActivityDialogView", null, async result => await OnRefuseDialogClosed(result.Parameters));
        }

        public async Task OnRefuseDialogClosed(IDialogParameters result)
        {
            try
            {
                if (!result.GetValue<bool>(NavigationParameterKeys._IsActivityRefused))
                    return;

                if (IsLoading)
                    return;

                var comment = result.GetValue<string>(NavigationParameterKeys._ActivityComment);

                IsLoading = true;
                Activity.ModificationProposals = new List<ModificationProposalDto>()
                {
                    new ModificationProposalDto()
                    {
                        Comment = comment,
                        CreationDate = DateTime.UtcNow
                    }
                };
                Activity.Consultant.SignatureDate = null;
                Activity.Customer.SignatureDate = null;
                Activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;
                //await _activityService.RefuseActivity(Mapper.Map<ActivityModel>(Activity));
                var activity = Mapper.Map<ActivityDto>(await _activityService.SaveActivityReport(Mapper.Map<ActivityModel>(Activity)));
                if (activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

                // Tracking event
                Logger.TrackEvent("[Retention] User refused an activity.", new Dictionary<string, string> {
                    { "UserId", AppSettings.CurrentUser.Id },
                    { "ActivityId", activity.Id },
                    { "UserName", $"{AppSettings.CurrentUser.Firstname} {AppSettings.CurrentUser.Lastname}"},
                    { "UserType", Enum.GetName(typeof(GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole) }
                });

                Activity = null;
                await LoadActivities();
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

        private async Task OnHistoryTapped()
        {
            await NavigationService.NavigateAsync("TrineNavigationPage/ActivityHistoryView");
        }
    }
}
