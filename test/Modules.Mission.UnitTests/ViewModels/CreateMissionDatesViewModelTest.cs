using System;
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
    public class CreateMissionDatesViewModelTest : UnitTestBase
    {
        [Fact]
        public void OnNextStep_NominalCase_ExpectNavigated()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionDatesViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, pickedUser);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.CreateMissionRequest.Should().NotBeNull();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionConsultantView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)));
        }

        [Fact]
        public void OnNextStep_WhenEndDateBeforeStartDate_ExpectErrorMessage()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionDatesViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, pickedUser);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.CreateMissionRequest.EndDate = DateTime.UtcNow;
            viewmodel.CreateMissionRequest.StartDate = DateTime.UtcNow.AddMonths(3);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsStartDateSuperior.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionConsultantView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }

        [Fact]
        public void OnNextStep_WhenDateAreNull_ExpectErrorMessage()
        {
            // Arrange
            var pickedUser = new Fixture().Create<UserDto>();
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionDatesViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._User, pickedUser);
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.CreateMissionRequest.EndDate = default(DateTime);
            viewmodel.CreateMissionRequest.StartDate = default(DateTime);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsStartDateNull.Should().BeTrue();
            viewmodel.IsEndDateNull.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionConsultantView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }
    }
}
