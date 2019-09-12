using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Windows.Input;
using Trine.Mobile.Components.ViewModels;

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
            LegalCommand = new DelegateCommand(() => OnLegalCommand());
            ContactCommand = new DelegateCommand(() => OnContactCommand());
            BlogCommand = new DelegateCommand(() => OnBlogCommand());
            HelpCenterCommand = new DelegateCommand(() => OnHelpCenterCommand());
        }

        private void OnHelpCenterCommand()
        {
        }

        private void OnBlogCommand()
        {
        }

        private void OnContactCommand()
        {
        }

        private void OnLegalCommand()
        {
        }
    }
}
