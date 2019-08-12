
using Sharpnado.Presentation.Forms.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Organization.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MembersContentView : ContentView, IAnimatableReveal
    {
        public bool Animate { get; set; }

        public bool IsColoredHeader
        {
            get => frame_searchbar.BackgroundColor == Color.FromHex("#5A28D6");
            set
            {
                if (value)
                    frame_searchbar.BackgroundColor = Color.FromHex("#5A28D6");
                else
                    frame_searchbar.BackgroundColor = Color.Transparent;
            }
        }

        public MembersContentView()
        {
            InitializeComponent();
        }
    }
}