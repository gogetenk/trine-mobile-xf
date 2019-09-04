using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Forms;

namespace Modules.Activity.ViewModels
{
    public class ActivityDetailsViewModel : ViewModelBase
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public bool CanModify { get => _canModify; set { _canModify = value; RaisePropertyChanged(); } }
        private bool _canModify;

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

        public Color CustomerSignedTextColor { get => _customerSignedTextColor; set { _customerSignedTextColor = value; RaisePropertyChanged(); } }
        private Color _customerSignedTextColor = Color.FromHex("#F0B429");

        public Color ConsultantSignedTextColor { get => _consultantSignedTextColor; set { _consultantSignedTextColor = value; RaisePropertyChanged(); } }
        private Color _consultantSignedTextColor = Color.FromHex("#F0B429");

        public string ConsultantSignedStatusText { get => _consultantSignedStatusText; set { _consultantSignedStatusText = value; RaisePropertyChanged(); } }
        private string _consultantSignedStatusText;
        public string CustomerSignedStatusText { get => _customerSignedStatusText; set { _customerSignedStatusText = value; RaisePropertyChanged(); } }
        private string _customerSignedStatusText;

        public string ConsultantGlyph { get => _consultantGlyph; set { _consultantGlyph = value; RaisePropertyChanged(); } }
        private string _consultantGlyph;

        public string CustomerGlyph { get => _customerGlyph; set { _customerGlyph = value; RaisePropertyChanged(); } }
        private string _customerGlyph;

        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }
        private ActivityDto _activity;

        public DelegateCommand AcceptActivityCommand { get; set; }
        public DelegateCommand RefuseActivityCommand { get; set; }
        public DelegateCommand SignActivityCommand { get; set; }
        public DelegateCommand SaveActivityCommand { get; set; }
        public DelegateCommand<GridDayDto> AbsenceCommand { get; set; }

        #endregion

        private readonly IActivityService _activityService;
        private readonly IDialogService _dialogService;

        public ActivityDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService pageDialogService, IActivityService activityService, IDialogService dialogService) : base(navigationService, mapper, logger, pageDialogService)
        {
            _activityService = activityService;
            _dialogService = dialogService;
            AcceptActivityCommand = new DelegateCommand(async () => await OnAcceptActivity());
            RefuseActivityCommand = new DelegateCommand(() => OnRefuseActivity());
            SignActivityCommand = new DelegateCommand(() => OnSignActivity());
            SaveActivityCommand = new DelegateCommand(async () => await OnSaveActivity());
            AbsenceCommand = new DelegateCommand<GridDayDto>((gridDay) => OnAbsenceSettingsOpened(gridDay as GridDayDto));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Activity = parameters.GetValue<ActivityDto>(NavigationParameterKeys._Activity);
            if (Activity is null)
                return;

            SetupUI();
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

            var activityDays = Activity.Days.Where(x => x.Day != updatedDay.Day).ToList();
            activityDays.Add(updatedDay);
            Activity.Days = activityDays;
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

                Activity = Mapper.Map<ActivityDto>(await _activityService.SignActivityReport(AppSettings.CurrentUser, Mapper.Map<ActivityModel>(Activity)));
                if (Activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

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

        #endregion

        #region Refusing management

        private void OnRefuseActivity()
        {
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
                Activity.ModificationProposals = new System.Collections.Generic.List<ModificationProposalDto>()
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
                Activity = Mapper.Map<ActivityDto>(await _activityService.SaveActivityReport(Mapper.Map<ActivityModel>(Activity)));
                if (Activity is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du CRA");

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

        #endregion

        #region Accepting management 

        private async Task OnAcceptActivity()
        {
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

        #endregion

        private void SetupUI()
        {
            if (Activity.Consultant.Id == AppSettings.CurrentUser.Id)
                SetupConsultantUI();
            else if (Activity.Commercial?.Id == AppSettings.CurrentUser.Id)
                SetupCommercialUI();
            else if (Activity.Customer.Id == AppSettings.CurrentUser.Id)
                SetupCustomerUI();

            if (Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.Generated)
            {
                ConsultantSignedTextColor = Color.FromHex(UIConstants._Yellow); // Yellow
                CustomerSignedTextColor = Color.FromHex(UIConstants._Yellow);
                ConsultantSignedStatusText = "En attente de signature";
                CustomerSignedStatusText = "En attente de signature";
                ConsultantGlyph = UIConstants._PendingGlyph;
                CustomerGlyph = UIConstants._PendingGlyph;
                IsCommentVisible = false;
            }
            else if (Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned)
            {
                ConsultantSignedTextColor = Color.FromHex(UIConstants._Green);  // Green
                CustomerSignedTextColor = Color.FromHex(UIConstants._Yellow);
                ConsultantSignedStatusText = $"Signé le {Activity.Consultant.SignatureDate}";
                CustomerSignedStatusText = "En attente de signature";
                ConsultantGlyph = UIConstants._SignedGlyph;
                CustomerGlyph = UIConstants._PendingGlyph;
                IsCommentVisible = false;
            }
            else if (Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired)
            {
                ConsultantSignedTextColor = Color.FromHex(UIConstants._Yellow);
                CustomerSignedTextColor = Color.FromHex(UIConstants._Red); // Red
                ConsultantSignedStatusText = "En attente de modification";
                CustomerSignedStatusText = "Refusé";
                ConsultantGlyph = UIConstants._PendingGlyph;
                CustomerGlyph = UIConstants._RefusedGlyph;
                IsCommentVisible = true;
            }
            else if (Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.CustomerSigned)
            {
                ConsultantSignedTextColor = Color.FromHex(UIConstants._Green);
                CustomerSignedTextColor = Color.FromHex(UIConstants._Green);
                ConsultantSignedStatusText = $"Signé le {Activity.Consultant.SignatureDate}";
                CustomerSignedStatusText = $"Signé le {Activity.Customer.SignatureDate}";
                ConsultantGlyph = UIConstants._SignedGlyph;
                CustomerGlyph = UIConstants._SignedGlyph;
                IsCommentVisible = false;
            }
        }

        private void SetupConsultantUI()
        {
            IsAcceptButtonVisible = false;
            IsRefuseButtonVisible = false;
            CanModify = Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.Generated || Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;
            IsSignButtonVisible = CanModify;
            IsSaveButtonVisible = CanModify;
        }
        private void SetupCustomerUI()
        {
            IsAcceptButtonVisible = Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            IsRefuseButtonVisible = Activity.Status == Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            IsSignButtonVisible = false;
            IsSaveButtonVisible = false;
            CanModify = false;
        }
        private void SetupCommercialUI()
        {
            IsAcceptButtonVisible = false;
            IsRefuseButtonVisible = false;
            IsSignButtonVisible = false;
            IsSaveButtonVisible = false;
            CanModify = false;
        }

    }
}
