using Prism.Ioc;
using Prism.Modularity;
using Modules.Mission.Views;
using Modules.Mission.ViewModels;

namespace Modules.Mission
{
    public class MissionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CreateMissionStartView, CreateMissionStartViewModel>();
            containerRegistry.RegisterForNavigation<CreateMissionContextView, CreateMissionContextViewModel>();
        }
    }
}
