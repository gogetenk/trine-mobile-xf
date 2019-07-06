using System.Reflection;
using Xamarin.Forms;

namespace Modules.Authentication.Views
{
    public partial class SignupView : ContentPage
    {
        public SignupView()
        {
            InitializeComponent();

            img.Source = ImageSource.FromResource("Modules.Authentication.Assets.logo.png", typeof(SignupView).GetTypeInfo().Assembly);
        }
    }
}