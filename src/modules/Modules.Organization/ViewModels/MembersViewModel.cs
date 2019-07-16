using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Extensions;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class MembersViewModel : ViewModelBase
    {
        

        #region Properties 

        // Liste filtrée par le picker
        private ObservableCollection<UserDto> _members;
        public ObservableCollection<UserDto> Members { get => _members; set { _members = value; RaisePropertyChanged(); } }

        private UserDto _selectedMember;
        public UserDto SelectedMember { get => _selectedMember; set { _selectedMember = value; RaisePropertyChanged(); OnSelectedMember(value); } }

        private List<string> _roles;
        public List<string> Roles { get => _roles; set { _roles = value; RaisePropertyChanged(); } }

        private string _selectedRole;
        public string SelectedRole { get => _selectedRole; set { _selectedRole = value; RaisePropertyChanged(); OnSearchChanged(value, SearchText); } }

        private int _roleSelectedIndex;
        public int RoleSelectedIndex { get => _roleSelectedIndex; set { _roleSelectedIndex = value; RaisePropertyChanged(); } }

        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; RaisePropertyChanged(); OnSearchChanged(SelectedRole, value); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public string OrganizationName { get; set; } = "Panda Services"; // TODO mocked
        public string PageTitle { get; set; } = "Membres"; // TODO mocked

        public ICommand RefreshCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }

        #endregion

        private readonly IOrganizationService _organizationService;
        private PartialOrganizationDto _organization;
        // Liste non filtrée
        private List<UserDto> _totalMemberList;

        public MembersViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;
            AddMemberCommand = new DelegateCommand(async () => await OnAddMember());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _organization = Mapper.Map<PartialOrganizationDto>(await _organizationService.GetById("5ca5cab077e80c1344dbafec"));// TODO Mocked
            //_organization = parameters.GetValue<PartialOrganizationDto>("Organization"); // TODO Mocked
            if (_organization is null)
                return; // TODO : Que faire?

            await LoadData();
        }

        private async Task OnAddMember()
        {
            await NavigationService.NavigateAsync("AddMemberView");
        }

        private async Task LoadData()
        {
            try
            {
                IsLoading = true;

                var t = await _organizationService.GetOrganizationMembers(_organization.Id);
                var list = Mapper.Map<ObservableCollection<UserDto>>(t);

                _totalMemberList = list.ToList();
                Members = list;
                Roles = Members.Select(x => x.Role).ToList();
                // On ajoute le champ vide
                Roles.Insert(0, "");
                SelectedRole = null;
                RoleSelectedIndex = -1;
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

        private void OnSearchChanged(string role, string searchText)
        {
            if (_totalMemberList is null)
                return;

            if (string.IsNullOrEmpty(role))
                RoleSelectedIndex = -1;

            Members = new ObservableCollection<UserDto>(_totalMemberList);

            if (!string.IsNullOrEmpty(role))
                Members.RemoveAll(x => x.Role != role);

            if (!string.IsNullOrEmpty(searchText))
                Members.RemoveAll(x => !x.DisplayName.ToLower().Contains(searchText.ToLower()));
        }

        private async Task OnRefresh()
        {
            await LoadData();
        }

        private void OnSelectedMember(UserDto user)
        {
            if (user is null)
                return;

            var parameters = new NavigationParameters();
            parameters.Add("User", user);
            parameters.Add("Organization", _organization);
            NavigationService.NavigateAsync("MemberDetailsView", parameters);
        }
    }
}
