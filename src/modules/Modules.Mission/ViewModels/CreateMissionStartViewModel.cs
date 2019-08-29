using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionStartViewModel : ViewModelBase
    {
        #region Bindings 

        public ICommand StartCommand { get; set; }

        #endregion

        private PartialOrganizationDto _partialOrganization;

        public CreateMissionStartViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            StartCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("CreateMissionContextView"));
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await base.InitializeAsync(parameters);

            _partialOrganization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(NavigationParameterKeys._Organization, _partialOrganization);

            base.OnNavigatedFrom(parameters);
        }
    }
}
