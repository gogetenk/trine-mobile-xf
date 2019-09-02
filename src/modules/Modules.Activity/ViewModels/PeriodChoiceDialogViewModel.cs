using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Activity.ViewModels
{
    public class PeriodChoiceDialogViewModel : ViewModelBase, IDialogAware
    {
        public DateTime SelectedDate { get => _selectedDate; set { _selectedDate = value; RaisePropertyChanged(); } }
        private DateTime _selectedDate;

        public List<DateTime> Dates { get => _dates; set { _dates = value; RaisePropertyChanged(); } }
        private List<DateTime> _dates;

        public ICommand SendCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event Action<IDialogParameters> RequestClose;
        private MissionDto _mission;

        public PeriodChoiceDialogViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            SendCommand = new DelegateCommand(() => OnValidatePeriod());
            CancelCommand = new DelegateCommand(() => RequestClose.Invoke(null));
        }

        private void OnValidatePeriod()
        {
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._Period, SelectedDate);
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
            _mission = parameters.GetValue<MissionDto>(NavigationParameterKeys._Mission);
            if (_mission is null)
                RequestClose.Invoke(null);

            Dates = new List<DateTime>()
            {
                DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(1)
            };
        }
    }
}
