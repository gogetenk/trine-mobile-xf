﻿using AutoMapper;
using Modules.Authentication;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Prism.Modularity;
using Prism.Unity;
using Trine.Mobile.Bootstrapper.Builders;
using Trine.Mobile.Components.Logging;
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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterNavigation(containerRegistry);
            RegisterLogger(containerRegistry);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<AuthenticationModule>(InitializationMode.WhenAvailable);
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

        private void RegisterMapper(IContainerRegistry containerRegistry)
        {
            var mapper = new MapperBuilder().CreateMapper();
            containerRegistry.RegisterInstance<IMapper>(mapper);
        }

        #endregion
    }
}