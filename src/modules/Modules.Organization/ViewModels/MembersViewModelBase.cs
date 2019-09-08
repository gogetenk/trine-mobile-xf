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
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Extensions;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public abstract class MembersViewModelBase : ViewModelBase
    {
        #region Properties 

        public bool IsListEmpty { get => _isListEmpty; set { _isListEmpty = value; RaisePropertyChanged(); } }
        private bool _isListEmpty;

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

        public bool IsActionButtonShown { get => _isActionButtonShown; set { _isActionButtonShown = value; RaisePropertyChanged(); } }
        private bool _isActionButtonShown;

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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _organization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
            if (_organization is null)
                await NavigationService.GoBackAsync();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(NavigationParameterKeys._Organization, _organization);
            base.OnNavigatedFrom(parameters);
        }

        protected abstract Task OnSelectedMember(UserDto user);
        protected abstract Task OnAddMember();

        protected virtual void OnSearchChanged(string role, string searchText)
        {
            if (_totalMemberList is null || !_totalMemberList.Any())
                return;

            if (string.IsNullOrEmpty(role))
                RoleSelectedIndex = -1;

            Members = new ObservableCollection<UserDto>(_totalMemberList);

            if (!string.IsNullOrEmpty(role))
                Members.RemoveAll(x => x.Role != role);

            if (!string.IsNullOrEmpty(searchText))
                Members.RemoveAll(x => string.IsNullOrEmpty(x.DisplayName) || !x.DisplayName.ToLower().Contains(searchText.ToLower()));

            IsListEmpty = !Members.Any();
        }

        protected virtual async Task LoadData()
        {
            try
            {
                IsLoading = true;

                //_organization = Mapper.Map<PartialOrganizationDto>(await _organizationService.GetById(_organization.Id));// TODO Mocked
                //if (_organization is null)
                //    return; // TODO : Que faire?

                //if (BlobCache.ApplicationName == "TrineUnitTests")
                //{
                //    RefreshUI(await _organizationService.GetOrganizationMembers(_organization.Id));
                //    return;
                //}

                //BlobCache.LocalMachine.GetAndFetchLatest(
                //        "MemberList",
                //        async () => await _organizationService.GetOrganizationMembers(_organization.Id),
                //        null,
                //        null,
                //        true
                //    ).Subscribe(members =>
                //    {
                //        RefreshUI(members);
                //    });

                RefreshUI(await _organizationService.GetOrganizationMembers(_organization.Id));
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

        private void RefreshUI(List<Trine.Mobile.Model.UserModel> members)
        {
            Members = Mapper.Map<ObservableCollection<UserDto>>(members);
            IsListEmpty = !Members.Any();
            _totalMemberList = Members.ToList();
            Roles = Members.Select(x => x.Role).ToList();
            // On ajoute le champ vide
            Roles.Insert(0, "");
            SelectedRole = null;
            RoleSelectedIndex = -1;
        }

        protected virtual async Task OnRefresh()
        {
            await LoadData();
        }
    }
}
