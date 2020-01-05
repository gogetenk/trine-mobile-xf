using AutoFixture;
using FluentAssertions;
using Modules.Customer.ViewModels;
using Moq;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Customer.UnitTests.ViewModels
{
    public class HomeViewModelTest : UnitTestBase
    {
        [Fact]
        public void LoadActivities_NominalCase()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activities = new Fixture().Create<List<ActivityModel>>();
            activities.ForEach(x => x.Customer.Id = AppSettings.CurrentUser.Id);

            var dialogServiceMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.ConsultantSigned))
                .ReturnsAsync(activities);

            var viewmodel = new HomeViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);

            // Assert
            viewmodel.Activities.Should().NotBeNull();
        }

        [Fact]
        public async Task SignActivity_NominalCase_ExpectScreenReloaded()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activities = new Fixture().Create<List<ActivityModel>>();
            activities.ForEach(x => x.Customer.Id = AppSettings.CurrentUser.Id);

            var dialogServiceMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
              .Setup(x => x.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.ConsultantSigned))
              .ReturnsAsync(activities);
            activityServiceMock
                .Setup(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>(), It.IsAny<Stream>()))
                .ReturnsAsync(activities.FirstOrDefault);
            var dialogParams = new Mock<IDialogParameters>();
            dialogParams
                .Setup(x => x.GetValue<bool>(NavigationParameterKeys._IsActivitySigned))
                .Returns(true);

            var viewmodel = new HomeViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);
            viewmodel.AcceptActivityCommand.Execute(activities.FirstOrDefault().Id);
            await viewmodel.OnSignDialogClosed(dialogParams.Object);

            // Assert
            activityServiceMock.Verify(x => x.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.ConsultantSigned), Times.Exactly(2));
        }

        [Fact]
        public void LoadActivities_WhenNoActivities_ExpectEmptyState()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activities = new Fixture().Create<List<ActivityModel>>();
            activities.ForEach(x => x.Customer.Id = AppSettings.CurrentUser.Id);

            var dialogServiceMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.ConsultantSigned))
                .ReturnsAsync(value: null);

            var viewmodel = new HomeViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);

            // Assert
            viewmodel.Activities.Should().BeNull();
            viewmodel.IsEmptyState.Should().BeTrue();
        }

        [Fact]
        public void LoadActivities_NetworkError_ExpectReported()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activities = new Fixture().Create<List<ActivityModel>>();
            activities.ForEach(x => x.Customer.Id = AppSettings.CurrentUser.Id);

            var dialogServiceMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromUser(AppSettings.CurrentUser.Id, Trine.Mobile.Model.ActivityStatusEnum.ConsultantSigned))
                .Throws<Exception>();

            var viewmodel = new HomeViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), It.IsAny<IDictionary<string, string>>()), Times.Once);
        }
    }
}
