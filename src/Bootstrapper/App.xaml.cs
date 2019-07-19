using AutoMapper;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Modules.Authentication;
using Modules.Dashboard;
using Modules.Organization;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Prism.Modularity;
using Prism.Unity;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Factory;
using Trine.Mobile.Bll.Impl.Services;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Builders;
using Trine.Mobile.Components.Logging;
using Trine.Mobile.Dal.Swagger;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Trine.Mobile.Bootstrapper
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

#if DEBUG
            HotReloader.Current.Run(this);
#endif

            await NavigationService.NavigateAsync("NavigationPage/SignupView");
        }

        protected override void OnStart()
        {
            base.OnStart();

            AppCenter.Start("android=9cfc99dc-15cc-4652-b794-44df21413075;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterNavigation(containerRegistry);
            RegisterLogger(containerRegistry);
            RegisterMapper(containerRegistry);
            RegisterServices(containerRegistry);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<AuthenticationModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<OrganizationModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<DashboardModule>(InitializationMode.WhenAvailable);
        }

        #region Registrations

        private void RegisterLogger(IContainerRegistry containerRegistry)
        {
#if DEBUG
            containerRegistry.RegisterSingleton<ILoggerFacade, DebugLogger>();
#endif
            containerRegistry.RegisterSingleton<Microsoft.Extensions.Logging.ILogger, PrismLoggerWrapper>();

            var logger = new AppCenterLogger();
            containerRegistry.RegisterInstance<ILogger>(logger);
            containerRegistry.RegisterInstance<IAnalyticsService>(logger);
            containerRegistry.RegisterInstance<ICrashesService>(logger);
        }

        private void RegisterNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }

        private void RegisterExtensions(IContainerRegistry containerRegistry)
        {
            //HttpExtensions.AddResilientHttpClient(containerRegistry);
        }

        private void RegisterMapper(IContainerRegistry containerRegistry)
        {
            var mapper = new MapperBuilder().CreateMapper();
            containerRegistry.RegisterInstance<IMapper>(mapper);
        }

        private void RegisterServices(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterInstance<IGatewayRepository>(new GatewayRepository(AppSettings.GatewayApi, new HttpClient()));
            containerRegistry.RegisterInstance<IGatewayRepository>(new GatewayRepository(AppSettings.ApiUrls[AppSettings.GatewayApi], HttpClientFactory.GetClient()));
            containerRegistry.Register<IAccountService, AccountService>();
            containerRegistry.Register<IOrganizationService, OrganizationService>();
            containerRegistry.Register<IUserService, UserService>();
        }

        #endregion
    }
}
