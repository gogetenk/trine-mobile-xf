using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Authentication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Signup3View : ContentPage
    {
        public Signup3View()
        {
            InitializeComponent();
        }

        //private void SetInactiveColors(Frame frame)
        //{
        //    frame.BackgroundColor = Color.White;
        //    frame.BorderColor = Color.FromHex("#DCDEE6");
        //    foreach (var label in (frame.Content as StackLayout).Children)
        //    {
        //        (label as Label).TextColor = Color.FromHex("#000000");
        //    }
        //}

        //private void SetActiveColors(Frame frame)
        //{
        //    frame.BackgroundColor = Color.FromHex("#5A28D6");
        //    frame.BorderColor = Color.White;
        //    foreach (var label in (frame.Content as StackLayout).Children)
        //    {
        //        (label as Label).TextColor = Color.White;
        //    }
        //}

        //private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        //{
        //    if (sender is null || (sender as Frame) == null)
        //        return;

        //    var frame = sender as Frame;
        //    if (frame.BackgroundColor == Color.White)
        //        SetActiveColors(frame);
        //    else
        //        SetInactiveColors(frame);
        //}
    }
}