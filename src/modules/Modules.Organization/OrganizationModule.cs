using Modules.Organization.ViewModels;
using Modules.Organization.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Modules.Organization
{
    public class OrganizationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<OrganizationChoiceView, OrganizationChoiceViewModel>();
            containerRegistry.RegisterForNavigation<CreateOrganizationView, CreateOrganizationViewModel>();
            containerRegistry.RegisterForNavigation<JoinOrganizationView, JoinOrganizationViewModel>();
            containerRegistry.RegisterForNavigation<MembersView, MembersViewModel>();
            containerRegistry.RegisterForNavigation<MemberPickerView, MemberPickerViewModel>();
            containerRegistry.RegisterForNavigation<AddMemberView, AddMemberViewModel>();
            containerRegistry.RegisterForNavigation<MemberDetailsView, MemberDetailsViewModel>();
            containerRegistry.RegisterForNavigation<OrganizationDetailsView, OrganizationDetailsViewModel>();
            //containerRegistry.RegisterForNavigation<OrganizationHomeView, OrganizationHomeViewModel>();
            //containerRegistry.RegisterForNavigation<OrganizationMissionsView, OrganizationMissionsViewModel>();
            containerRegistry.RegisterForNavigation<OrganizationTabbedView, OrganizationTabbedViewModel>();
        }
    }
}
