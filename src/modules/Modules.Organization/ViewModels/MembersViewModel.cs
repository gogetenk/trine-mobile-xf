using AutoMapper;
using Prism;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class MembersViewModel : MembersViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        public MembersViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService, organizationService)
        {
        }

        // Triggered only during classical navigation
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _isUserPickerMode = parameters.GetValue<bool>(NavigationParameterKeys._IsUserPickerModeEnabled);
            _navigatedFrom = parameters.GetValue<string>(NavigationParameterKeys._NavigatedFromUri);

            if (Members is null || !Members.Any())
                await LoadData();
        }

        // Triggered only on tabbed pages
        protected virtual async void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
            await LoadData();
        }

        protected override async Task OnSelectedMember(UserDto user)
        {
            if (user is null)
                return;

            var parameters = new NavigationParameters();
            // we navigate to the member details
            parameters.Add(NavigationParameterKeys._User, user);
            parameters.Add(NavigationParameterKeys._OrganizationId, _organization.Id);
            await NavigationService.NavigateAsync("MemberDetailsView", parameters);
        }

        protected override async Task OnAddMember()
        {
            await NavigationService.NavigateAsync("AddMemberView");
        }
    }
}
