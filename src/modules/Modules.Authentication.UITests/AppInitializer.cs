using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.InstalledApp("io.trine.trineapp.dev").StartApp();
            }

            return ConfigureApp.iOS.InstalledApp("io.trine.trineapp.dev").StartApp();
        }
    }
}