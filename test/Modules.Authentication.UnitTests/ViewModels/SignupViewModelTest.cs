using AutoFixture;
using FluentAssertions;
using Modules.Authentication.ViewModels;
using Moq;
using Prism.Navigation;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Authentication.UnitTests.ViewModels
{
    public class SignupViewModelTest : UnitTestBase
    {
        public SignupViewModelTest()
        {
        }

        [Fact]
        public async Task SignupTest_NominalCase_ExpectNavigatedToSignup()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(credentials))
                .ReturnsAsync(false);

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;
            viewmodel.Password = credentials.Password;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("Signup2View", It.IsAny<NavigationParameters>()), Times.Once);
        }

        [Fact]
        public async Task SignupTest_WhenServiceThrowsException_ExpectReport()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            string userId = new Fixture().Create<string>();
            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(It.IsAny<RegisterUserModel>()))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;
            viewmodel.Password = credentials.Password;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

        [Fact]
        public async Task SignupTest_WhenServiceThrowsBusinessException_ExpectLog()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            string userId = new Fixture().Create<string>();
            var exception = new Fixture().Create<BusinessException>();

            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(It.IsAny<RegisterUserModel>()))
                .ThrowsAsync(exception);

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;
            viewmodel.Password = credentials.Password;
            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            _logger.Verify(x => x.Log(exception.Message, null), Times.Once);
        }


        [Fact]
        public async Task SignupTest_whenEmailIsNull_ExpectEmailErrorMessage()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = null;
            viewmodel.Password = password;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            viewmodel.IsEmailErrorVisible.Should().BeTrue();
            viewmodel.IsPasswordErrorVisible.Should().BeFalse();
        }

        [Fact]
        public async Task SignupTest_whenPasswordIsNull_ExpectPasswordErrorMessage()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            var supportServiceMock = new Mock<ISupportService>();

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = email;
            viewmodel.Password = null;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            viewmodel.IsEmailErrorVisible.Should().BeFalse();
            viewmodel.IsPasswordErrorVisible.Should().BeTrue();
        }

        [Fact]
        public async Task SignupTest_whenCredentialsAreNull_ExpectCredentialsErrorMessages()
        {
            // Arrange
            string email = new Fixture().Create<string>();
            string password = new Fixture().Create<string>();
            string userId = new Fixture().Create<string>();
            var accountServiceMock = new Mock<IAccountService>();
            var supportServiceMock = new Mock<ISupportService>();

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = null;
            viewmodel.Password = null;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            viewmodel.IsEmailErrorVisible.Should().BeTrue();
            viewmodel.IsPasswordErrorVisible.Should().BeTrue();
        }

        [Fact]
        public async Task OnEmailEntered_NominalCase_ExpectNoMessage()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(It.IsAny<RegisterUserModel>()))
                .ReturnsAsync(false);

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;

            // Act
            viewmodel.EmailUnfocusedCommand.Execute();

            // Assert
            viewmodel.IsUserExistTextVisible.Should().BeFalse();
        }

        [Fact]
        public async Task OnEmailEntered_WhenUserAlreadyExist_ExpectMessage()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(It.IsAny<RegisterUserModel>()))
                .ReturnsAsync(true);

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;

            // Act
            viewmodel.EmailUnfocusedCommand.Execute();

            // Assert
            viewmodel.IsUserExistTextVisible.Should().BeTrue();
        }

        [Fact]
        public async Task OnEmailEntered_WhenServiceThrowsException_ExpectLogButNoPopup()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            var supportServiceMock = new Mock<ISupportService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(It.IsAny<RegisterUserModel>()))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;

            // Act
            viewmodel.EmailUnfocusedCommand.Execute();

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task OnEmailEntered_WhenServiceThrowsBusinessException_ExpectLogButNoPopup()
        {
            // Arrange
            var credentials = new Fixture().Create<RegisterUserModel>();
            var accountServiceMock = new Mock<IAccountService>();
            var supportServiceMock = new Mock<ISupportService>();
            var exception = new Fixture().Create<BusinessException>();
            accountServiceMock
                .Setup(x => x.DoesUserExist(It.IsAny<RegisterUserModel>()))
                .ThrowsAsync(exception);

            var viewmodel = new SignupViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object, supportServiceMock.Object);
            viewmodel.Email = credentials.Email;

            // Act
            viewmodel.EmailUnfocusedCommand.Execute();

            // Assert
            _logger.Verify(x => x.Log(It.IsAny<string>(), null), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
