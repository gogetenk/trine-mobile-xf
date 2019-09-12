using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Settings.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpView : ContentPage
    {
        public HelpView()
        {
            InitializeComponent();

            img.Source = ImageSource.FromResource("Modules.Settings.Assets.trine.png", typeof(NotificationsView).GetTypeInfo().Assembly);
        }
    }
}