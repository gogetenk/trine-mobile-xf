using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
using System.Threading.Tasks;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class CreateMissionPriceViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task OnNavigatedTo_NominalCase_ExpectStandardValues()
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
            await viewmodel.InitializeAsync(navParams);

            // Assert
            viewmodel.CreateMissionRequest.DailyPrice.Should().Be(400);
            viewmodel.CreateMissionRequest.CommercialFeePercentage.Should().Be(15);
        }

        [Fact]
        public async Task OnNextStep_NominalCase_ExpectNavigated()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionPriceViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act 
            await viewmodel.InitializeAsync(navParams);
            request.DailyPrice = 10f;
            request.CommercialFeePercentage = 10f;
            viewmodel.PriceChangedCommand.Execute(null);
            viewmodel.TotalPrice.Should().Be(viewmodel.CreateMissionRequest.DailyPrice + (viewmodel.CreateMissionRequest.DailyPrice * viewmodel.CreateMissionRequest.CommercialFeePercentage * .01f));
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.CreateMissionRequest.Should().NotBeNull();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionContractView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)));
        }

        [Fact]
        public async Task OnNextStep_WhenNoPickedUser_ExpectErrorMessageVisible()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var viewmodel = new CreateMissionPriceViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._CreateMissionRequest, request);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.CreateMissionRequest.DailyPrice = 0;
            viewmodel.NextCommand.Execute();

            // Assert
            viewmodel.IsDailyRateErrorVisible.Should().BeTrue();
            _navigationService.Verify(x => x.NavigateAsync("CreateMissionContractView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._CreateMissionRequest] == viewmodel.CreateMissionRequest)), Times.Never);
        }
    }
}
