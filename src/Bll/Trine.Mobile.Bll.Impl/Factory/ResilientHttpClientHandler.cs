using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Trine.Mobile.Bll.Impl.Factory
{
    public class ResilientHttpClientHandler : HttpClientHandler
    {
        public ResilientHttpClientHandler() : base()
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Adding JWT in header

            var message = await base.SendAsync(request, cancellationToken);
            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await message.Content.ReadAsStringAsync();
                var jobject = JObject.Parse(response);

                if (jobject != null && jobject.ContainsKey("errorCode") && jobject["errorCode"].ToString().StartsWith("5"))
                    throw new HttpRequestException();
            }

            return message;

            //var policyWrap = Policy.WrapAsync(CreatePolicies().ToArray());
            //return await policyWrap.ExecuteAsync(async () =>
            //{
            //    var message = await base.SendAsync(request, cancellationToken);
            //    if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            //    {
            //        var response = await message.Content.ReadAsStringAsync();
            //        var jobject = JObject.Parse(response);

            //        if (jobject != null && jobject.ContainsKey("errorCode") && jobject["errorCode"].ToString().StartsWith("5"))
            //            throw new HttpRequestException();
            //    }

            //    return message;
            //});
        }

        //private static IEnumerable<Policy> CreatePolicies()
        //{
        //    var waitAndRetryPolicy = Policy.Handle<HttpRequestException>()
        //        .WaitAndRetryAsync(
        //            50,
        //            retryAttempt => TimeSpan.FromSeconds(1),
        //            (exception, timeSpan, retryCount, context) =>
        //            {

        //            });

        //    var circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
        //        .CircuitBreakerAsync(
        //            // number of exceptions before breaking circuit
        //            5,
        //            // time circuit opened before retry
        //            TimeSpan.FromMinutes(1),
        //            (exception, duration) =>
        //            {
        //                // on circuit opened
        //                //_logger.LogTrace("Circuit breaker opened");
        //            },
        //            () =>
        //            {
        //                // on circuit closed
        //                //_logger.LogTrace("Circuit breaker reset");
        //            });

        //    return new List<Policy>
        //    {
        //        waitAndRetryPolicy,
        //        circuitBreakerPolicy
        //    };
        //}

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
