using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.ViewModels;

namespace Modules.Organization.ViewModels
{
    public class MembersViewModel : ViewModelBase
    {
        public string Title { get; set; } = "Panda Services";

        public MembersViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {

        }
    }
}
