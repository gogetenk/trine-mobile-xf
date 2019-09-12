using System;
using System.Collections.Generic;
using System.Text;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface ISupportService
    {
        void ShowMessenger();
        void ShowHelpCenter();
        void RegisterUser(UserModel user);
        void RegisterAnonymousUser();
        void Logout();
    }
}
