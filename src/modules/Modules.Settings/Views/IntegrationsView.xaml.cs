using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Settings.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntegrationsView : ContentPage
    {
        public IntegrationsView()
        {
            InitializeComponent();

            img.Source = ImageSource.FromResource("Modules.Settings.Assets.integrations.png", typeof(NotificationsView).GetTypeInfo().Assembly);
            //SettingsModule.navPageInstance.BarBackgroundColor = Color.FromHex("#FFFFFF");
        }
    }
}