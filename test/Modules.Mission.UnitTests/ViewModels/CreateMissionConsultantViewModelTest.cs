using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class CreateMissionConsultantViewModelTest : UnitTestBase
    {
        [Fact]
        public void OnNextStep_NominalCase_ExpectNavigated()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionConsultantViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, pickedUser);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.CreateMissionRequest.Should().NotBeNull();
            viewmodel.CreateMissionRequest.Consultant.Should().BeEquivalentTo(pickedUser);
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionCommercialView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)));
        }

        [Fact]
        public void OnNextStep_WhenNoPickedUser_ExpectErrorMessageVisible()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionConsultantViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsUserErrorVisible.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionCommercialView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }


    }
}
