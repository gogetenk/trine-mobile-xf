
using Sharpnado.Presentation.Forms.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Organization.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MembersContentView : ContentView, IAnimatableReveal
    {
        public MembersContentView()
        {
            InitializeComponent();
        }
        public bool Animate { get; set; }
    }
}