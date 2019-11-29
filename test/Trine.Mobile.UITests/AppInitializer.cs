using Xamarin.UITest;

namespace Trine.Mobile.UITests
{
    public class AppInitializer
    {
        private const string _ApkFileName = "io.trine.trineapp.dev";
        private const string _IpaFileName = "io.trine.trineapp.dev";

        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.InstalledApp(_ApkFileName).StartApp();
            }

            return ConfigureApp.iOS.InstalledApp(_IpaFileName).StartApp();
        }
    }
}