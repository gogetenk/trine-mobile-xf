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
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Forms;
using Xunit;

namespace Modules.Activity.UnitTests
{
    public class RefuseTests : UnitTestBase
    {

        [Fact]
        public async Task RefuseActivityAsACustomer_NominalCase_ExpectUpdatedActivityAndUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Commercial.Id = AppSettings.CurrentUser.Id;
            activity.Days.FirstOrDefault().WorkedPart = Trine.Mobile.Dto.DayPartEnum.Full;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            var updatedActivity = activity;
            updatedActivity.Consultant.SignatureDate = null;
            updatedActivity.Customer.SignatureDate = null;
            updatedActivity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;

            var dialogServiceMock = new Mock<IDialogService>();
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._IsActivityRefused, true);

            var activityServiceMock = new Mock<IActivityService>();
            activityServiceMock.Setup(x => x.SaveActivityReport(It.IsAny<ActivityModel>()))
                .ReturnsAsync(It.IsAny<ActivityModel>());

            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.RefuseActivityCommand.Execute();
            await viewmodel.OnRefuseDialogClosed(dialogParams); 

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("RefuseActivityDialogView", null, It.IsAny<Action<IDialogResult>>()), Times.Once);
            activityServiceMock.Verify(x => x.SignActivityReport(AppSettings.CurrentUser, It.IsAny<ActivityModel>()), Times.Once);
        }
    }
}
