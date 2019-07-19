using Prism.Ioc;
using Prism.Modularity;
using Modules.Dashboard.Views;
using Modules.Dashboard.ViewModels;

namespace Modules.Dashboard
{
    public class DashboardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
        }
    }
}
