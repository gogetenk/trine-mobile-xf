using Prism.Ioc;
using Prism.Modularity;
using Modules.Menu.Views;
using Modules.Menu.ViewModels;

namespace Modules.Menu
{
    public class MenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>();
            containerRegistry.RegisterForNavigation<MenuRootView, MenuRootViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();
        }
    }
}
