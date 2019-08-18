using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Activity.ViewModels
{
    public class ActivityDetailsViewModel : ViewModelBase
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public ActivityDto Activity { get => _activity; set { _activity = value; RaisePropertyChanged(); } }
        private ActivityDto _activity;

        #endregion

        private readonly IActivityService _activityService;

        public ActivityDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IActivityService activityService) : base(navigationService, mapper, logger, dialogService)
        {
            _activityService = activityService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Activity = parameters.GetValue<ActivityDto>(NavigationParameterKeys._Activity);
        }
    }
}
