using Modules.Customer.ViewModels;
using Modules.Customer.Views;
using Prism.Ioc;
using Prism.Modularity;

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
            containerRegistry.RegisterForNavigation<ActivityHistoryView, ActivityHistoryViewModel>();

            containerRegistry.RegisterDialog<AcceptActivityDialogView, SignActivityDialogViewModel>(); // Same VM
            containerRegistry.RegisterDialog<RefuseActivityDialogView, RefuseActivityDialogViewModel>();
        }
    }
}
