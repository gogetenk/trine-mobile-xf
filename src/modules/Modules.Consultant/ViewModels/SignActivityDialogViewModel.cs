using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Consultant.ViewModels
{
    public class SignActivityDialogViewModel : ViewModelBase, IDialogAware
    {
        public ICommand SignCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public event Action<IDialogParameters> RequestClose;

        public SignActivityDialogViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            SignCommand = new DelegateCommand(() => OnSignActivity());
            CancelCommand = new DelegateCommand(() => RequestClose.Invoke(null));
        }

        private void OnSignActivity()
        {
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivitySigned, true);
            RequestClose.Invoke(dialogParams);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
