using Android.Animation;
using Android.Content;
using Android.Support.V4.View;
using System.ComponentModel;
using Trine.Mobile.Bootstrapper.Droid.Renderers;
using Trine.Mobile.Components.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ElevationFrame), typeof(ElevationFrameRenderer))]
namespace Trine.Mobile.Bootstrapper.Droid.Renderers
{
    public class ElevationFrameRenderer : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public ElevationFrameRenderer(Context ctx) : base(ctx)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            try
            {
                // Setting shadow style
                SetOutlineSpotShadowColor(Color.Argb(100, 0, 0, 0));
                SetOutlineAmbientShadowColor(Color.Argb(100, 0, 0, 0));
                UpdateElevation();
            }
            catch
            {
                UpdateElevation();
                return;
            }

        }

        private void UpdateElevation()
        {
            var materialFrame = (ElevationFrame)Element;

            // we need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
            Control.StateListAnimator = new StateListAnimator();

            // set the elevation manually
            ViewCompat.SetElevation(this, materialFrame.Elevation);
            ViewCompat.SetElevation(Control, materialFrame.Elevation);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateElevation();
            }
        }
    }
}