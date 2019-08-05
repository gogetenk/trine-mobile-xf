﻿using AutoFixture;
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
    public class CreateMissionPriceViewModelTest : UnitTestBase
    {
        [Fact]
        public void OnNavigatedTo_NominalCase_ExpectStandardValues()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            request.DailyPrice = 0;
            request.CommercialFeePercentage = 0f;

            var viewmodel = new CreateMissionPriceViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters
            {
                { NavigationParameterKeys._CreateMissionRequest, request }
            };

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.CreateMissionRequest.DailyPrice.Should().Be(400);
            viewmodel.CreateMissionRequest.CommercialFeePercentage.Should().Be(15);
        }

        [Fact]
        public void OnNextStep_NominalCase_ExpectNavigated()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionPriceViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act 
            viewmodel.OnNavigatedTo(navParams);
            request.DailyPrice = 10f;
            request.CommercialFeePercentage = 10f;
            viewmodel.PriceChangedCommand.Execute(null);
            viewmodel.TotalPrice.Should().Be(11);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.CreateMissionRequest.Should().NotBeNull();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionContractView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)));
        }

        [Fact]
        public void OnNextStep_WhenNoPickedUser_ExpectErrorMessageVisible()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionPriceViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsUserErrorVisible.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionContractView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }
    }
}
