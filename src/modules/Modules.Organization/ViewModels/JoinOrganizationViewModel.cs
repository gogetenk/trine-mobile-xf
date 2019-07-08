using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Organization.ViewModels
{
    public class JoinOrganizationViewModel : ViewModelBase
    {
        public JoinOrganizationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
        {
        }
    }
}
