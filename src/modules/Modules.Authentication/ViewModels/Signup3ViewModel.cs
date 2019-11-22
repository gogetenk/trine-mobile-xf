using AutoMapper;
using Com.OneSignal;
using Microsoft.AppCenter;
using Prism.Commands;
using Prism.Logging;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using static Trine.Mobile.Dto.RegisterUserDto;

namespace Modules.Authentication.ViewModels
{
    public class Signup3ViewModel : ViewModelBase
    {
        #region Bindings 

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading = false;

        public ICommand CommercialCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ConsultantCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        #endregion 

        private RegisterUserDto _userToCreate;
        private readonly IAccountService _accountService;
        private readonly IModuleManager _moduleManager;

        public Signup3ViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService, IModuleManager moduleManager) : base(navigationService, mapper, logger, dialogService)
        {
            _accountService = accountService;
            _moduleManager = moduleManager;

            ConsultantCommand = new DelegateCommand(async () => await OnConsultantPicked());
            CommercialCommand = new DelegateCommand(async () => await OnCommercialPicked());
            CustomerCommand = new DelegateCommand(async () => await OnCustomerPicked());
            LoginCommand = new DelegateCommand(async () => await OnLogin());
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _userToCreate = parameters.GetValue<RegisterUserDto>(NavigationParameterKeys._User);
            if (_userToCreate is null)
                await NavigationService.NavigateAsync("LoginView");
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(NavigationParameterKeys._User, _userToCreate);
        }

        private async Task OnLogin()
        {
            await NavigationService.NavigateAsync("LoginView");
        }


        private async Task OnCommercialPicked()
        {
            await Submit(RegisterUserDto.GlobalRoleEnum.Admin);
        }

        private async Task OnCustomerPicked()
        {
            await Submit(RegisterUserDto.GlobalRoleEnum.Customer);
        }

        private async Task OnConsultantPicked()
        {
            await Submit(RegisterUserDto.GlobalRoleEnum.Consultant);
        }

        private async Task Submit(RegisterUserDto.GlobalRoleEnum role)
        {
            _userToCreate.GlobalRole = role;

            if (IsLoading)
                return;

            try
            {
                IsLoading = true;

                var id = await _accountService.RegisterUser(Mapper.Map<RegisterUserModel>(_userToCreate));
                if (string.IsNullOrEmpty(id))
                    throw new TechnicalException("Error while subscribing the user.");

                var navParams = new NavigationParameters();
                navParams.Add(NavigationParameterKeys._IsLaterTextVisible, true);
                //await NavigationService.NavigateAsync("OrganizationChoiceView", navParams);
                //TODO: remettre la création d'orga quand on sera prêts

                // Setting the user id to app center
                AppCenter.SetUserId(id);
                // Setting the user id to One Signal, and assigning tag
                OneSignal.Current.SetExternalUserId(id);
                OneSignal.Current.SendTag("user_type", Enum.GetName(typeof(GlobalRoleEnum), AppSettings.CurrentUser.GlobalRole));
                // Loading the corresponding module depending on user type
                LoadModuleFromUserType();
                await NavigationService.NavigateAsync("MenuRootView/TrineNavigationPage/HomeView");
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

        private void LoadModuleFromUserType()
        {
            string moduleName;
            switch (AppSettings.CurrentUser.GlobalRole)
            {
                case UserModel.GlobalRoleEnum.Admin:
                    moduleName = "CommercialModule";
                    break;
                case UserModel.GlobalRoleEnum.Consultant:
                    moduleName = "ConsultantModule";
                    break;
                case UserModel.GlobalRoleEnum.Customer:
                    moduleName = "CustomerModule";
                    break;
                default:
                    throw new BusinessException("Votre compte utilisateur n'est pas adapté à cette version de Trine. Veuillez créer un nouveau compte ou contacter le support client.");
            }
            _moduleManager.LoadModule(moduleName);
        }
    }
}
