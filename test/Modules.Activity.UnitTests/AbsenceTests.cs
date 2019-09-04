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
using Xunit;

namespace Modules.Activity.UnitTests
{
    public class AbsenceTests : UnitTestBase
    {
        [Fact]
        public async Task SetAbsence_NominalCase_ExpectUpdatedActivity()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var dialogServiceMock = new Mock<IDialogService>();

            var absenceDay = activity.Days.FirstOrDefault();
            absenceDay.Absence = new AbsenceDto()
            {
                Comment = "Titi",
                Reason = Trine.Mobile.Dto.ReasonEnum.Holiday
            };
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._Absence, absenceDay);

            var activityServiceMock = new Mock<IActivityService>();
            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.AbsenceCommand.Execute(activity.Days.FirstOrDefault());
            viewmodel.OnAbsenceSettingsClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("AbsenceDialogView", dialogParams, It.IsAny<Action<IDialogResult>>()), Times.Once);
            viewmodel.Activity.Days.Any(x => x == absenceDay);
        }

        [Fact]
        public async Task SetAbsence_DialogCancelled_ExpectNoChanges()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Consultant.Id = AppSettings.CurrentUser.Id;
            var dialogServiceMock = new Mock<IDialogService>();

            var absenceDay = activity.Days.FirstOrDefault();
            absenceDay.Absence = new AbsenceDto()
            {
                Comment = "Titi",
                Reason = Trine.Mobile.Dto.ReasonEnum.Holiday
            };
            var dialogParams = new DialogParameters();
            dialogParams.Add(NavigationParameterKeys._Absence, null);

            var activityServiceMock = new Mock<IActivityService>();
            var viewmodel = new ActivityDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, activityServiceMock.Object, dialogServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._Activity, activity);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            viewmodel.AbsenceCommand.Execute(activity.Days.FirstOrDefault());
            viewmodel.OnAbsenceSettingsClosed(dialogParams);

            // Assert
            dialogServiceMock.Verify(x => x.ShowDialog("AbsenceDialogView", It.IsAny<IDialogParameters>(), It.IsAny<Action<IDialogResult>>()), Times.Once);
            viewmodel.Activity.Should().BeEquivalentTo(activity);
        }
    }
}
