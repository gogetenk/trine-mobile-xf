using System.Net.Http;
using CacheCow.Client;

namespace Trine.Mobile.Bll.Impl.Factory
{
    public sealed class HttpClientFactory : IHttpClientFactory
    {
        private readonly IAppSettings appSettings;

        public HttpClientFactory(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }


        public HttpClient CreateClient(string name)
        {
            HttpClient _httpClient = ClientExtensions.CreateClient(new ResilientHttpClientHandler(appSettings));
            return _httpClient;
        }

        public HttpClient GetClient()
        {
            return CreateClient(string.Empty);
        }
    }
}
