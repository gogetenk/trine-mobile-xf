using AutoFixture;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Consultant.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Bindings

        private ActivityDto _activity;
        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        #endregion

        public HomeViewModel(INavigationService navigationService, global::AutoMapper.IMapper mapper, Prism.Logging.ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            Activity = new Fixture().Create<ActivityDto>();
        }
    }
}
