﻿using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Organization.ViewModels
{
    public class OrganizationDetailsViewModel : ViewModelBase
    {
        public OrganizationDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
