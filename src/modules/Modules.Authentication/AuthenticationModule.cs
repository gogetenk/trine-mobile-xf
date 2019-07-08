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
            containerRegistry.RegisterForNavigation<Signup2View, Signup2ViewModel>();
            containerRegistry.RegisterForNavigation<Signup3View, Signup3ViewModel>();
            containerRegistry.RegisterForNavigation<Signup4View, Signup4ViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<ForgotPasswordView, ForgotPasswordViewModel>();
            containerRegistry.RegisterForNavigation<ForgotPasswordConfirmationView, ForgotPasswordConfirmationViewModel>();
        }
    }
}
