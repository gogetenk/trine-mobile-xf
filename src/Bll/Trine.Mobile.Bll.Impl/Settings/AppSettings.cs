using System.Collections.Generic;
using Newtonsoft.Json;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Settings
{
    public class AppSettings : IAppSettings
    {
        private readonly ISecureStorage _secureStorage;
        public Dictionary<string, string> ApiUrls { get; set; }
        public UserModel CurrentUser { get; set; }

        private TokenModel _accesstoken;
        public TokenModel AccessToken
        {
            get
            {
                if (_accesstoken != null)
                    return _accesstoken;

                _accesstoken = JsonConvert.DeserializeObject<TokenModel>(_secureStorage.GetAsync(CacheKeys._CurrentToken).Result);
                if (_accesstoken is null)
                    throw new TechnicalException("Authentication error. You have to reconnect to the app.");

                return _accesstoken;
            }
            set
            {
                _secureStorage.SetAsync(CacheKeys._CurrentToken, JsonConvert.SerializeObject(value)).GetAwaiter().GetResult();
            }
        }

        public AppSettings(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }
    }
}
