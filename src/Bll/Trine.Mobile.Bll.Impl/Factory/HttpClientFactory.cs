using System.Net.Http;
using CacheCow.Client;

namespace Trine.Mobile.Bll.Impl.Factory
{
    public sealed class HttpClientFactory
    {
        public static HttpClientFactory Instance { get; } = new HttpClientFactory();
        private static readonly HttpClient _httpClient = ClientExtensions.CreateClient(new ResilientHttpClientHandler());

        public static HttpClient GetClient()
        {
            return _httpClient;
        }
    }
}
