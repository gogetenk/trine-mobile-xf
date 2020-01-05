using Binding.Intercom.iOS;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using InstabugLib;
using MultiGestureViewPlugin.iOS;
using Plugin.DownloadManager;
using Prism;
using Prism.Ioc;
using Sharpnado.Presentation.Forms.iOS;
using System;
using Trine.Mobile.Bll;
using Trine.Mobile.Bootstrapper;
using Trine.Mobile.iOS.Services;
using UIKit;
using Xamarin.Forms;
namespace Trine.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // DEV ONLY
        //private const string instabugToken = "ee39ba65bc0171ea932b98e05acab1f2";
        //private const string intercomToken = "nb4w5hji";
        //private const string intercomApiKey = "ios_sdk-b655535b4f9815c9ead33d059c2cb43020d9431d";
        // PROD ONLY
        private const string instabugToken = "2be1819819cfe4eda2b908b5bba59b73";
        private const string intercomToken = "v4l26lv4";
        private const string intercomApiKey = "ios_sdk-be15dd592c71d03d0f5245caece3fd0a40ae6435";

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            InitializeLibs();

            LoadApplication(new App(new iOSInitializer()));

            #if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            #endif

            return base.FinishedLaunching(app, options);
        }

        private static void InitializeLibs()
        {
            try
            {
                // For linker only
                MultiGestureViewRenderer multiGestureViewRenderer = new MultiGestureViewRenderer();

                ImageCircleRenderer.Init();
                FormsMaterial.Init();
                SharpnadoInitializer.Initialize(enableInternalLogger: true);

                Instabug.StartWithToken(instabugToken, IBGInvocationEvent.Shake | IBGInvocationEvent.TwoFingersSwipeLeft | IBGInvocationEvent.Screenshot);

                Instabug.ShouldCaptureViewHierarchy = true;
                Instabug.TintColor = UIColor.FromHSB(257, 81, 84);
                Instabug.SetLocale(IBGLocale.French);
                Instabug.SetWelcomeMessageMode(IBGWelcomeMessageMode.WelcomeMessageModeDisabled); // Disable welcome message
                // Disable the Replies. If disabled, the chats list button is removed from Instabug's prompt, the in-app notifications are disabled, and manually showing the chats list doesn't have an effect. 
                IBGReplies.Enabled = false;

                // Intercom
                Intercom.SetApiKey(intercomApiKey, intercomToken);


            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception while initializing app: {exception.Message}");
                throw;
            }
        }


        //The following Exports are needed to run OneSignal in the iOS Simulator.
        //  The simulator doesn't support push however this prevents a crash due to a Xamarin bug
        //   https://bugzilla.xamarin.com/show_bug.cgi?id=52660
        [Export("oneSignalApplicationDidBecomeActive:")]
        public void OneSignalApplicationDidBecomeActive(UIApplication application)
        {
            // Remove line if you don't have a OnActivated method.
            OnActivated(application);
        }

        [Export("oneSignalApplicationWillResignActive:")]
        public void OneSignalApplicationWillResignActive(UIApplication application)
        {
            // Remove line if you don't have a OnResignActivation method.
            OnResignActivation(application);
        }

        [Export("oneSignalApplicationDidEnterBackground:")]
        public void OneSignalApplicationDidEnterBackground(UIApplication application)
        {
            // Remove line if you don't have a DidEnterBackground method.
            DidEnterBackground(application);
        }

        [Export("oneSignalApplicationWillTerminate:")]
        public void OneSignalApplicationWillTerminate(UIApplication application)
        {
            // Remove line if you don't have a WillTerminate method.
            WillTerminate(application);
        }

        // Note: Similar exports are needed if you add other AppDelegate overrides.


        /**
         * Save the completion-handler we get when the app opens from the background.
         * This method informs iOS that the app has finished all internal processing and can sleep again.
         */
        public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
        {
            CrossDownloadManager.BackgroundSessionCompletionHandler = completionHandler;
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<ISupportService, IntercomService>();
        }
    }
}
