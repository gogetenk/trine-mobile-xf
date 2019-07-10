using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Authentication.ViewModels
{
    public class ForgotPasswordConfirmationViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; set; }

        public ForgotPasswordConfirmationViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            LoginCommand = new DelegateCommand(async () => await OnLogin());
        }

        private async Task OnLogin()
        {
            await NavigationService.NavigateAsync("LoginView");
        }
    }
}
