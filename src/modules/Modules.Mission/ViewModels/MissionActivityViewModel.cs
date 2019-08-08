using AutoMapper;
using Prism;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Mission.ViewModels
{
    public class MissionActivityViewModel : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        public MissionActivityViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
        }

        // Triggered only on tabbed pages
        protected virtual async void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
