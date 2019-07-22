using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Bootstrapper.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralDevicesStyle : ResourceDictionary
    {
        public static GeneralDevicesStyle SharedInstance { get; } = new GeneralDevicesStyle();
        

        public GeneralDevicesStyle()
        {
            InitializeComponent();
        }


    }
}