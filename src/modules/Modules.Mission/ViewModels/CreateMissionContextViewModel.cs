﻿using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionContextViewModel : ViewModelBase
    {
        public ICommand StartCommand { get; set; }

        public CreateMissionContextViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            //StartCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("CreateMissionContextView"));
        }
    }
}