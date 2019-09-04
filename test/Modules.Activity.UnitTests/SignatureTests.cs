using AutoFixture;
using FluentAssertions;
using Modules.Activity.ViewModels;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Forms;
using Xunit;

namespace Modules.Activity.UnitTests
{
    public class SignatureTests : UnitTestBase
    {

        [Fact]
        public async Task SignActivityAsAConsultant_NominalCase_ExpectUpdatedActivityAndUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            updatedActivity.Consultant.SignatureDate = DateTime.UtcNow;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivitySigned, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SignActivityReport(It.IsAny<UserModel>(), It.IsAny<ActivityModel>()))
                .ReturnsAsync(_mapper.Map<ActivityModel>(updatedActivity));

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SignActivityCommand.Execute();
            await viewmodel.OnSignDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("SignActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Once);
            viewmodel.Activity.Days.FirstOrDefault().WorkedPart.Should().Be(Trine.Mobile.Dto.DayPartEnum.Full);
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public async Task SignActivityAsAConsultant_WhenDialogIsRefused_ExpectNoChange()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            updatedActivity.Consultant.SignatureDate = DateTime.UtcNow;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivitySigned, false); // here

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SignActivityReport(It.IsAny<UserModel>(), It.IsAny<ActivityModel>()))
                .ReturnsAsync(_mapper.Map<ActivityModel>(updatedActivity));

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SignActivityCommand.Execute();
            await viewmodel.OnSignDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("SignActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Never);
        }

        [Fact]
        public async Task SignActivityAsAConsultant_WhenActivityIsNull_ExpectErrorMessage()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            updatedActivity.Consultant.SignatureDate = DateTime.UtcNow;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivitySigned, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SignActivityReport(It.IsAny<UserModel>(), It.IsAny<ActivityModel>()))
                .ReturnsAsync(value: null);

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SignActivityCommand.Execute();
            await viewmodel.OnSignDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("SignActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(ErrorMessages.error, "Une erreur s'est produite lors de la mise à jour du CRA", "Ok"));
        }

        [Fact]
        public async Task SignActivityAsAConsultant_WhenServiceThrowsException_ExpectErrorMessage()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            updatedActivity.Consultant.SignatureDate = DateTime.UtcNow;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivitySigned, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SignActivityReport(It.IsAny<UserModel>(), It.IsAny<ActivityModel>()))
                .ThrowsAsync(new Exception());

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SignActivityCommand.Execute();
            await viewmodel.OnSignDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("SignActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Once);
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null));
        }

    }
}
