using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class CreateMissionContractViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task OnNextStep_NominalCase_ExpectNavigated()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var contract = new Fixture().Create<FrameContractModel>();
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetContractPreview(It.IsAny<CreateMissionRequestModel>()))
                .ReturnsAsync(contract);

            var viewmodel = new CreateMissionContractViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.CreateMissionRequest.Should().NotBeNull();
            viewmodel.Contract.Should().NotBeNull();
            viewmodel.NextCommand.Execute();

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionSuccessView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)));
        }

        [Fact]
        public async Task OnNextStep_WhenServiceThrowsException_ExpectReport()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var contract = new Fixture().Create<FrameContractModel>();
            var missionServiceMock = new Mock<IMissionService>();
            missionServiceMock
                .Setup(x => x.GetContractPreview(It.IsAny<CreateMissionRequestModel>()))
                .ThrowsAsync(new Exception());

            var viewmodel = new CreateMissionContractViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, missionServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.NextCommand.Execute();

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }
    }
}
