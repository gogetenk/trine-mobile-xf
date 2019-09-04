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
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;
namespace Modules.Activity.UnitTests
{
    public class SaveTests : UnitTestBase
    {

        [Fact]
        public async Task SaveActivityAsAConsultant_NominalCase_ExpectUpdatedActivity()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.UpdateActivity(It.IsAny<ActivityModel>()))
                .ReturnsAsync(_mapper.Map<ActivityModel>(updatedActivity));
            var dialogServiceMock = new Mock<IDialogService>();

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SaveActivityCommand.Execute();

            // Assert
            viewmodel.Activity.Days.FirstOrDefault().WorkedPart.Should().Be(Trine.Mobile.Dto.DayPartEnum.Full);
            activityServiceMock.Verify(x => x.UpdateActivity(It.IsAny<ActivityModel>()), Times.Once);
        }

        [Fact]
        public async Task SaveActivityAsAConsultant_WhenActivityIsNull_ExpectErrorMessage()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.UpdateActivity(It.IsAny<ActivityModel>()))
                .ReturnsAsync(value: null);
            var dialogServiceMock = new Mock<IDialogService>();

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SaveActivityCommand.Execute();

            // Assert
            activityServiceMock.Verify(x => x.UpdateActivity(It.IsAny<ActivityModel>()), Times.Once);
            _pageDialogService.Verify(x => x.DisplayAlertAsync(ErrorMessages.error, "Une erreur s'est produite lors de la mise à jour du CRA", "Ok"));
        }

        [Fact]
        public async Task SaveActivityAsAConsultant_WhenServiceThrowsException_ExpectErrorMessage()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.UpdateActivity(It.IsAny<ActivityModel>()))
                .ThrowsAsync(new Exception());
            var dialogServiceMock = new Mock<IDialogService>();

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SaveActivityCommand.Execute();

            // Assert
            activityServiceMock.Verify(x => x.UpdateActivity(It.IsAny<ActivityModel>()), Times.Once);
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null));
        }
    }
}
