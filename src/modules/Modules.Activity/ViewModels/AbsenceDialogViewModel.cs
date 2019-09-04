using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<string> _reasons = Enum.GetNames(typeof(ReasonEnum)).ToList();
        public List<string> Reasons { get => _reasons; set { _reasons = value; RaisePropertyChanged(); } }

        private string _selectedReason;
        public string SelectedReason { get => _selectedReason; set { _selectedReason = value; RaisePropertyChanged(); OnReasonChanged(); } }

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
            // Setting the absence to null if it's cancelled
            if (Day.Absence.Reason == ReasonEnum.Absent && string.IsNullOrEmpty(Day.Absence.Comment))
                Day = null;

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
            if (Day.Absence is null)
                Day.Absence = new AbsenceDto();

            SelectedReason = Enum.GetName(typeof(ReasonEnum), Day?.Absence?.Reason);
        }

        private void OnReasonChanged()
        {
            if (Day is null)
                return;

            if (Day.Absence is null)
                Day.Absence = new AbsenceDto();

            Enum.TryParse(SelectedReason, out ReasonEnum reason);
            Day.Absence.Reason = reason;
        }
    }
}
