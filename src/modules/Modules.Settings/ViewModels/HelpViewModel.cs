using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Components.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public HelpViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            LegalCommand = new DelegateCommand(async () => await OnLegalCommand());
            ContactCommand = new DelegateCommand(() => OnContactCommand());
            BlogCommand = new DelegateCommand(async () => await OnBlogCommand());
            HelpCenterCommand = new DelegateCommand(() => OnHelpCenterCommand());
        }

        private void OnHelpCenterCommand()
        {
        }

        private async Task OnBlogCommand()
        {
            await Browser.OpenAsync("https://medium.com/trine-app", BrowserLaunchMode.SystemPreferred);
        }

        private void OnContactCommand()
        {
        }

        private async Task OnLegalCommand()
        {
            await DialogService.DisplayAlertAsync("Revenez bientôt !", "Nous n'avons pas encore terminé cette fonctionnalité :)", "Ok");
        }
    }
}
