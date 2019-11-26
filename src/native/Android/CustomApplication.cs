using Android.App;
using Android.Graphics;
using Android.Runtime;
using Com.Instabug.Library;
using Com.Instabug.Library.Core;
using Com.Instabug.Library.Invocation;
using Com.Instabug.Library.UI.Onboarding;
using Java.Util;
using System;

namespace Trine.Mobile.Bootstrapper.Droid
{
    [Application]
    public class CustomApplication : Application
    {
        // DEV ONLY
        //private const string instabugToken = "ee39ba65bc0171ea932b98e05acab1f2";
        //private const string intercomToken = "nb4w5hji";
        //private const string intercomApiKey = "android_sdk-595dfe1547495f27cbdfe8485bfd2909f93dc1cd";
        // PROD ONLY
        private const string instabugToken = "2be1819819cfe4eda2b908b5bba59b73";
        private const string intercomToken = "v4l26lv4";
        private const string intercomApiKey = "android_sdk-131fedc98cf2a3bcf9c4b8ac0a64f49cb739bb90";

        protected CustomApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            //Intercom.Initialize(this, intercomApiKey, intercomToken);
            new Instabug
                .Builder(this, instabugToken)
                .SetInvocationEvents(InstabugInvocationEvent.Shake, InstabugInvocationEvent.TwoFingerSwipeLeft, InstabugInvocationEvent.Screenshot)
                .Build();
            Instabug.SetViewHierarchyState(Feature.State.Enabled);
            InstabugCore.SetRepliesState(Feature.State.Disabled);
            Instabug.PrimaryColor = Color.ParseColor("#5A28D6");
            Instabug.SetLocale(Locale.French);
            Instabug.SetWelcomeMessageState(WelcomeMessage.State.Disabled);
        }
    }
}