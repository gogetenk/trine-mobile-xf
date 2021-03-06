﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Com.OneSignal;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using Trine.Mobile.Bll;
using Trine.Mobile.Bootstrapper;
using Trine.Mobile.Bootstrapper.Droid.Services;
using Xamarin.Forms;

namespace Trine.Mobile.Android
{
    [Activity(Label = "Trine", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Trine.Mobile.Bootstrapper.Droid.Resource.Layout.Tabbar;
            ToolbarResource = Trine.Mobile.Bootstrapper.Droid.Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ImageCircleRenderer.Init();
            FormsMaterial.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            OneSignal
                .Current
                .StartInit("overriden")
                .EndInit();

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<ISupportService, IntercomService>();
        }
    }
}

