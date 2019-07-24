using System;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class CreateMissionStartViewModel : ViewModelBase
    {
        #region Bindings 

        public ICommand StartCommand { get; set; }

        #endregion

        public CreateMissionStartViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            StartCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("CreateMissionContextView", useModalNavigation: false));
        }
    }
}
