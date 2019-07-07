using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Authentication.ViewModels
{
    public class ForgotPasswordConfirmationViewModel : ViewModelBase
    {
        public ForgotPasswordConfirmationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
        {
        }
    }
}
