using Prism.Ioc;
using Prism.Modularity;
using Modules.Customer.Views;
using Modules.Customer.ViewModels;

namespace Modules.Customer
{
    public class CustomerModule : IModule
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

            containerRegistry.RegisterDialog<AcceptActivityDialogView, SignActivityDialogViewModel>(); // Same VM
            containerRegistry.RegisterDialog<RefuseActivityDialogView, RefuseActivityDialogViewModel>();
        }
    }
}
