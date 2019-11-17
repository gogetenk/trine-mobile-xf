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
using Xamarin.Essentials;

namespace Modules.Customer.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Bindings

        private List<ActivityDto> _activities;
        public List<ActivityDto> Activities { get => _activities; set { _activities = value; RaisePropertyChanged(); } }

        private ActivityDto _activity;
        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        public DelegateCommand<string> AcceptActivityCommand { get; set; }
        public DelegateCommand<string> RefuseActivityCommand { get; set; }
        public DelegateCommand<string> DownloadActivityCommand { get; set; }

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

                // Getting all active activities for the user
                Activities = Mapper.Map<List<ActivityDto>>(await _activityService.GetFromUser(AppSettings.CurrentUser.Id))
                    .Where(x => x.Status == Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned)
                    .ToList(); // TODO: faire une requete speciale pour ça
                var ids = Activities.Select(x => x.Id).ToList();
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
