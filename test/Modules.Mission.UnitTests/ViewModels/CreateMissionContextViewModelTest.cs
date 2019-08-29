using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class CreateMissionContextViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task OnNextStep_NominalCase_ExpectNavigated()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var orga = new Fixture().Create<PartialOrganizationDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            var viewmodel = new CreateMissionContextViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, dashboardServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, pickedUser);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);
            navParams.Add(NavigationParameterKeys._Organization, orga);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.CreateMissionRequest.ProjectName = "toto";
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.CreateMissionRequest.Should().NotBeNull();
            viewmodel.CreateMissionRequest.Customer.Should().BeEquivalentTo(pickedUser);
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionDatesView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)));
        }

        [Fact]
        public async Task OnNextStep_WhenNoPickedUser_ExpectErrorMessageVisible()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            var viewmodel = new CreateMissionContextViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, dashboardServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.CreateMissionRequest.ProjectName = "toto";
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsUserErrorVisible.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionDatesView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }

        [Fact]
        public async Task OnNextStep_WhenTitleEmpty_ExpectErrorMessageVisible()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var dashboardServiceMock = new Mock<IDashboardService>();
            var viewmodel = new CreateMissionContextViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, dashboardServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.CreateMissionRequest.ProjectName = "";
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsTitleEmptyErrorVisible.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionDatesView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }
    }
}