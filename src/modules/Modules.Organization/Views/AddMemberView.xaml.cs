
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Organization.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMemberView : ContentPage
    {

        public AddMemberView()
        {
            InitializeComponent();

            //if (!Application.Current.Resources.ContainsKey("AccentColor"))
            //    return;

            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["AccentColor"];
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;
        }

    }
}