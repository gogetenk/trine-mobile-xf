using AutoFixture;
using FluentAssertions;
using Modules.Organization.ViewModels;
using Moq;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Organization.UnitTests.ViewModels
{
    public class OrganizationMissionsViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task IsActiveChanged_NominalCase_ExpectMissionList()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationDto>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var missionService = new Mock<IMissionService>();
            missionService
                .Setup(x => x.GetFromOrganization(It.IsAny<string>()))
                .ReturnsAsync(missions);

            var viewmodel = new OrganizationMissionsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionService.Object);
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._Organization, orga);

            // Act
            await viewmodel.InitializeAsync(navparams);

            // Assert
            viewmodel.Missions.Should().NotBeNull();
        }

        [Fact]
        public async Task IsActiveChanged_WhenServiceThrowsException_ExpectLogged()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationDto>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var missionService = new Mock<IMissionService>();
            missionService
                .Setup(x => x.GetFromOrganization(It.IsAny<string>()))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new OrganizationMissionsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionService.Object);
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._Organization, orga);

            // Act
            await viewmodel.InitializeAsync(navparams);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

        [Fact]
        public async Task OnSelectedMission_NominalCase_ExpectNavigated()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationDto>();
            var missions = new Fixture().Create<List<MissionModel>>();
            var missionService = new Mock<IMissionService>();
            missionService
                .Setup(x => x.GetFromOrganization(It.IsAny<string>()))
                .ReturnsAsync(missions);

            var viewmodel = new OrganizationMissionsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionService.Object);
            var navparams = new NavigationParameters();
            navparams.Add(NavigationParameterKeys._Organization, orga);

            // Act
            await viewmodel.InitializeAsync(navparams);
            viewmodel.SelectedMission = viewmodel.Missions.FirstOrDefault();

            // Assert
            viewmodel.Missions.Should().NotBeNull();
            _navigationService.Verify(x => x.NavigateAsync("MissionDetailsView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._Mission] == viewmodel.SelectedMission)));
        }
    }
}
