using AutoMapper;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class MissionDetailsViewModel : ViewModelBase
    {
        private MissionDto _mission;

        #region Bindings


        private MissionInvoiceViewModel _missionInvoiceViewModel;
        public MissionInvoiceViewModel MissionInvoiceViewModel
        {
            get => _missionInvoiceViewModel;
            set { _missionInvoiceViewModel = value; RaisePropertyChanged(); }
        }

        private MissionContractViewModel _missionContractViewModel;
        public MissionContractViewModel MissionContractViewModel
        {
            get => _missionContractViewModel;
            set { _missionContractViewModel = value; RaisePropertyChanged(); }
        }

        private MissionActivityViewModel _missionActivityViewModel;
        public MissionActivityViewModel MissionActivityViewModel
        {
            get => _missionActivityViewModel;
            set { _missionActivityViewModel = value; RaisePropertyChanged(); }
        }

        private int _selectedViewModelIndex = 0;
        public int SelectedViewModelIndex
        {
            get => _selectedViewModelIndex;
            set { _selectedViewModelIndex = value; RaisePropertyChanged(); TriggerOnNavigatedTo(value); }
        }

        #endregion

        public MissionDetailsViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService pageDialogService, IActivityService activityService, IDialogService dialogService) : base(navigationService, mapper, logger, pageDialogService)
        {
            MissionActivityViewModel = new MissionActivityViewModel(navigationService, mapper, logger, pageDialogService, activityService, dialogService);
            MissionContractViewModel = new MissionContractViewModel(navigationService, mapper, logger, pageDialogService);
            MissionInvoiceViewModel = new MissionInvoiceViewModel(navigationService, mapper, logger, pageDialogService);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _mission = parameters.GetValue<MissionDto>(NavigationParameterKeys._Mission);
            TriggerOnNavigatedTo(0);
        }

        private void TriggerOnNavigatedTo(int value)
        {
            var parameters = new NavigationParameters();
            parameters.Add(NavigationParameterKeys._Mission, _mission);

            switch (value)
            {
                case 0:
                    MissionActivityViewModel.OnNavigatedTo(parameters);
                    break;
                case 1:
                    MissionInvoiceViewModel.OnNavigatedTo(parameters);
                    break;
                case 2:
                    MissionContractViewModel.OnNavigatedTo(parameters);
                    break;
            }
        }
    }
}
