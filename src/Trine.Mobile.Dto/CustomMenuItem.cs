using Xamarin.Forms;

namespace Trine.Mobile.Dto
{
    public class CustomMenuItem
    {
        public string Title { get; set; }
        public string PageUri { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }
    }
}
