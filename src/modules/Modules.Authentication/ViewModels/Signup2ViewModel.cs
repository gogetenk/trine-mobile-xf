using AutoMapper;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Authentication.ViewModels
{
    public class Signup2ViewModel : ViewModelBase
    {
        #region Bindings

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading = false;

        public bool _isLastnameErrorVisible = false;
        public bool IsLastnameErrorVisible { get => _isLastnameErrorVisible; set { _isLastnameErrorVisible = value; RaisePropertyChanged(); } }

        public bool _isFirstnameErrorVisible = false;
        public bool IsFirstnameErrorVisible { get => _isFirstnameErrorVisible; set { _isFirstnameErrorVisible = value; RaisePropertyChanged(); } }

        public string _lastname;
        public string Lastname { get => _lastname; set { _lastname = value; RaisePropertyChanged(); } }

        public string _firstname;
        public string Firstname { get => _firstname; set { _firstname = value; RaisePropertyChanged(); } }

        public ICommand NextCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        #endregion

        private RegisterUserDto _userToCreate;
        private readonly IAccountService _accountService;

        public Signup2ViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            _accountService = accountService;

            LoginCommand = new DelegateCommand(async () => await OnLogin());
            NextCommand = new DelegateCommand(async () => await OnSubmit());
        }

        private async Task OnLogin()
        {
            await NavigationService.NavigateAsync("LoginView");
        }

        private async Task OnSubmit()
        {
            if (_userToCreate is null)
                await NavigationService.GoBackAsync();

            IsFirstnameErrorVisible = string.IsNullOrEmpty(Firstname);
            IsLastnameErrorVisible = string.IsNullOrEmpty(Lastname);

            if (IsFirstnameErrorVisible || IsLastnameErrorVisible)
                return;

            var navParams = new NavigationParameters();
            _userToCreate.LastName = Lastname;
            _userToCreate.FirstName = Firstname;
            navParams.Add(NavigationParameterKeys._User, _userToCreate);

            await NavigationService.NavigateAsync("Signup3View", navParams);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _userToCreate = parameters.GetValue<RegisterUserDto>(NavigationParameterKeys._User);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(NavigationParameterKeys._User, _userToCreate);
        }
    }
}
