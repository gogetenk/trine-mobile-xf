using Modules.Settings.ViewModels;
using Modules.Settings.Views;
using Prism.Ioc;
using Prism.Modularity;
using Xamarin.Forms;

namespace Modules.Settings
{
    public class SettingsModule : IModule
    {
        public static NavigationPage navPageInstance;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //navPageInstance = containerProvider.Resolve<NavigationPage>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<EditUserView, EditUserViewModel>();
            containerRegistry.RegisterForNavigation<HelpView, HelpViewModel>();
            containerRegistry.RegisterForNavigation<NotificationsView, NotificationsViewModel>();
            containerRegistry.RegisterForNavigation<IntegrationsView, IntegrationsViewModel>();
        }
    }
}
