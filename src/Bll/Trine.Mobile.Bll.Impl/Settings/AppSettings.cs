using System.Collections.Generic;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Settings
{
    public sealed class AppSettings
    {
        public static AppSettings Instance { get; } = new AppSettings();

        public static TokenModel AccessToken { get; set; }
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
            ApiUrls.Add("GatewayApi", "https://app-assistance.azurewebsites.net");
            //ApiUrls.Add("GatewayApi", "https://gateway-dot-trine-dev.appspot.com");
            //ApiUrls.Add("GatewayApi", "http://localhost:5001/");
        }
    }
}
