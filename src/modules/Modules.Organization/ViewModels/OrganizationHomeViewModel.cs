using AutoMapper;
using Prism;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Organization.ViewModels
{
    public class OrganizationHomeViewModel : ViewModelBase, IActiveAware
    {
        #region Bindings 

        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        private PartialOrganizationDto _organization;
        public PartialOrganizationDto Organization { get => _organization; set { _organization = value; RaisePropertyChanged(); } }

        #endregion 

        public OrganizationHomeViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Organization = parameters.GetValue<PartialOrganizationDto>(NavigationParameterKeys._Organization);
            if (Organization is null)
                await NavigationService.GoBackAsync();
        }

        // Triggered only on tabbed pages
        protected virtual async void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
