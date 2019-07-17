using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Organization.ViewModels
{
    public class MemberDetailsViewModel : ViewModelBase
    {
        #region Properties 

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        private OrganizationMemberDto _member;
        public OrganizationMemberDto Member { get => _member; set { _member = value; RaisePropertyChanged(); } }

        #endregion

        private readonly IOrganizationService _organizationService;
        private string _organizationId;


        public MemberDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;

            SaveCommand = new DelegateCommand(async () => await OnSave());
            DeleteCommand = new DelegateCommand(async () => await OnDelete());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _organizationId = parameters.GetValue<string>(NavigationParameterKeys._OrganizationId);
            var user = parameters.GetValue<UserDto>(NavigationParameterKeys._User);
            await LoadData(user.Id);
        }

        private async Task LoadData(string userId)
        {
            try
            {
                IsLoading = true;
                Member = Mapper.Map<OrganizationMemberDto>(await _organizationService.GetMember(_organizationId, userId));
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

        private async Task OnSave()
        {
            try
            {
                IsLoading = true;
                await _organizationService.UpdateMember(_organizationId, Mapper.Map<OrganizationMemberModel>(Member));
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

        private async Task OnDelete()
        {
            try
            {
                IsLoading = true;
                await _organizationService.RemoveMember(_organizationId, Member.UserId);
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

    }
}
