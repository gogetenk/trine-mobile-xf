
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Settings.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsView : ContentPage
    {
        public NotificationsView()
        {
            InitializeComponent();

            img.Source = ImageSource.FromResource("Modules.Settings.Assets.notifications.png", typeof(NotificationsView).GetTypeInfo().Assembly);
        }
    }
}