using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Activity.ViewModels
{
    public class ActivityDetailsViewModel : ViewModelBase
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public bool IsSignButtonVisible { get => _isSignButtonVisible; set { _isSignButtonVisible = value; RaisePropertyChanged(); } }
        private bool _isSignButtonVisible;

        public bool IsSaveButtonVisible { get => _isSaveButtonVisible; set { _isSaveButtonVisible = value; RaisePropertyChanged(); } }
        private bool _isSaveButtonVisible;

        public bool IsRefuseButtonVisible { get => _isRefuseButtonVisible; set { _isRefuseButtonVisible = value; RaisePropertyChanged(); } }
        private bool _isRefuseButtonVisible;

        public bool IsAcceptButtonVisible { get => _isAcceptButtonVisible; set { _isAcceptButtonVisible = value; RaisePropertyChanged(); } }
        private bool _isAcceptButtonVisible;

        public bool IsCommentVisible { get => _isCommentVisible; set { _isCommentVisible = value; RaisePropertyChanged(); } }
        private bool _isCommentVisible;

        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }
        private ActivityDto _activity;

        public DelegateCommand AcceptActivityCommand { get; set; }
        public DelegateCommand RefuseActivityCommand { get; set; }
        public DelegateCommand SignActivityCommand { get; set; }
        public DelegateCommand SaveActivityCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;

        public ActivityDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IActivityService activityService) : base(navigationService, mapper, logger, dialogService)
        {
            _activityService = activityService;

            AcceptActivityCommand = new DelegateCommand(async () => await OnAcceptActivity());
            RefuseActivityCommand = new DelegateCommand(async () => await OnRefuseActivity());
            SignActivityCommand = new DelegateCommand(async () => await OnSignActivity());
            SaveActivityCommand = new DelegateCommand(async () => await OnSaveActivity());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Activity = parameters.GetValue<ActivityDto>(NavigationParameterKeys._Activity);
            if (Activity is null)
                return;

            SetupUI();
        }

        private async Task OnSignActivity()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;
                Activity = Mapper.Map<ActivityDto>(await _activityService.SignActivityReport(AppSettings.CurrentUser, Mapper.Map<ActivityModel>(Activity)));
                SetupUI();
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

        private async Task OnSaveActivity()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;
                await _activityService.UpdateActivity(Mapper.Map<ActivityModel>(Activity));
                SetupUI();
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

        private async Task OnRefuseActivity()
        {
        }

        private async Task OnAcceptActivity()
        {
        }

        private void SetupUI()
        {
            if (Activity.Consultant.Id == AppSettings.CurrentUser.Id)
                SetupConsultantUI();
            else if (Activity.Commercial?.Id == AppSettings.CurrentUser.Id)
                SetupCommercialUI();
            else if (Activity.Customer.Id == AppSettings.CurrentUser.Id)
                SetupCustomerUI();
        }

        private void SetupConsultantUI()
        {
            IsAcceptButtonVisible = false;
            IsRefuseButtonVisible = false;
            IsSignButtonVisible = Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            IsSaveButtonVisible = Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.Generated;
        }
        private void SetupCustomerUI()
        {
            IsAcceptButtonVisible = Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            IsRefuseButtonVisible = true;
            IsSignButtonVisible = false;
            IsSaveButtonVisible = false;
        }

        private void SetupCommercialUI()
        {
        }

    }
}
