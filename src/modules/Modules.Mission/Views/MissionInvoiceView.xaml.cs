using Sharpnado.Presentation.Forms.CustomViews;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Mission.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MissionInvoiceView : ContentView, IAnimatableReveal
    {
        public bool Animate { get; set; }

        public MissionInvoiceView()
        {
            InitializeComponent();
        }
    }
}