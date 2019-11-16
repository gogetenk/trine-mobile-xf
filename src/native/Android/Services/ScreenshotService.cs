using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using Trine.Mobile.Bll;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;
using Color = Android.Graphics.Color;

namespace Trine.Mobile.Bootstrapper.Droid.Services
{
    public class ScreenshotService : IScreenshotService
    {
        private Activity _currentActivity;
        public void SetActivity(Activity activity) => _currentActivity = activity;

        public byte[] Capture()
        {
            var rootView = _currentActivity.Window.DecorView.RootView;

            using (var screenshot = Bitmap.CreateBitmap(
                                    rootView.Width,
                                    rootView.Height,
                                    Bitmap.Config.Argb8888))
            {
                var canvas = new Canvas(screenshot);
                rootView.Draw(canvas);

                using (var stream = new MemoryStream())
                {
                    screenshot.Compress(Bitmap.CompressFormat.Png, 90, stream);
                    return stream.ToArray();
                }
            }
        }

        public static ViewGroup ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
        {
            var vRenderer = Platform.CreateRenderer(view);
            var viewGroup = vRenderer.ViewGroup;
            vRenderer.Tracker.UpdateLayout();
            var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);
            viewGroup.LayoutParameters = layoutParams;
            view.Layout(size);
            viewGroup.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup;
        }

        private void SaveImage(Bitmap bitmap)
        {
            var sdCardPath = "";
            var fileName = System.IO.Path.Combine(sdCardPath, DateTime.Now.ToFileTime() + ".png");
            using (var os = new FileStream(fileName, FileMode.CreateNew))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 95, os);
            }

            Toast.MakeText(Application.Context, "Image saved Successfully..!  at " + fileName, ToastLength.Long).Show();
        }


        private Bitmap ConvertViewToBitMap(ViewGroup view)
        {
            Bitmap bitmap = Bitmap.CreateBitmap(1000, 1600, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            canvas.DrawColor(Color.White);
            view.Draw(canvas);
            return bitmap;
        }

        //public Bitmap CreateBitmapFromView(View view, bool autoScale = true)
        //{
        //    var wasDrawingCacheEnabled = view.DrawingCacheEnabled;
        //    view.DrawingCacheEnabled = true;
        //    view.BuildDrawingCache(autoScale);
        //    var bitmap = view.GetDrawingCache(autoScale);
        //    view.DrawingCacheEnabled = wasDrawingCacheEnabled;
        //    return bitmap;
        //}
    }
}