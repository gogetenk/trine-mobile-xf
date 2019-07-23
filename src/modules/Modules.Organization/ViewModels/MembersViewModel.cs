using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class MembersViewModel : MembersViewModelBase
    {
        #region Properties 

        public ICommand AddMemberCommand { get; set; }

        #endregion


        public MembersViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService, organizationService)
        {
            AddMemberCommand = new DelegateCommand(async () => await OnAddMember());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _isUserPickerMode = parameters.GetValue<bool>(NavigationParameterKeys._IsUserPickerModeEnabled);
            _navigatedFrom = parameters.GetValue<string>(NavigationParameterKeys._NavigatedFromUri);

            if (Members is null || !Members.Any())
                await LoadData();
        }


        protected override async Task OnSelectedMember(UserDto user)
        {
            if (user is null)
                return;

            var parameters = new NavigationParameters();

            // If we are in picker mode, we come back to the origin page
            if (_isUserPickerMode && !string.IsNullOrEmpty(_navigatedFrom))
            {
                parameters.Add(NavigationParameterKeys._User, user);
                await NavigationService.GoBackAsync(parameters);
            }
            else // else we navigate to the member details
            {
                parameters.Add(NavigationParameterKeys._User, user);
                parameters.Add(NavigationParameterKeys._OrganizationId, _organization.Id);
                await NavigationService.NavigateAsync("MemberDetailsView", parameters);
            }
        }


        private async Task OnAddMember()
        {
            await NavigationService.NavigateAsync("AddMemberView");
        }
    }
}
