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
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Organization.ViewModels
{
    public class CreateOrganizationViewModel : ViewModelBase
    {
        #region Bindings

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading = false;

        public string IconUrl { get => _iconUrl; set { _iconUrl = value; RaisePropertyChanged(); } }
        private string _iconUrl;

        public string Name { get => _name; set { _name = value; RaisePropertyChanged(); } }
        private string _name;

        public ICommand CreateOrganizationCommand { get; set; }

        #endregion

        private readonly IOrganizationService _organizationService;


        public CreateOrganizationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IOrganizationService organizationService, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            _organizationService = organizationService;

            CreateOrganizationCommand = new DelegateCommand(async () => await OnCreateOrganization(), () => !IsLoading);
        }

        private async Task OnCreateOrganization()
        {
            IsLoading = true;

            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return;

                var id = await _organizationService.CreateOrganization(Name, IconUrl);
                await DialogService.DisplayAlertAsync("Vous avez créé votre organisation !", "Un peu de patience, le reste arrive bientôt...", "J'ai hâte !");
                //NavigateAbsoluteCommandExecute($"/BurgerMenuView/NavigationPage/OrganizationDashboardView?OrganizationId={id}");
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
