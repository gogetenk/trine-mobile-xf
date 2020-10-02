using System.Collections.Generic;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IAppSettings
    {
        TokenModel AccessToken { get; set; }
        Dictionary<string, string> ApiUrls { get; set; }
        UserModel CurrentUser { get; set; }
    }
}
