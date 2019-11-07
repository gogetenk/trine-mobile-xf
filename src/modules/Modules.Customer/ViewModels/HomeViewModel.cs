using AutoFixture;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Customer.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Bindings

        private List<ActivityDto> _activity;
        public List<ActivityDto> Activities { get => _activity; set { _activity = value; RaisePropertyChanged(); } }

        #endregion

        public HomeViewModel(INavigationService navigationService, global::AutoMapper.IMapper mapper, Prism.Logging.ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            Activities = new Fixture().Create<List<ActivityDto>>();
        }
    }
}
