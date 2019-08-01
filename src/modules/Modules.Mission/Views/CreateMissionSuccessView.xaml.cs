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
            
        }

        private async void ActivityIndicator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is null || e.PropertyName != "IsRunning")
                return;

            if ((sender as ActivityIndicator).IsRunning)
                return;

            await Task.WhenAll(sl_title.FadeTo(1d, 1000, Easing.CubicInOut), sl_title.TranslateTo(0, -10, 500, Easing.CubicInOut));
            await Task.WhenAll(grid_missionCard.FadeTo(1d, 1000, Easing.CubicInOut), grid_missionCard.TranslateTo(0, -10, 1000, Easing.CubicInOut));
            await frame_check.ScaleTo(1, 250, Easing.BounceOut);
            await Task.WhenAll(lb_caption.FadeTo(1d, 1000, Easing.CubicInOut), lb_caption.TranslateTo(0, -10, 1000, Easing.CubicInOut));
        }

        private async void Grid_missionCard_LayoutChanged(object sender, EventArgs e)
        {
            
        }

        private async void Grid_missionCard_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
    }
}