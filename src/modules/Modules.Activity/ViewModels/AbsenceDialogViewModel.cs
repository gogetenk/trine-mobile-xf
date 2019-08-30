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
using Trine.Mobile.Dto;

namespace Modules.Activity.ViewModels
{
    public class AbsenceDialogViewModel : ViewModelBase, IDialogAware
    {
        public GridDayDto Day { get => _day; set { _day = value; RaisePropertyChanged(); } }
        private GridDayDto _day;

        public ICommand SendCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event Action<IDialogParameters> RequestClose;

        public AbsenceDialogViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            SendCommand = new DelegateCommand(() => OnValidateAbsence());
            CancelCommand = new DelegateCommand(() => RequestClose.Invoke(null));
        }

        private void OnValidateAbsence()
        {
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._Absence, Day);
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
            Day = parameters.GetValue<GridDayDto>(NavigationParameterKeys._Absence);
        }
    }
}
