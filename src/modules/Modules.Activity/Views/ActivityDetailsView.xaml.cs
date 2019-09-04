
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Activity.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [HotReloader.CSharpVisual]
    public partial class ActivityDetailsView : ContentPage
    {
        public ActivityDetailsView()
        {
            InitializeComponent();
        }
    }
}