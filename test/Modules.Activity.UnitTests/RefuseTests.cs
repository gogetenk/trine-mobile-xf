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
    public class RefuseTests : UnitTestBase
    {

        [Fact]
        public async Task RefuseActivityAsACustomer_NominalCase_ExpectUpdatedActivityAndUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Customer.Id = AppSettings.CurrentUser.Id;
            activity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var updatedActivity = activity;
            updatedActivity.Consultant.SignatureDate = null;
            updatedActivity.Customer.SignatureDate = null;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;

            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivityRefused, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SaveActivityReport(It.IsAny<ActivityModel>()))
                .ReturnsAsync(_mapper.Map<ActivityModel>(updatedActivity));

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.RefuseActivityCommand.Execute();
            await viewmodel.OnRefuseDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("RefuseActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Red));
            viewmodel.ConsultantSignedStatusText.Should().Be($"En attente de modification");
            viewmodel.CustomerSignedStatusText.Should().Be($"Refusé");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._RefusedGlyph);
            viewmodel.IsCommentVisible.Should().BeTrue();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public async Task RefuseActivityAsACustomer_WhenDialogCancelled_ExpectNoChange()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Customer.Id = AppSettings.CurrentUser.Id;
            activity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var updatedActivity = activity;
            updatedActivity.Consultant.SignatureDate = null;
            updatedActivity.Customer.SignatureDate = null;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;

            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivityRefused, false);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SaveActivityReport(It.IsAny<ActivityModel>()))
                .ReturnsAsync(_mapper.Map<ActivityModel>(updatedActivity));

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.RefuseActivityCommand.Execute();
            await viewmodel.OnRefuseDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("RefuseActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Never);
            viewmodel.Activity.Should().BeEquivalentTo(activity);
        }

        [Fact]
        public async Task RefuseActivityAsACustomer_WhenActivityIsNull_ExpectErrorMessage()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Customer.Id = AppSettings.CurrentUser.Id;
            activity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;

            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivityRefused, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SaveActivityReport(It.IsAny<ActivityModel>()))
                .ReturnsAsync(value: null);

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.RefuseActivityCommand.Execute();
            await viewmodel.OnRefuseDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("RefuseActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(ErrorMessages.error, "Une erreur s'est produite lors de la mise à jour du CRA", "Ok"));
        }

        [Fact]
        public async Task RefuseActivityAsACustomer_WhenServiceThrowsException_ExceptLogged()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Customer.Id = AppSettings.CurrentUser.Id;
            activity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;

            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivityRefused, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SaveActivityReport(It.IsAny<ActivityModel>()))
                .ThrowsAsync(new Exception());

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.RefuseActivityCommand.Execute();
            await viewmodel.OnRefuseDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("RefuseActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null));
        }
    }
}
