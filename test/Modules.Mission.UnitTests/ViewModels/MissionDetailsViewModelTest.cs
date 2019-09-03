using AutoFixture;
using FluentAssertions;
using Modules.Mission.ViewModels;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Xunit;

namespace Modules.Mission.UnitTests.ViewModels
{
    public class MissionDetailsViewModelTest : UnitTestBase
    {
        [Fact]
        public async Task OnNavigatedTo_NominalCase_ExpectOnNavigatedToFirstTab()
        {
            // Arrange
            var mission = new Fixture().Create<MissionDto>();
            var dialogMock = new Mock<IDialogService>();
            var activityServiceMock = new Mock<IActivityService>();

            var viewmodel = new MissionDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Mission, mission);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.MissionActivityViewModel.Should().NotBeNull();
            viewmodel.MissionContractViewModel.Should().NotBeNull();
            viewmodel.MissionInvoiceViewModel.Should().NotBeNull();
            viewmodel.SelectedViewModelIndex.Should().Be(0);
        }

    }
}
