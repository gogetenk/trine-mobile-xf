using AutoFixture;
using FluentAssertions;
using Modules.Organization.ViewModels;
using Moq;
using Prism.Navigation;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.Tests;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xunit;

namespace Modules.Organization.UnitTests.ViewModels
{
    public class MemberDetailsViewModelTest : UnitTestBase
    {
        public MemberDetailsViewModelTest()
        {
        }

        [Fact]
        public async Task OnNavigatedTo_NominalCase_ExpectMember()
        {
            // Arrange
            var orgaId = new Fixture().Create<string>();
            var user = new Fixture().Create<UserDto>();
            var member = new Fixture().Create<OrganizationMemberModel>();
            var organizationServiceMock = new Mock<IOrganizationService>();

            organizationServiceMock
                .Setup(x => x.GetMember(orgaId, user.Id))
                .ReturnsAsync(member);

            var viewmodel = new MemberDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._OrganizationId, orgaId);
            navParams.Add(NavigationParameterKeys._User, user);

            // Act
            await viewmodel.InitializeAsync(navParams);

            // Assert
            viewmodel.Member.Should().NotBeNull();
            viewmodel.Member.Should().BeEquivalentTo(member);
        }

        [Fact]
        public async Task DeleteMember_NominalCase_ExpectDeleted()
        {
            // Arrange
            var orgaId = new Fixture().Create<string>();
            var user = new Fixture().Create<UserDto>();
            var member = new Fixture().Create<OrganizationMemberModel>();

            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetMember(orgaId, user.Id))
                .ReturnsAsync(member);

            _pageDialogService
                .Setup(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var viewmodel = new MemberDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._OrganizationId, orgaId);
            navParams.Add(NavigationParameterKeys._User, user);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.DeleteCommand.Execute(null);

            // Assert
            organizationServiceMock.Verify(x => x.RemoveMember(orgaId, member.UserId), Times.Once);
            _navigationService.Verify(x => x.GoBackAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMember_NominalCase_ExpectNotDeleted()
        {
            // Arrange
            var orgaId = new Fixture().Create<string>();
            var user = new Fixture().Create<UserDto>();
            var member = new Fixture().Create<OrganizationMemberModel>();

            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetMember(orgaId, user.Id))
                .ReturnsAsync(member);

            _pageDialogService
                .Setup(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var viewmodel = new MemberDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._OrganizationId, orgaId);
            navParams.Add(NavigationParameterKeys._User, user);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.DeleteCommand.Execute(null);

            // Assert
            organizationServiceMock.Verify(x => x.RemoveMember(orgaId, member.UserId), Times.Never);
            _navigationService.Verify(x => x.GoBackAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_NominalCase_ExpectUpdated()
        {
            // Arrange
            var orgaId = new Fixture().Create<string>();
            var user = new Fixture().Create<UserDto>();
            var member = new Fixture().Create<OrganizationMemberModel>();
            member.Role = OrganizationMemberModel.RoleEnum.MANAGER;
            var updatedMember = member;
            updatedMember.Role = OrganizationMemberModel.RoleEnum.SUPER_MANAGER;

            var organizationServiceMock = new Mock<IOrganizationService>();
            organizationServiceMock
                .Setup(x => x.GetMember(orgaId, user.Id))
                .ReturnsAsync(member);

            organizationServiceMock
                .Setup(x => x.UpdateMember(orgaId, updatedMember))
                .ReturnsAsync(updatedMember);

            var viewmodel = new MemberDetailsViewModel(_navigationService.Object, _mapper, _logger.Object, _pageDialogService.Object, organizationServiceMock.Object);
            var navParams = new NavigationParameters();
            navParams.Add(NavigationParameterKeys._OrganizationId, orgaId);
            navParams.Add(NavigationParameterKeys._User, user);

            // Act
            await viewmodel.InitializeAsync(navParams);
            viewmodel.SaveCommand.Execute(null);

            // Assert
            member.Should().BeEquivalentTo(updatedMember);
        }
    }
}
