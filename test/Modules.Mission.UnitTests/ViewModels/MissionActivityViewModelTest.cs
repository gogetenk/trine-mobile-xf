using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
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
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            await viewmodel.InitializeAsync(navParams);

            // Assert
            viewmodel.Activities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task OnNavigatedTo_WhenMissionIsNull_ExpectGoBackAsync()
        {
            // Arrange
            var activityServiceMock = new Mock<IActivityService>();

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, null);

            // Act
            await viewmodel.InitializeAsync(navParams);

            // Assert
            _navigationService.Verify(x => x.GoBackAsync(), Times.Once);
        }

        [Fact]
        public async Task OnNavigatedTo_WhenHasAlreadyBeenLoaded_ExpectDoNothing()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activities = new Fixture().Create<ObservableCollection<ActivityModel>>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            await viewmodel.InitializeAsync(navParams);
            await viewmodel.InitializeAsync(navParams); // Called two times in a row

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
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.SearchText = "1991";

            // Assert
            viewmodel.Activities.FirstOrDefault().Should().BeEquivalentTo(activities[0]);
        }

        [Fact]
        public async Task OnSearchChanged_WhenSearchTextIsEmpty_ExpectAllActivities()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activities = new Fixture().Create<ObservableCollection<ActivityModel>>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(activities);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.SearchText = viewmodel.Activities.FirstOrDefault().StartDate.ToString("MMMM yyyy");
            viewmodel.SearchText = "";

            // Assert
            viewmodel.Activities.Should().BeEquivalentTo(activities);
        }

        [Fact]
        public async Task OnSearchChanged_WhenNoActivities_ExpectNoActivities()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock
                .Setup(x => x.GetFromMission(mission.Id))
                .ReturnsAsync(value: null);

            var viewmodel = new MissionActivityViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.SearchText = "";

            // Assert
            viewmodel.Activities.Should().BeNull();
        }

    }
}
