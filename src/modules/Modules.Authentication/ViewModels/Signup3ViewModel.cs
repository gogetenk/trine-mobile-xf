using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Modules.Authentication.Navigation;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;

namespace Modules.Authentication.ViewModels
{
    public class Signup3ViewModel : ViewModelBase
    {
        public ICommand CommercialCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ConsultantCommand { get; set; }

        private RegisterUserDto _userToCreate;


        public Signup3ViewModel(INavigationService navigationService, IMapper mapper, ILogger logger) : base(navigationService, mapper, logger)
        {
            ConsultantCommand = new DelegateCommand(async () => await OnConsultantPicked());
            CommercialCommand = new DelegateCommand(async () => await OnCommercialPicked());
            CustomerCommand = new DelegateCommand(async () => await OnCustomerPicked());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _userToCreate = parameters.GetValue<RegisterUserDto>(NavigationParameterKeys._User);
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
            var navParams = new NavigationParameters();
            _userToCreate.GlobalRole = role;
            navParams.Add(NavigationParameterKeys._User, _userToCreate);

            await NavigationService.NavigateAsync("Signup4View", navParams);
        }
    }
}
