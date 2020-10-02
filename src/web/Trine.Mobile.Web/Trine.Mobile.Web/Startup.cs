using System.Net.Http;
using AutoMapper;
using Com.OneSignal.Abstractions;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plugin.DownloadManager.Abstractions;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Services;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Builders;
using Trine.Mobile.Dal;
using Trine.Mobile.Dal.AzureBlobStorage.Repositories;
using Trine.Mobile.Dal.Configuration;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Web.Configs;
using Trine.Mobile.Web.Fakes;

namespace Trine.Mobile.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<IGatewayRepository>(provider => new GatewayRepository("https://app-assistance-dev.azurewebsites.net", provider.GetRequiredService<HttpClient>()));
            services.AddTransient<IImageAttachmentStorageRepository, ImageAttachmentStorageRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMissionService, MissionService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IImageAttachmentStorageConfiguration, ImageAttachmentStorageConfiguration>();
            services.AddSingleton<IDownloadManager, FakeDownloadManager>();
            services.AddSingleton<IOneSignal, FakeOneSignal>();
            services.AddSingleton<ISecureStorage, FakeSecureStorage>();

            services.AddLogging();
            services.AddSingleton<ILogger, Logger<object>>();
            var mapper = new MapperBuilder().CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
