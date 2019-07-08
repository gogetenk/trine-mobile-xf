using System;
using System.Threading.Tasks;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Organization.ViewModels
{
    public class JoinOrganizationViewModel : ViewModelBase
    {
        #region Bindings

        public DelegateCommand SendInvitationCode { get; set; }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading = false;

        public string InvitationCode { get => _invitationCode; set { _invitationCode = value; RaisePropertyChanged(); } }
        private string _invitationCode;

        #endregion

        private readonly IOrganizationService _organizationService;
        private readonly IPageDialogService _dialogService;


        public JoinOrganizationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger)
        {
            _dialogService = dialogService;
            _organizationService = organizationService;
            SendInvitationCode = new DelegateCommand(async () => await OnSendInvitationCode(), () => IsLoading);
        }

        private async Task OnSendInvitationCode()
        {
            try
            {
                if (string.IsNullOrEmpty(InvitationCode))
                    return;

                IsLoading = true;
                Guid.TryParse(InvitationCode, out Guid codeGuid);
                if (codeGuid == default(Guid))
                    throw new BusinessException("Le format du code d'invitation est incorrect. Veuillez vérifier le code entré.");

                var orgaId = await _organizationService.JoinOrganization(codeGuid);
                //NavigateAbsoluteCommandExecute($"/BurgerMenuView/NavigationPage/OrganizationDashboardView?OrganizationId={orgaId}");
            }
            catch (BusinessException bExc)
            {
                Logger.Log(bExc.Message);
                await _dialogService.DisplayAlertAsync(ErrorMessages.error, bExc.Message, "Ok");
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
