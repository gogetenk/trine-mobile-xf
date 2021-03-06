﻿
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Consultant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityHistoryView : ContentPage
    {
        public ActivityHistoryView()
        {
            InitializeComponent();
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
#if __ANDROID__
            grid_header.TranslationY = -e.ScrollY / 2.5;
            grid_header_title.TranslationY = -e.ScrollY / 4;
#endif

            //grid_header.Opacity = Normalize(e.ScrollY, 0, 500, 1, 0);
        }

        private double Normalize(double val, double valmin, double valmax, double min, double max)
        {
            return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
        }
    }
}