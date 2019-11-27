using Newtonsoft.Json;
using Sogetrel.Sinapse.Framework.Exceptions;
using System.Collections.Generic;
using Trine.Mobile.Model;
using Xamarin.Essentials;

namespace Trine.Mobile.Bll.Impl.Settings
{
    public sealed class AppSettings
    {
        public static AppSettings Instance { get; } = new AppSettings();

        private static TokenModel _accesstoken;
        public static TokenModel AccessToken
        {
            get
            {
                if (_accesstoken != null)
                    return _accesstoken;

                _accesstoken = JsonConvert.DeserializeObject<TokenModel>(SecureStorage.GetAsync(CacheKeys._CurrentToken).Result);
                if (_accesstoken is null)
                    throw new TechnicalException("Authentication error. You have to reconnect to the app.");

                return _accesstoken;
            }
            set
            {
                SecureStorage.SetAsync(CacheKeys._CurrentToken, JsonConvert.SerializeObject(value)).GetAwaiter().GetResult();
            }
        }
        public static Dictionary<string, string> ApiUrls { get; private set; }
        public static UserModel CurrentUser { get; set; }

        public static string GatewayApi = "GatewayApi";

        //public static MobileServiceClient CurrentClient => new MobileServiceClient("https://app-assistance.azurewebsites.net");

        public static string FcmToken { get; set; }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AppSettings()
        {
        }

        private AppSettings()
        {
            ApiUrls = new Dictionary<string, string>();
            ApiUrls.Add("GatewayApi", "https://app-assistance-dev.azurewebsites.net");
        }
    }
}
