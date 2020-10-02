using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Settings;

namespace Trine.Mobile.Bll.Impl.Factory
{
    public class ResilientHttpClientHandler : HttpClientHandler
    {
        public IAppSettings AppSettings { get; }

        public ResilientHttpClientHandler(IAppSettings appSettings) : base()
        {
            AppSettings = appSettings;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // If we are not calling login endpoint and we have no accesstoken 
            if (!request.RequestUri.OriginalString.Contains("accounts/users/register") && !request.RequestUri.OriginalString.Contains("accounts/login") && AppSettings.AccessToken != null && !string.IsNullOrEmpty(AppSettings.AccessToken.AccessToken))
            {
                // Adding JWT in header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.AccessToken.AccessToken);
            }

            var policyWrap = Policy.WrapAsync(CreatePolicies().ToArray());
            return await policyWrap.ExecuteAsync(async () =>
            {
                var message = await base.SendAsync(request, cancellationToken);
                if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    var response = await message.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(response);

                    if (jobject != null && jobject.ContainsKey("errorCode") && jobject["errorCode"].ToString().StartsWith("5"))
                        throw new HttpRequestException();
                }

                return message;
            });

            //var message = await base.SendAsync(request, cancellationToken);
            //if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            //{
            //    var response = await message.Content.ReadAsStringAsync();
            //    var jobject = JObject.Parse(response);
            //    if (jobject != null && jobject.ContainsKey("errorCode") && jobject["errorCode"].ToString().StartsWith("5"))
            //        throw new HttpRequestException();
            //}
            //else if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    // If we 
            //    if (AppSettings.CurrentUser is null)
            //    {
            //        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{request.RequestUri.Host}/api/accounts/login",);
            //        await base.SendAsync(tokenRequest, cancellationToken);
            //    }
            //}

            //return message;

        }

        private static IEnumerable<IAsyncPolicy> CreatePolicies()
        {
            var waitAndRetryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    50,
                    retryAttempt => TimeSpan.FromSeconds(1),
                    (exception, timeSpan, retryCount, context) =>
                    {

                    });

            var circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    // number of exceptions before breaking circuit
                    5,
                    // time circuit opened before retry
                    TimeSpan.FromMinutes(1),
                    (exception, duration) =>
                    {
                        // on circuit opened
                        //_logger.LogTrace("Circuit breaker opened");
                    },
                    () =>
                    {
                        // on circuit closed
                        //_logger.LogTrace("Circuit breaker reset");
                    });

            //var tokenExpiredPolicy = Policy
            //    .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
            //    .RetryAsync(1, async (result, retryCount, context) =>
            //    {
            //        var credentials = new UserCredentialsModel(AppSettings.CurrentUser.Mail, AppSettings.CurrentUser.);
            //        var client = new HttpClient();
            //        var response = await client.PostAsync($"{AppSettings.ApiUrls["GatewayApi"]}/api/accounts/login", new StringContent());
            //        response.
            //    });

            return new List<IAsyncPolicy>
            {
                waitAndRetryPolicy,
                circuitBreakerPolicy
            };
        }

        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim().ToLower();
        }

        private static string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);

            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";

            return origin;
        }

    }
}
