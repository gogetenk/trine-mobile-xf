using AutoFixture;
using FluentAssertions;
using Modules.Activity.ViewModels;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using Trine.Mobile.Bll;
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
    public class OnNavigatedToActivityDetailsViewModelTests : UnitTestBase
    {
        #region Consultant

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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be("En attente de signature");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeTrue();
            viewmodel.IsSaveButtonVisible.Should().BeTrue();
            viewmodel.CanModify.Should().BeTrue();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsConsultantSignedAsAConsultant_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsCustomerSignedAsAConsultant_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.CustomerSigned;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be($"Signé le {activity.Customer.SignatureDate}");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsModificationsRequiredAsAConsultant_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;
            activity.Consultant.SignatureDate = DateTime.UtcNow;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Red));
            viewmodel.ConsultantSignedStatusText.Should().Be($"En attente de modification");
            viewmodel.CustomerSignedStatusText.Should().Be($"Refusé");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._RefusedGlyph);
            viewmodel.IsCommentVisible.Should().BeTrue();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeTrue();
            viewmodel.IsSaveButtonVisible.Should().BeTrue();
            viewmodel.CanModify.Should().BeTrue();
        }


        #endregion


        #region Customer

        [Fact]
        public void OnNavigatedTo_WhenStatusIsGeneratedAsACustomer_ExpectCustomerUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Customer.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be("En attente de signature");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsConsultantSignedAsACustomer_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            activity.Customer.SignatureDate = DateTime.UtcNow;
            activity.Customer.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeTrue();
            viewmodel.IsRefuseButtonVisible.Should().BeTrue();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsCustomerSignedAsACustomer_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.CustomerSigned;
            activity.Customer.SignatureDate = DateTime.UtcNow;
            activity.Customer.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be($"Signé le {activity.Customer.SignatureDate}");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsModificationsRequiredAsACustomer_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;
            activity.Customer.SignatureDate = DateTime.UtcNow;
            activity.Customer.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Red));
            viewmodel.ConsultantSignedStatusText.Should().Be($"En attente de modification");
            viewmodel.CustomerSignedStatusText.Should().Be($"Refusé");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._RefusedGlyph);
            viewmodel.IsCommentVisible.Should().BeTrue();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        #endregion


        #region Commercial

        [Fact]
        public void OnNavigatedTo_WhenStatusIsGeneratedAsACommercial_ExpectCommercialUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.Generated;
            activity.Commercial.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be("En attente de signature");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsConsultantSignedAsACommercial_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ConsultantSigned;
            activity.Commercial.SignatureDate = DateTime.UtcNow;
            activity.Commercial.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be("En attente de signature");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsCustomerSignedAsACommercial_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.CustomerSigned;
            activity.Commercial.SignatureDate = DateTime.UtcNow;
            activity.Commercial.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Green));
            viewmodel.ConsultantSignedStatusText.Should().Be($"Signé le {activity.Consultant.SignatureDate}");
            viewmodel.CustomerSignedStatusText.Should().Be($"Signé le {activity.Customer.SignatureDate}");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._SignedGlyph);
            viewmodel.IsCommentVisible.Should().BeFalse();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        [Fact]
        public void OnNavigatedTo_WhenStatusIsModificationsRequiredAsACommercial_ExpectAccordingUI()
        {
            // Arrange
            AppSettings.CurrentUser = new Fixture().Create<UserModel>();
            var mission = new Fixture().Create<MissionDto>();
            var activity = new Fixture().Create<ActivityDto>();
            activity.Status = Trine.Mobile.Dto.ActivityStatusEnum.ModificationsRequired;
            activity.Commercial.SignatureDate = DateTime.UtcNow;
            activity.Customer.Id = AppSettings.CurrentUser.Id;
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
            viewmodel.ConsultantSignedTextColor.Should().Be(Color.FromHex(UIConstants._Yellow));
            viewmodel.CustomerSignedTextColor.Should().Be(Color.FromHex(UIConstants._Red));
            viewmodel.ConsultantSignedStatusText.Should().Be($"En attente de modification");
            viewmodel.CustomerSignedStatusText.Should().Be($"Refusé");
            viewmodel.ConsultantGlyph.Should().Be(UIConstants._PendingGlyph);
            viewmodel.CustomerGlyph.Should().Be(UIConstants._RefusedGlyph);
            viewmodel.IsCommentVisible.Should().BeTrue();
            viewmodel.IsAcceptButtonVisible.Should().BeFalse();
            viewmodel.IsRefuseButtonVisible.Should().BeFalse();
            viewmodel.IsSignButtonVisible.Should().BeFalse();
            viewmodel.IsSaveButtonVisible.Should().BeFalse();
            viewmodel.CanModify.Should().BeFalse();
        }

        #endregion

    }
}
