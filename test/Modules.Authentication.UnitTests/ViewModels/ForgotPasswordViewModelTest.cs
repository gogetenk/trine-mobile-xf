using System;
using AutoFixture;
using FluentAssertions;
using Modules.Authentication.ViewModels;
using Moq;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Authentication.UnitTests.ViewModels
{
    public class ForgotPasswordViewModelTest : UnitTestBase
    {
        public ForgotPasswordViewModelTest()
        {
        }

        [Fact]
        public void SubmitTest_NominalCase_ExpectNavigatedToConfirmationView()
        {
            // Arrange
            var passwordUpdate = new Fixture().Create<PasswordUpdateModel>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RecoverPasswordAsync(passwordUpdate));

            var viewmodel = new ForgotPasswordViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = passwordUpdate.Email;
            viewmodel.Password = passwordUpdate.NewPassword;
            viewmodel.Password2 = passwordUpdate.NewPassword;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("ForgotPasswordConfirmationView"), Times.Once);
        }

        [Fact]
        public void SubmitTest_WhenNotSamePassword_ExpectErrorMessage()
        {
            // Arrange
            var passwordUpdate = new Fixture().Create<PasswordUpdateModel>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RecoverPasswordAsync(passwordUpdate));

            var viewmodel = new ForgotPasswordViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = passwordUpdate.Email;
            viewmodel.Password = passwordUpdate.NewPassword;
            viewmodel.Password2 = "another password";

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            viewmodel.IsNotSamePassword.Should().BeTrue();
        }

        [Fact]
        public void SubmitTest_WhenServiceThrowsException_ExpectReport()
        {
            // Arrange
            var passwordUpdate = new Fixture().Create<PasswordUpdateModel>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(x => x.RecoverPasswordAsync(It.IsAny<PasswordUpdateModel>()))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new ForgotPasswordViewModel(_navigationService.Object, _mapper, _logger.Object, accountServiceMock.Object, _pageDialogService.Object);
            viewmodel.Email = passwordUpdate.Email;
            viewmodel.Password = passwordUpdate.NewPassword;
            viewmodel.Password2 = passwordUpdate.NewPassword;

            // Act
            viewmodel.SubmitCommand.Execute();

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

    }
}
