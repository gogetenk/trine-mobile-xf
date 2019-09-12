using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.ViewModels;
using Xamarin.Essentials;

namespace Modules.Settings.ViewModels
{
    public class HelpViewModel : ViewModelBase
    {
        #region Bindings 

        public ICommand BlogCommand { get; set; }
        public ICommand HelpCenterCommand { get; set; }
        public ICommand ContactCommand { get; set; }
        public ICommand LegalCommand { get; set; }

        #endregion

        private readonly ISupportService _supportService;

        public HelpViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, ISupportService supportService) : base(navigationService, mapper, logger, dialogService)
        {
            _supportService = supportService;

            LegalCommand = new DelegateCommand(async () => await OnLegalCommand());
            ContactCommand = new DelegateCommand(() => OnContactCommand());
            BlogCommand = new DelegateCommand(async () => await OnBlogCommand());
            HelpCenterCommand = new DelegateCommand(() => OnHelpCenterCommand());
        }

        private void OnHelpCenterCommand()
        {
            _supportService.ShowHelpCenter();
        }

        private async Task OnBlogCommand()
        {
            await Browser.OpenAsync("https://medium.com/trine-app", BrowserLaunchMode.SystemPreferred);
        }

        private void OnContactCommand()
        {
            _supportService.ShowMessenger();
        }

        private async Task OnLegalCommand()
        {
            await DialogService.DisplayAlertAsync("Revenez bientôt !", "Nous n'avons pas encore terminé cette fonctionnalité :)", "Ok");
        }
    }
}
