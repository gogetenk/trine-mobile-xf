
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InformationBubble : ContentView
    {
        public string Title
        {
            get => lb_text.Text;
            set => lb_text.Text = value;
        }

        public InformationBubble()
        {
            InitializeComponent();
        }
    }
}