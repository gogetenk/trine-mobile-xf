using Binding.Intercom.iOS;
using Com.OneSignal;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using InstabugLib;
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
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            InitializeLibs();

            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }

        private static void InitializeLibs()
        {
            try
            {
                ImageCircleRenderer.Init();
                FormsMaterial.Init();
                SharpnadoInitializer.Initialize(enableInternalLogger: true);
                Instabug.StartWithToken("ee39ba65bc0171ea932b98e05acab1f2", IBGInvocationEvent.Shake | IBGInvocationEvent.TwoFingersSwipeLeft | IBGInvocationEvent.Screenshot);
                Instabug.ShouldCaptureViewHierarchy = true;
                Instabug.TintColor = UIColor.FromHSB(257, 81, 84);
                Instabug.SetLocale(IBGLocale.French);
                Instabug.SetWelcomeMessageMode(IBGWelcomeMessageMode.WelcomeMessageModeDisabled); // Disable welcome message
                // Disable the Replies. If disabled, the chats list button is removed from Instabug's prompt, the in-app notifications are disabled, and manually showing the chats list doesn't have an effect. 
                IBGReplies.Enabled = false;

                // Intercom
                Intercom.SetApiKey("ios_sdk-be15dd592c71d03d0f5245caece3fd0a40ae6435", "v4l26lv4");

                // OneSignal
                OneSignal
                   .Current
                   .StartInit("12785512-a98b-4c91-89ca-05959a685120")
                   .EndInit();
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
