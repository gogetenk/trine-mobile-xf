using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrineLogoHeaderView : ContentView
    {
        public TrineLogoHeaderView()
        {
            InitializeComponent();

            img.Source = ImageSource.FromResource("Trine.Mobile.Components.Assets.logo.png", typeof(TrineLogoHeaderView).GetTypeInfo().Assembly);
        }
    }
}