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
        protected CustomApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            new Instabug
                .Builder(this, "ee39ba65bc0171ea932b98e05acab1f2")
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