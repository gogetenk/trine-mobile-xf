using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using InstabugLib;
using Prism;
using Prism.Ioc;
using Sharpnado.Presentation.Forms.iOS;
using System;
using Trine.Mobile.Bootstrapper;
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
                Instabug.StartWithToken("ee39ba65bc0171ea932b98e05acab1f2", IBGInvocationEvent.Shake | IBGInvocationEvent.TwoFingersSwipeLeft);
                Instabug.ShouldCaptureViewHierarchy = true;
                // Disable the Replies. If disabled, the chats list button is removed from Instabug's prompt, the in-app notifications are disabled, and manually showing the chats list doesn't have an effect. 
                IBGReplies.Enabled = false;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception while initializing app: {exception.Message}");
                throw;
            }
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
