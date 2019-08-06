using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Prism.Navigation;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class ContractDetailsViewModelTest : UnitTestBase
    {
        [Fact]
        public void OnNavigatedTo_NominalCase_ExpectContract()
        {
            // Arrange
            var request = new Fixture().Create<CreateMissionRequestDto>();
            var contract = new Fixture().Create<FrameContractDto>();

            var viewmodel = new ContractDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object);
            var navParams = new NavigationParameters
            {
                { NavigationParameterKeys._Contract, contract }
            };

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.Contract.Should().NotBeNull();
            viewmodel.Contract.Should().Be(contract);
        }
    }
}
