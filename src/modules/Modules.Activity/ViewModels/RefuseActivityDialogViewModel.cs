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

namespace Modules.Activity.ViewModels
{
    public class RefuseActivityDialogViewModel : ViewModelBase, IDialogAware
    {
        public string Comment { get => _comment; set { _comment = value; RaisePropertyChanged(); } }
        private string _comment;

        public ICommand SendCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event Action<IDialogParameters> RequestClose;

        public RefuseActivityDialogViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            SendCommand = new DelegateCommand(() => OnSendActivity());
            CancelCommand = new DelegateCommand(() => RequestClose.Invoke(null));
        }

        private void OnSendActivity()
        {
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivityRefused, true);
            dialogParams.Add(NavigationParameterKeys._ActivityComment, Comment);
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
