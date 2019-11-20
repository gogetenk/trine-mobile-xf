using Android.Content;
using Trine.Mobile.Bootstrapper.Droid.Renderers;
using Trine.Mobile.Components.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(Frame), typeof(SoftShadowFrameRenderer), new[] { typeof(SoftShadowFrameVisual) })]
namespace Trine.Mobile.Bootstrapper.Droid.Renderers
{
    public class SoftShadowFrameRenderer : MaterialFrameRenderer
    {
        public SoftShadowFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            // Shadow
            Control.SetOutlineAmbientShadowColor(Color.Black);
            Control.SetOutlineSpotShadowColor(Color.Black);
            Control.SetElevation(5);
        }
    }
}