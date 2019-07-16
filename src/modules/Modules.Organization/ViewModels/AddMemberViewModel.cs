using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Organization.ViewModels
{
    public class AddMemberViewModel : ViewModelBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAccountService _accountService;

        #region Bindings 

        public ICommand AddMember { get; set; }
        public ICommand EmailUnfocusedCommand { get; set; }

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }
        private string _email;

        public float CaptionOpacity { get => _captionOpacity; set { _captionOpacity = value; RaisePropertyChanged(); } }
        private float _captionOpacity = 0;

        private ObservableCollection<InviteDto> _invites;
        public ObservableCollection<InviteDto> Invites { get => _invites; set { _invites = value; RaisePropertyChanged(); } }

        #endregion

        public AddMemberViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService, IAccountService accountService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;
            _accountService = accountService;

            AddMember = new DelegateCommand(async () => await OnInviteMember());
            EmailUnfocusedCommand = new DelegateCommand(async () => await OnEmailUnfocused());
        }

        private async Task OnEmailUnfocused()
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


        private async Task OnInviteMember()
        {
            var request = new CreateInvitationRequestDto()
            {
                InviterId = "5ca5ca8f26482d1254b85dc1", // TODO mocked
                Mail = Email
            };
            var invite = await _organizationService.SendInvitation(Mapper.Map<CreateInvitationRequestModel>(request));
            Invites.Add(Mapper.Map<InviteDto>(invite));
        }
    }
}
