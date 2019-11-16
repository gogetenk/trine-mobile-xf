using Android.Content;
using Android.Graphics;
using System;
using System.IO;
using Trine.Mobile.Bootstrapper.Droid.Renderers;
using Trine.Mobile.Components.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ActivityCalendarView), typeof(CalendarRenderer))]
namespace Trine.Mobile.Bootstrapper.Droid.Renderers
{
    public class CalendarRenderer : ViewRenderer<ActivityCalendarView, global::Android.Views.View>
    {
        public CalendarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ActivityCalendarView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                e.NewElement.OnDrawBitmap += NewElement_OnDrawBitmap;
            }
        }

        private void NewElement_OnDrawBitmap(object sender, EventArgs e)
        {
            var calendarView = sender as ActivityCalendarView;
            if (this.ViewGroup is null || calendarView is null)
                return;

            // If we are not in readonly, we dont generate image.
            if (calendarView.IsInputEnabled)
                return;

            //get the subview
            global::Android.Views.View subView = ViewGroup.GetChildAt(0);
            int width = ViewGroup.Width;
            int height = ViewGroup.Height;

            //create and draw the bitmap
            Bitmap b = Bitmap.CreateBitmap(800, 800, Bitmap.Config.Argb8888);
            Canvas c = new Canvas(b);
            ViewGroup.Draw(c);

            //save the bitmap to file
            var bytes = SaveBitmapToBytes(b);
            var base64 = Convert.ToBase64String(bytes);
            calendarView.SetImage(bytes);
        }

        private byte[] SaveBitmapToBytes(Bitmap bm)
        {
            MemoryStream ms = new MemoryStream();
            bm.Compress(Bitmap.CompressFormat.Png, 100, ms);
            return ms.ToArray();
        }
    }
}