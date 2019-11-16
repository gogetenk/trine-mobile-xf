using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Trine.Mobile.Bootstrapper.Droid.Renderers;
using Trine.Mobile.Components.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MaterialEntry), typeof(MaterialEntryRenderer))]
namespace Trine.Mobile.Bootstrapper.Droid.Renderers
{
    public class MaterialEntryRenderer : EntryRenderer
    {
        public MaterialEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(Color.White);
                else
                    Control.Background.SetColorFilter(Color.White, PorterDuff.Mode.SrcAtop);
            }
        }
    }
}