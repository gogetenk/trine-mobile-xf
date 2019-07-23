using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Extensions;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public abstract class MembersViewModelBase : ViewModelBase
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

        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand AddMemberCommand { get; set; }

        #endregion

        protected readonly IOrganizationService _organizationService;
        protected PartialOrganizationDto _organization;
        // Liste non filtrée
        protected List<UserDto> _totalMemberList;
        protected bool _isUserPickerMode;
        protected string _navigatedFrom;


        public MembersViewModelBase(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;

            AddMemberCommand = new DelegateCommand(async () => await OnAddMember());
            RefreshCommand = new DelegateCommand(async () => await LoadData());
        }

        protected abstract Task OnSelectedMember(UserDto user);
        protected abstract Task OnAddMember();

        protected virtual void OnSearchChanged(string role, string searchText)
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

        protected virtual async Task LoadData()
        {
            try
            {
                IsLoading = true;

                _organization = Mapper.Map<PartialOrganizationDto>(await _organizationService.GetById("5ca5cab077e80c1344dbafec"));// TODO Mocked
                if (_organization is null)
                    return; // TODO : Que faire?

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

        protected virtual async Task OnRefresh()
        {
            await LoadData();
        }
    }
}
