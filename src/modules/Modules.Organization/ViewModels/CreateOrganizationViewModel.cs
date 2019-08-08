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
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class CreateOrganizationViewModel : ViewModelBase
    {
        #region Bindings

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); CreateOrganizationCommand.RaiseCanExecuteChanged(); } }
        private bool _isLoading = false;

        public string IconUrl { get => _iconUrl; set { _iconUrl = value; RaisePropertyChanged(); } }
        private string _iconUrl;

        public string Name { get => _name; set { _name = value; RaisePropertyChanged(); } }
        private string _name;

        public DelegateCommand CreateOrganizationCommand { get; set; }

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

                var createdOrga = await _organizationService.CreateOrganization(Name, IconUrl);
                var navParams = new NavigationParameters();
                navParams.Add(NavigationParameterKeys._Organization, Mapper.Map<OrganizationDto>(createdOrga));
                await NavigationService.NavigateAsync("TrineNavigationPage/OrganizationDetailsView", navParams);
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
