using AutoFixture;
using FluentAssertions;
using Modules.Menu.ViewModels;
using Moq;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Menu.UnitTests
{
    public class MenuRootViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task OnNavigatedTo_NominalCase_ExpectOrgasAndMissions()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var orgas = new Fixture().Create<ObservableCollection<PartialOrganizationModel>>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            dashboardServiceMock
                .Setup(x => x.GetUserOrganizations(AppSettings.CurrentUser.Id))
                .ReturnsAsync(orgas);
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetUserMissions(AppSettings.CurrentUser.Id))
                .ReturnsAsync(missions);

            var viewmodel = new MenuRootViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object, dashboardServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);

            // Assert
            viewmodel.Missions.Should().NotBeNull();
            viewmodel.Organizations.Should().NotBeNull();
            viewmodel.Missions.Should().BeEquivalentTo(_mapper.Map<List<MissionDto>>(missions));
            viewmodel.Organizations.Should().BeEquivalentTo(_mapper.Map<ObservableCollection<PartialOrganizationDto>>(orgas));
        }

        [Fact]
        public async Task OnMissionSelected_NominalCase_ExpectNavigated()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var orgas = new Fixture().Create<ObservableCollection<PartialOrganizationModel>>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            dashboardServiceMock
                .Setup(x => x.GetUserOrganizations(AppSettings.CurrentUser.Id))
                .ReturnsAsync(orgas);
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetUserMissions(AppSettings.CurrentUser.Id))
                .ReturnsAsync(missions);

            var viewmodel = new MenuRootViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object, dashboardServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);
            var selectedMission = viewmodel.Missions[1];
            viewmodel.SelectedMission = selectedMission;

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("TrineNavigationPage/MissionDetailsView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._Mission] == selectedMission)), Times.Once);
        }

        [Fact]
        public async Task OnOrganizationSelected_NominalCase_ExpectNavigated()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var orgas = new Fixture().Create<ObservableCollection<PartialOrganizationModel>>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            dashboardServiceMock
                .Setup(x => x.GetUserOrganizations(AppSettings.CurrentUser.Id))
                .ReturnsAsync(orgas);
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetUserMissions(AppSettings.CurrentUser.Id))
                .ReturnsAsync(missions);

            var viewmodel = new MenuRootViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object, dashboardServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);
            var selectedOrga = viewmodel.Organizations[1];
            viewmodel.SelectedOrganization = selectedOrga;

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("TrineNavigationPage/OrganizationTabbedView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._Organization] == selectedOrga)), Times.Once);
        }

        [Fact]
        public async Task OnNavigatedTo_WhenOrgaServiceThrowsException_ExpectLogged()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var orgas = new Fixture().Create<ObservableCollection<PartialOrganizationModel>>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            dashboardServiceMock
                .Setup(x => x.GetUserOrganizations(AppSettings.CurrentUser.Id))
                .ThrowsAsync(new Exception());
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetUserMissions(AppSettings.CurrentUser.Id))
                .ReturnsAsync(missions);

            var viewmodel = new MenuRootViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object, dashboardServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

        [Fact]
        public async Task OnNavigatedTo_WhenMissionServiceThrowsException_ExpectLogged()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var orgas = new Fixture().Create<ObservableCollection<PartialOrganizationModel>>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            dashboardServiceMock
                .Setup(x => x.GetUserOrganizations(AppSettings.CurrentUser.Id))
                .ReturnsAsync(orgas);
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetUserMissions(AppSettings.CurrentUser.Id))
                .ThrowsAsync(new Exception());

            var viewmodel = new MenuRootViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object, dashboardServiceMock.Object);

            // Act
            viewmodel.OnNavigatedTo(null);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }
    }
}
