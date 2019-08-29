using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Essentials;

namespace Modules.Organization.ViewModels
{
    public class AddMemberViewModel : ViewModelBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAccountService _accountService;

        #region Bindings 

        public DelegateCommand AddMemberCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand EmailUnfocusedCommand { get; set; }
        public ICommand ItemSelectedCommand { get; set; }

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }
        private string _email;

        private bool _isLoading = false;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }

        private bool _isAddMemberLoading = false;
        public bool IsAddMemberLoading { get => _isAddMemberLoading; set { _isAddMemberLoading = value; RaisePropertyChanged(); AddMemberCommand.RaiseCanExecuteChanged(); } }

        public float CaptionOpacity { get => _captionOpacity; set { _captionOpacity = value; RaisePropertyChanged(); } }
        private float _captionOpacity = 0;

        private ObservableCollection<InviteDto> _invites;
        public ObservableCollection<InviteDto> Invites { get => _invites; set { _invites = value; RaisePropertyChanged(); } }

        private InviteDto _selectedInvite;
        private PartialOrganizationDto _organization;

        public InviteDto SelectedInvite { get => _selectedInvite; set { _selectedInvite = value; RaisePropertyChanged(); } }

        #endregion


        public AddMemberViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService, IAccountService accountService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;
            _accountService = accountService;

            AddMemberCommand = new DelegateCommand(async () => await OnInviteMember(), () => !IsAddMemberLoading);
            EmailUnfocusedCommand = new DelegateCommand(async () => await OnEmailUnfocused());
            ItemSelectedCommand = new DelegateCommand(async () => await OnItemSelected());
            RefreshCommand = new DelegateCommand(async () => await LoadData());
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await base.InitializeAsync(parameters);

            _organization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
            if (_organization is null)
                await NavigationService.GoBackAsync();

            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                IsLoading = true;
                Invites = Mapper.Map<ObservableCollection<InviteDto>>(await _organizationService.GetInvites(_organization.Id)); // TODO Mocked
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

        private async Task OnItemSelected()
        {
            await Clipboard.SetTextAsync(SelectedInvite.Code.ToString());
        }

        private async Task OnEmailUnfocused()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    CaptionOpacity = 0f;
                    return;
                }

                var userToComplete = new RegisterUserDto()
                {
                    Email = Email
                };
                var exists = await _accountService.DoesUserExist(Mapper.Map<RegisterUserModel>(userToComplete));

                if (!exists)
                {
                    CaptionOpacity = 1f;
                    return;
                }

                CaptionOpacity = 0f;
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
        }

        private async Task OnInviteMember()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                    return;

                IsAddMemberLoading = true;
                var request = new CreateInvitationRequestDto()
                {
                    InviterId = AppSettings.CurrentUser.Id,
                    Mail = Email
                };

                var invite = await _organizationService.SendInvitation("5ca5cab077e80c1344dbafec", Mapper.Map<CreateInvitationRequestModel>(request));
                Invites.Insert(0, Mapper.Map<InviteDto>(invite));
                Email = string.Empty;
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
                IsAddMemberLoading = false;
            }
        }
    }
}
