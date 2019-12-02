using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Consultant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();

            frame_comment.IsVisible = false;
        }
    }
}