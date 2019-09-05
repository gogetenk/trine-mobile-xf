using Akavache;
using AutoMapper;
using Com.OneSignal;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using Modules.Activity;
using Modules.Authentication;
using Modules.Dashboard;
using Modules.Menu;
using Modules.Mission;
using Modules.Organization;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Prism.Modularity;
using Prism.Unity;
using System;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Factory;
using Trine.Mobile.Bll.Impl.Services;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Bootstrapper.Resources;
using Trine.Mobile.Bootstrapper.Views;
using Trine.Mobile.Components.Builders;
using Trine.Mobile.Components.Logging;
using Trine.Mobile.Dal.Swagger;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Trine.Mobile.Bootstrapper
{
    public partial class App : PrismApplication
    {
        private const int smallWightResolution = 768;
        private const int smallHeightResolution = 1280;

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            try
            {

                InitializeComponent();
                LoadStyles();

                OneSignal
                    .Current
                    .StartInit("12785512-a98b-4c91-89ca-05959a685120")
                    .EndInit();
#if DEBUG
                HotReloader.Current.Run(this);
#endif
                Akavache.Registrations.Start("TrineApp");
                await NavigationService.NavigateAsync("TrineNavigationPage/SignupView");
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            AppCenter.Start("android=9cfc99dc-15cc-4652-b794-44df21413075;" +
                  "uwp={Your UWP App secret here};" +
                  "ios=8a841e14-34c8-4774-b034-c8ed5991f943",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            BlobCache.Shutdown().Wait();
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
            moduleCatalog.AddModule<MenuModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<MissionModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<ActivityModule>(InitializationMode.WhenAvailable);
        }

        public static bool IsASmallDevice()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            return (width <= smallWightResolution && height <= smallHeightResolution) && Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS;
        }

        private void LoadStyles()
        {
            if (IsASmallDevice())
            {
                base_dictionary.MergedDictionaries.Add(SmallDevicesStyle.SharedInstance);
            }
            else
            {
                base_dictionary.MergedDictionaries.Add(GeneralDevicesStyle.SharedInstance);
            }
        }

        #region Registrations

        private void RegisterLogger(IContainerRegistry containerRegistry)
        {
#if DEBUG
            containerRegistry.RegisterSingleton<Prism.Logging.ILogger, ConsoleLoggingService>();
#else
            containerRegistry.RegisterSingleton<Prism.Logging.ILogger, AppCenterLogger>();
#endif
            containerRegistry.RegisterSingleton<Microsoft.Extensions.Logging.ILogger, PrismLoggerWrapper>();
            containerRegistry.Register(typeof(ILogger<>), typeof(PrismLoggerWrapper<>));
        }

        private void RegisterNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TrineNavigationPage>();
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
            containerRegistry.RegisterInstance<IGatewayRepository>(new GatewayRepository(AppSettings.ApiUrls[AppSettings.GatewayApi], HttpClientFactory.GetClient()));
            containerRegistry.Register<IAccountService, AccountService>();
            containerRegistry.Register<IOrganizationService, OrganizationService>();
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IMissionService, MissionService>();
            containerRegistry.Register<IDashboardService, DashboardService>();
            containerRegistry.Register<IActivityService, ActivityService>();
        }

        #endregion

    }

}
