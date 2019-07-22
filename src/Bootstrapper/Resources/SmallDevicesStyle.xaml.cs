using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Bootstrapper.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmallDevicesStyle : ResourceDictionary
    {
        public static SmallDevicesStyle SharedInstance { get; } = new SmallDevicesStyle();
        

        public SmallDevicesStyle()
        {
            InitializeComponent();
        }
    }
}