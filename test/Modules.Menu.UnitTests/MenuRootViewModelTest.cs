using AutoFixture;
using FluentAssertions;
using Modules.Menu.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Menu.UnitTests
{
    public class MenuRootViewModelTest : UnitTestBase
    {
        [Fact]
        public void OnNavigatedTo_NominalCase_ExpectOrgasAndMissions()
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
    }
}
