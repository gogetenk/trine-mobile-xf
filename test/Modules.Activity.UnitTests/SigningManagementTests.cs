using AutoFixture;
using FluentAssertions;
using Modules.Activity.ViewModels;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class SigningManagementTests : UnitTestBase
    {
        private const string _Yellow = "#F0B429";
        private const string _Green = "#3EBD93";
        private const string _Red = "#FF5A39";
        private const string _PendingGlyph = "\ue5d3";
        private const string _SignedGlyph = "\ue5ca";
        private const string _RefusedGlyph = "\ue5cd";

        [Fact]
        public async Task SignActivity_AsAConsultant_ExpectUpdatedActivity()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var updatedActivity = activity;
            updatedActivity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            updatedActivity.Consultant.SignatureDate = DateTime.UtcNow;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivitySigned, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SignActivityReport(It.IsAny<UserModel>(), It.IsAny<ActivityModel>()))
                .ReturnsAsync(_mapper.Map<ActivityModel>(updatedActivity));

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.SignActivityCommand.Execute();
            await viewmodel.OnSignDialogClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("SignActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Once);
            viewmodel.Activity.Days.FirstOrDefault().WorkedPart.Should().Be(Trine.Mobile.Dto.DayPartEnum.Full);
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(_Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(_Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(_SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(_PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

    }
}
