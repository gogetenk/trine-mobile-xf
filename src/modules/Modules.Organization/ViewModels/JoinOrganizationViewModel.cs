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

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); SendInvitationCode.RaiseCanExecuteChanged(); } }
        private bool _isLoading = false;

        public string InvitationCode { get => _invitationCode; set { _invitationCode = value; RaisePropertyChanged(); } }
        private string _invitationCode;

        #endregion

        private readonly IOrganizationService _organizationService;

        public JoinOrganizationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IOrganizationService organizationService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;
            SendInvitationCode = new DelegateCommand(async () => await OnSendInvitationCode(), () => IsLoading);
        }

        private async Task OnSendInvitationCode()
        {
            if (string.IsNullOrEmpty(InvitationCode))
                return;

            try
            {
                IsLoading = true;

                Guid.TryParse(InvitationCode, out Guid codeGuid);
                if (codeGuid == default(Guid))
                    throw new BusinessException("Le format du code d'invitation est incorrect. Veuillez vérifier le code entré.");

                var orgaId = await _organizationService.JoinOrganization(codeGuid);
                await DialogService.DisplayAlertAsync("Vous avez rejoint une organisation !", "Un peu de patience, le reste arrive bientôt...", "J'ai hâte !");
                //NavigateAbsoluteCommandExecute($"/BurgerMenuView/NavigationPage/OrganizationDashboardView?OrganizationId={orgaId}");
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
