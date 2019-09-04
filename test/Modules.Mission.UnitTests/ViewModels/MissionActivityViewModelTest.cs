using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class MissionActivityViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task OnNavigatedTo_NominalCase_ExpectMissionList()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activities = new Fixture().Create<ObservableCollection<ActivityModel>>();
            var dialogMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.Activities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task OnNavigatedTo_WhenMissionIsNull_ExpectGoBackAsync()
        {
            // Arrange
            var activityServiceMock = new Mock<IActivityService>();
            var dialogMock = new Mock<IDialogService>();

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, null);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            _navigationService.Verify(x => x.GoBackAsync(), Times.Once);
        }

        [Fact]
        public async Task OnNavigatedTo_WhenHasAlreadyBeenLoaded_ExpectDoNothing()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activities = new Fixture().Create<ObservableCollection<ActivityModel>>();
            var dialogMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.OnNavigatedTo(navParams); // Called two times in a row

            // Assert
            activityServiceMock.Verify(x => x.GetFromMission(It.IsAny<string>()), Times.AtMostOnce);
        }

        [Fact]
        public async Task OnSearchChanged_NominalCase_ExpectSearchedActivities()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activities = new Fixture().Create<ObservableCollection<ActivityModel>>();
            activities[0].StartDate = new DateTime(1991, 03, 14);
            var dialogMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SearchText = "1991";

            // Assert
            viewmodel.Activities.FirstOrDefault().Should().BeEquivalentTo(_mapper.Map<ActivityDto>(activities[0]));
        }

        [Fact]
        public async Task OnSearchChanged_WhenSearchTextIsEmpty_ExpectAllActivities()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activities = new Fixture().Create<ObservableCollection<ActivityModel>>();
            var dialogMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SearchText = viewmodel.Activities.FirstOrDefault().StartDate.ToString("MMMM yyyy");
            viewmodel.SearchText = "";

            // Assert
            viewmodel.Activities.Should().BeEquivalentTo(_mapper.Map<ObservableCollection<ActivityDto>>(activities));
        }

        [Fact]
        public async Task OnSearchChanged_WhenNoActivities_ExpectNoActivities()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var dialogMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(value: null);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SearchText = "";

            // Assert
            viewmodel.Activities.Should().BeNull();
        }

    }
}
