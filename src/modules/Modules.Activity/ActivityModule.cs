using Modules.Activity.ViewModels;
using Modules.Activity.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Modules.Activity
{
    public class ActivityModule : IModule
    {


        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ActivityDetailsView, ActivityDetailsViewModel>();
            containerRegistry.RegisterDialog<SignActivityDialogView, SignActivityDialogViewModel>();
            containerRegistry.RegisterDialog<RefuseActivityDialogView, RefuseActivityDialogViewModel>();
            containerRegistry.RegisterDialog<AbsenceDialogView, AbsenceDialogViewModel>();
        }
    }
}
