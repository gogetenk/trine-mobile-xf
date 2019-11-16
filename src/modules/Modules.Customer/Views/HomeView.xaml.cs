
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Customer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            grid_header.TranslationY = -e.ScrollY / 2.5;
            grid_header_title.TranslationY = -e.ScrollY / 4;

            //grid_header.Opacity = Normalize(e.ScrollY, 0, 500, 1, 0);
        }

        private double Normalize(double val, double valmin, double valmax, double min, double max)
        {
            return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
        }
    }
}