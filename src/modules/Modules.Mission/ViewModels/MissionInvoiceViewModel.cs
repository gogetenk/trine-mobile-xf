using AutoMapper;
using Prism;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Mission.ViewModels
{
    public class MissionInvoiceViewModel : ViewModelBase, IActiveAware
    {
        #region Bindings

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        public ObservableCollection<ActivityDto> Activities { get => _activities; set { _activities = value; RaisePropertyChanged(); } }
        private ObservableCollection<ActivityDto> _activities;

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        #endregion

        public ICommand RefreshCommand { get; set; }
        public MissionDto Mission { get; internal set; }

        private readonly IInvoiceService _invoiceService;
        public event EventHandler IsActiveChanged;
        private bool _hasBeenLoadedOnce;


        public MissionInvoiceViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService/*, IInvoiceService invoiceService*/) : base(navigationService, mapper, logger, dialogService)
        {
            //_invoiceService = invoiceService;
            RefreshCommand = new DelegateCommand(async () => await LoadData());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Mission = parameters.GetValue<MissionDto>(NavigationParameterKeys._Mission);
        }

        // Triggered only on tabbed pages
        protected virtual async void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);

            // We dont load the data each time we navigate on the tab
            if (_hasBeenLoadedOnce)
                return;

            await LoadData();
            _hasBeenLoadedOnce = true;
        }

        private async Task LoadData()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;
                //Activities = Mapper.Map<ObservableCollection<ActivityDto>>(await _invoiceService.GetFromMissionAsync("5ca5cab077e80c1344dbafec", null)); // TODO MOCKED
            }
            catch (BusinessException bExc)
            {
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
