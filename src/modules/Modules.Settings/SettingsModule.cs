using Modules.Settings.ViewModels;
using Modules.Settings.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Modules.Settings
{
    public class SettingsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<EditUserView, EditUserViewModel>();
        }
    }
}
