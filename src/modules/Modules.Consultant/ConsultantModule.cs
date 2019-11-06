using Modules.Consultant.ViewModels;
using Modules.Consultant.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Modules.Consultant
{
    public class ConsultantModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>();
            containerRegistry.RegisterForNavigation<MenuRootView, MenuRootViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
        }
    }
}
