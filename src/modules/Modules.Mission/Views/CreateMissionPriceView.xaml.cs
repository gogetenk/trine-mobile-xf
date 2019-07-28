using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Mission.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]  
    public partial class CreateMissionPriceView : ContentPage
    {
        public CreateMissionPriceView()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lb_slider.Text = e.NewValue.ToString("0.##") + " %";
        }
    }
}