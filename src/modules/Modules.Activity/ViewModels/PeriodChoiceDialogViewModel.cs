﻿using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Activity.ViewModels
{
    public class PeriodChoiceDialogViewModel : ViewModelBase, IDialogAware
    {
        public DateTime SelectedDate { get => _selectedDate; set { _selectedDate = value; RaisePropertyChanged(); } }
        private DateTime _selectedDate;

        public List<DateTime> Dates { get => _dates; set { _dates = value; RaisePropertyChanged(); } }
        private List<DateTime> _dates;

        public List<string> StringDates { get => _stringDates; set { _stringDates = value; RaisePropertyChanged();  } }
        private List<string> _stringDates;

        public string SelectedStringDate { get => _selectedStringDate; set { _selectedStringDate = value; RaisePropertyChanged(); } }
        private string _selectedStringDate;


        public ICommand SendCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event Action<IDialogParameters> RequestClose;
        private MissionDto _mission;
        private readonly IActivityService _activityService;

        public PeriodChoiceDialogViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IActivityService activityService) : base(navigationService, mapper, logger, dialogService)
        {
            SendCommand = new DelegateCommand(() => OnValidatePeriod());
            CancelCommand = new DelegateCommand(() => RequestClose.Invoke(null));

            _activityService = activityService;
        }

        private void OnValidatePeriod()
        {
            SelectedDate = Dates.Where(x => StringDates.Contains(x.ToString("MMMM yyyy"))).FirstOrDefault();
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

            Dates = _activityService.GetMissionPeriods(Mapper.Map<MissionModel>(_mission));
            StringDates = Dates.Select(x => x.ToString("MMMM yyyy")).ToList();
        }
    }
}
