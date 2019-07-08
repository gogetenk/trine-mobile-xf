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
        private readonly IPageDialogService _dialogService;


        public CreateOrganizationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IOrganizationService organizationService, IPageDialogService dialogService) : base(navigationService, mapper, logger)
        {
            _organizationService = organizationService;
            _dialogService = dialogService;

            CreateOrganizationCommand = new DelegateCommand(async () => await OnCreateOrganization(), () => !IsLoading);
        }

        private async Task OnCreateOrganization()
        {
            if (string.IsNullOrEmpty(Name))
                throw new BusinessException("Name cannot be null");

            IsLoading = true;

            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return;

                var id = await _organizationService.CreateOrganization(Name, IconUrl);
                //NavigateAbsoluteCommandExecute($"/BurgerMenuView/NavigationPage/OrganizationDashboardView?OrganizationId={id}");
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
