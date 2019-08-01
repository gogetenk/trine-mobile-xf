using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Mission.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateMissionSuccessView : ContentPage
    {
        public CreateMissionSuccessView()
        {
            InitializeComponent();
            Appearing += CreateMissionSuccessView_AppearingAsync;
        }

        private async void CreateMissionSuccessView_AppearingAsync(object sender, EventArgs e)
        {
            await Task.WhenAll(sl_title.FadeTo(1d, 1000, Easing.CubicInOut), sl_title.TranslateTo(0, -10, 500, Easing.CubicInOut));
            await Task.WhenAll(grid_missionCard.FadeTo(1d, 1000, Easing.CubicInOut), grid_missionCard.TranslateTo(0, -10, 1000, Easing.CubicInOut));
            await frame_check.ScaleTo(1, 250, Easing.BounceOut);
            await Task.WhenAll(lb_caption.FadeTo(1d, 1000, Easing.CubicInOut), lb_caption.TranslateTo(0, -10, 1000, Easing.CubicInOut));
        }
    }
}