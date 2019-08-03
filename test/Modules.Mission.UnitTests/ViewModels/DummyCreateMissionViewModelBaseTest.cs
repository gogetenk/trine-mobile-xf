using AutoFixture;
using FluentAssertions;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class DummyCreateMissionViewModelBaseTest : UnitTestBase
    {
        [Fact]
        public void OnNavigatedTo_NominalCase_ExpectPickedUserNotNull()
        {
            // Arrange
            var createMission = new Fixture().Create<CreateMissionRequestDto>();
            var pageDialogServiceMock = new Mock<IPageDialogService>();
            var dto = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();

            var viewmodel = new DummyCreateMissionViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, dto);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.PickedUser.Should().NotBeNull();
            viewmodel.CreateMissionRequest.Should().NotBeNull();
        }

        [Fact]
        public void OnNavigatedTo_WhenRequestIsNull_ExpectPickedUserNotNull()
        {
            // Arrange
            var dto = new Fixture().Create<UserDto>();

            var viewmodel = new DummyCreateMissionViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, dto);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, null);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.PickedUser.Should().NotBeNull();
            _pageDialogService.Verify(x => x.DisplayAlertAsync("Oops...", ErrorMessages.unknownError, "Ok"), Times.Once);
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionStartView"), Times.Once);
        }

        [Fact]
        public void OnNavigatedTo_WhenCreateMissionRequestIsNotNull_ExpectNoOtherCall()
        {
            // Arrange
            var createMission = new Fixture().Create<CreateMissionRequestDto>();
            var dto = new Fixture().Create<UserDto>();

            var viewmodel = new DummyCreateMissionViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, dto);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, createMission);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.PickedUser.Should().NotBeNull();
            _pageDialogService.Verify(x => x.DisplayAlertAsync("Oops...", ErrorMessages.unknownError, "Ok"), Times.Never);
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionStartView"), Times.Never);
        }

        [Fact]
        public void OnRemoveUser_NominalCase_ExpectUserIsNull()
        {
            // Arrange
            var createMission = new Fixture().Create<CreateMissionRequestDto>();
            var dto = new Fixture().Create<UserDto>();

            var viewmodel = new DummyCreateMissionViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, dto);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, createMission);

            // Act
            viewmodel.RemovedUserCommand.Execute(null);

            // Assert
            viewmodel.PickedUser.Should().BeNull();
        }
    }
}
