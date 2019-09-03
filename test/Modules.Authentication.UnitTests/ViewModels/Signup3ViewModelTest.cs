using AutoFixture;
using Modules.Authentication.Navigation;
using Modules.Authentication.ViewModels;
using Moq;
using Prism.Navigation;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Authentication.UnitTests.ViewModels
{
    public class Signup3ViewModelTest : UnitTestBase
    {
        public Signup3ViewModelTest()
        {
        }

        [Fact]
        public async Task SubmitCommercial_NominalCase_ExpectNavigatedToSignup()
        {
            // Arrange
            var id = "1337";
            var credentials = new Fixture().Create<RegisterUserDto>();
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._User, credentials);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RegisterUser(It.Is<RegisterUserModel>(y => y.GlobalRole == RegisterUserModel.GlobalRoleEnum.Admin)))
                .ReturnsAsync(id);

            var viewmodel = new Signup3ViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.OnNavigatedTo(navparams);

            // Act
            viewmodel.CommercialCommand.Execute(null);

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("OrganizationChoiceView"), Times.Once);
        }

        [Fact]
        public async Task SubmitConsultant_NominalCase_ExpectNavigatedToSignup()
        {
            // Arrange
            var id = "1337";
            var credentials = new Fixture().Create<RegisterUserDto>();
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._User, credentials);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RegisterUser(It.Is<RegisterUserModel>(y => y.GlobalRole == RegisterUserModel.GlobalRoleEnum.Consultant)))
                .ReturnsAsync(id);

            var viewmodel = new Signup3ViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.OnNavigatedTo(navparams);

            // Act
            viewmodel.ConsultantCommand.Execute(null);

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("OrganizationChoiceView"), Times.Once);
        }

        [Fact]
        public async Task SubmitCustomer_NominalCase_ExpectNavigatedToSignup()
        {
            // Arrange
            var id = "1337";
            var credentials = new Fixture().Create<RegisterUserDto>();
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._User, credentials);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RegisterUser(It.Is<RegisterUserModel>(y => y.GlobalRole == RegisterUserModel.GlobalRoleEnum.Customer)))
                .ReturnsAsync(id);

            var viewmodel = new Signup3ViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.OnNavigatedTo(navparams);

            // Act
            viewmodel.CustomerCommand.Execute(null);

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("OrganizationChoiceView"), Times.Once);
        }

        [Fact]
        public async Task Submit_WhenServiceThrowsException_ExpectReport()
        {
            // Arrange
            var id = "1337";
            var credentials = new Fixture().Create<RegisterUserDto>();
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._User, credentials);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RegisterUser(It.Is<RegisterUserModel>(y => y.GlobalRole == RegisterUserModel.GlobalRoleEnum.Customer)))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new Signup3ViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.OnNavigatedTo(navparams);

            // Act
            viewmodel.CustomerCommand.Execute(null);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Submit_WhenServiceThrowsBusinessException_ExpectLogAndPopup()
        {
            // Arrange
            var id = "1337";
            var credentials = new Fixture().Create<RegisterUserDto>();
            var exception = new Fixture().Create<BusinessException>();
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._User, credentials);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RegisterUser(It.Is<RegisterUserModel>(y => y.GlobalRole == RegisterUserModel.GlobalRoleEnum.Customer)))
                .ThrowsAsync(exception);

            var viewmodel = new Signup3ViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.OnNavigatedTo(navparams);

            // Act
            viewmodel.CustomerCommand.Execute(null);

            // Assert
            _logger.Verify(x => x.Log(exception.Message, null), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(ErrorMessages.error, exception.Message, "Ok"), Times.Once);
        }
    }
}
