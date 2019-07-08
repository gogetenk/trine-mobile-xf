using Prism.Ioc;
using Prism.Modularity;
using Modules.Organization.Views;
using Modules.Organization.ViewModels;

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
        }
    }
}
