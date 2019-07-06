using Prism.Ioc;
using Prism.Modularity;
using Modules.Authentication.Views;
using Modules.Authentication.ViewModels;

namespace Modules.Authentication
{
    public class AuthenticationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SignupView, SignupViewModel>();
        }
    }
}
