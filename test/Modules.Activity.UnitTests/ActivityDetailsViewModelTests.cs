using AutoFixture;
using FluentAssertions;
using Modules.Activity.ViewModels;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Forms;
using Xunit;

namespace Modules.Activity.UnitTests
{
    public class ActivityDetailsViewModelTests : UnitTestBase
    {
        [Fact]
        public void OnNavigatedTo_WhenStatusIsGeneratedAsAConsultant_ExpectConsultantUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var activityServiceMock = new Mock<IActivityService>();
            var dialogServiceMock = new Mock<IDialogService>();

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.Activity.Should().NotBeNull();
            viewmodel.Activity.Should().BeEquivalentTo(activity);
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex("#F0B429"));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex("#F0B429"));
            viewmodel.ConsultantSignedStatusText.Should().Be("En attente de signature");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be("\ue5d3");
            viewmodel.CustomerGlyph.Should().Be("\ue5d3");
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeTrue();
            viewmodel.IsSaveButtonVisible.Should().BeTrue();
            viewmodel.CanModify.Should().BeTrue();
        }
    }
}
