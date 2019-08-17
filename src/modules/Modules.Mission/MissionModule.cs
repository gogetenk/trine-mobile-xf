using Modules.Mission.ViewModels;
using Modules.Mission.Views;
using Prism.Ioc;
using Prism.Modularity;

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
            containerRegistry.RegisterForNavigation<CreateMissionDatesView, CreateMissionDatesViewModel>();
            containerRegistry.RegisterForNavigation<CreateMissionConsultantView, CreateMissionConsultantViewModel>();
            containerRegistry.RegisterForNavigation<CreateMissionCommercialView, CreateMissionCommercialViewModel>();
            containerRegistry.RegisterForNavigation<CreateMissionPriceView, CreateMissionPriceViewModel>();
            containerRegistry.RegisterForNavigation<CreateMissionContractView, CreateMissionContractViewModel>();
            containerRegistry.RegisterForNavigation<CreateMissionSuccessView, CreateMissionSuccessViewModel>();
            containerRegistry.RegisterForNavigation<ContractDetailsView, ContractDetailsViewModel>();
            //containerRegistry.RegisterForNavigation<MissionActivityView, MissionActivityViewModel>();
            //containerRegistry.RegisterForNavigation<MissionContractView, MissionContractViewModel>();
            containerRegistry.RegisterForNavigation<MissionDetailsView, MissionDetailsViewModel>();
            //containerRegistry.RegisterForNavigation<MissionInvoiceView, MissionInvoiceViewModel>();
        }
    }
}
