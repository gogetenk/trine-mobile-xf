using System;
using AutoFixture;
using FluentAssertions;
using Modules.Authentication.ViewModels;
using Moq;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Tests;
using Xunit;

namespace Modules.Authentication.UnitTests.ViewModels
{
    public class LoginViewModelTests : UnitTestBase
    {
        public LoginViewModelTests()
        {
        }

        [Fact]
        public void LoginTest_NominalCase_ExpectNavigatedToDashboard()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ReturnsAsync(userId);

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = email;
            viewmodel.Password = password;

            // Act
            viewmodel.LoginCommand.Execute(null);

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("../DashboardView"), Times.Once);
        }

        [Fact]
        public void LoginTest_WhenServiceThrowsException_ExpectReport()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = email;
            viewmodel.Password = password;

            // Act
            viewmodel.LoginCommand.Execute(null);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

        [Fact]
        public void LoginTest_WhenServiceThrowsBusinessException_ExpectReport()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var exception = new Fixture().Create<BusinessException>();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ThrowsAsync(exception);

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = email;
            viewmodel.Password = password;

            // Act
            viewmodel.LoginCommand.Execute(null);

            // Assert
            _logger.Verify(x => x.Log(exception.Message, null), Times.Once);
        }

        [Fact]
        public void LoginTest_WhenServiceReturnsNull_ExpectNoException()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var exception = new Fixture().Create<BusinessException>();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ReturnsAsync(value: null);

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = email;
            viewmodel.Password = password;

            // Act
            viewmodel.LoginCommand.Execute(null);
        }

        [Fact]
        public void LoginTest_whenEmailIsNull_ExpectEmailErrorMessage()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ReturnsAsync(userId);

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = null;
            viewmodel.Password = password;

            // Act
            viewmodel.LoginCommand.Execute(null);

            // Assert
            viewmodel.IsEmailErrorVisible.Should().BeTrue();
            viewmodel.IsPasswordErrorVisible.Should().BeFalse();
        }

        [Fact]
        public void LoginTest_whenPasswordIsNull_ExpectPasswordErrorMessage()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ReturnsAsync(userId);

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = email;
            viewmodel.Password = null;

            // Act
            viewmodel.LoginCommand.Execute(null);

            // Assert
            viewmodel.IsEmailErrorVisible.Should().BeFalse();
            viewmodel.IsPasswordErrorVisible.Should().BeTrue();
        }

        [Fact]
        public void LoginTest_whenCredentialsAreNull_ExpectCredentialsErrorMessages()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.Login(email, password))
                .ReturnsAsync(userId);

            var viewmodel = new LoginViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = null;
            viewmodel.Password = null;

            // Act
            viewmodel.LoginCommand.Execute(null);

            // Assert
            viewmodel.IsEmailErrorVisible.Should().BeTrue();
            viewmodel.IsPasswordErrorVisible.Should().BeTrue();
        }
    }
}
