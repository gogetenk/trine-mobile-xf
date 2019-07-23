using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Organization.ViewModels
{
    public class MemberPickerViewModel : MembersViewModelBase
    {
        public bool IsListEmpty { get => _isListEmpty; set { _isListEmpty = value; RaisePropertyChanged(); } }
        private bool _isListEmpty;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(); } }
        private string _email;

        private bool _isAddMemberLoading = false;
        public bool IsAddMemberLoading { get => _isAddMemberLoading; set { _isAddMemberLoading = value; RaisePropertyChanged(); AddMemberCommand.RaiseCanExecuteChanged(); } }


        public MemberPickerViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService, organizationService)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _navigatedFrom = parameters.GetValue<string>(NavigationParameterKeys._NavigatedFromUri);

            if (Members is null || !Members.Any())
                await LoadData();
        }

        protected override async Task OnSelectedMember(UserDto user)
        {
            if (user is null)
                return;

            if (string.IsNullOrEmpty(_navigatedFrom))
                return;

            // If we are in picker mode, we come back to the origin page, with the picked user as a param
            var parameters = new NavigationParameters();
            parameters.Add(NavigationParameterKeys._User, user);
            await NavigationService.GoBackAsync(parameters);
        }

        protected override void OnSearchChanged(string role, string searchText)
        {
            base.OnSearchChanged(role, searchText);

            IsListEmpty = !Members.Any();
        }

        protected override async Task OnAddMember()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                    return;

                IsAddMemberLoading = true;
                var request = new CreateInvitationRequestDto()
                {
                    InviterId = "5ca5ca8f26482d1254b85dc1", // TODO mocked
                    Mail = Email
                };

                var invite = await _organizationService.SendInvitation("5ca5cab077e80c1344dbafec", Mapper.Map<CreateInvitationRequestModel>(request));
                Email = string.Empty;
                var unknownUser = new UserDto()
                {
                    DisplayName = Email,
                    Mail = Email,
                    ProfilePicUrl = "http://upstarttheater.com/wp-content/uploads/2014/06/profile-pic.jpg"
                };
                await OnSelectedMember(unknownUser);
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
