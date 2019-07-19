using AutoFixture;
using Modules.Organization.ViewModels;
using Moq;
using Prism.Navigation;
using Trine.Mobile.Bll;
using System.Collections.Generic;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Model;
using Xunit;
using System.Linq;
using FluentAssertions;
using System;
using Trine.Mobile.Dto;

namespace Modules.Organization.UnitTests.ViewModels
{
    public class MembersViewModelTest : UnitTestBase
    {
        public MembersViewModelTest()
        {
        }

        [Fact]
        public void OnNavigatedTo_NominalCase_ExpectMemberList()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationModel>();
            var members = new Fixture().CreateMany<UserModel>().ToList();
            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .ReturnsAsync(orga);
            organizationServiceMock
                .Setup(x => x.GetOrganizationMembers(orga.Id))
                .ReturnsAsync(members);

            var viewmodel = new MembersViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._IsUserPickerModeEnabled, false);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            viewmodel.Members.Should().NotBeNull();
        }

        [Fact]
        public void OnNavigatedTo_WhenOrgaServiceThrowsException_ExpectReport()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationModel>();
            var members = new Fixture().CreateMany<UserModel>().ToList();
            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new MembersViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._IsUserPickerModeEnabled, false);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

        [Fact]
        public void OnNavigatedTo_WhenMembersServiceThrowsException_ExpectReport()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationModel>();
            var members = new Fixture().CreateMany<UserModel>().ToList();
            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .ReturnsAsync(orga);
            organizationServiceMock
                .Setup(x => x.GetOrganizationMembers(orga.Id))
                .ThrowsAsync(It.IsAny<Exception>());

            var viewmodel = new MembersViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._IsUserPickerModeEnabled, false);

            // Act
            viewmodel.OnNavigatedTo(navParams);

            // Assert
            _logger.Verify(x => x.Report(It.IsAny<Exception>(), null), Times.Once);
        }

        [Fact]
        public void OnSelectdMember_NominalCase_ExpectNavigated()
        {
            // Arrange
            var orga = new Fixture().Create<PartialOrganizationModel>();
            var members = new Fixture().CreateMany<UserModel>().ToList();
            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetById(It.IsAny<string>()))
                .ReturnsAsync(orga);
            organizationServiceMock
                .Setup(x => x.GetOrganizationMembers(orga.Id))
                .ReturnsAsync(members);

            var viewmodel = new MembersViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._IsUserPickerModeEnabled, false);

            // Act
            viewmodel.OnNavigatedTo(navParams);
            var selectedMember = _mapper.Map<UserDto>(members.First());
            viewmodel.SelectedMember = selectedMember;

            // Assert
            _navigationService.Verify(x => x.NavigateAsync("MemberDetailsView", It.Is<NavigationParameters>(y => y[NavigationParameterKeys._User] == selectedMember)), Times.Once);
        }
    }
}