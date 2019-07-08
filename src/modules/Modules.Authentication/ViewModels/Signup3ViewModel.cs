using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Modules.Authentication.Navigation;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

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

        #endregion 

        private RegisterUserDto _userToCreate;
        private readonly IAccountService _accountService;
        private readonly IPageDialogService _dialogService;

        public Signup3ViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IAccountService accountService, IPageDialogService dialogService) : base(navigationService, mapper, logger)
        {
            _accountService = accountService;
            _dialogService = dialogService;

            ConsultantCommand = new DelegateCommand(async () => await OnConsultantPicked());
            CommercialCommand = new DelegateCommand(async () => await OnCommercialPicked());
            CustomerCommand = new DelegateCommand(async () => await OnCustomerPicked());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _userToCreate = parameters.GetValue<RegisterUserDto>(NavigationParameterKeys._User);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(NavigationParameterKeys._User, _userToCreate);
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

                await NavigationService.NavigateAsync("OrganizationChoiceView");
            }
            catch (BusinessException bExc)
            {
                Logger.Log(bExc.Message);
                await _dialogService.DisplayAlertAsync(ErrorMessages.error, bExc.Message, "Ok");
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
