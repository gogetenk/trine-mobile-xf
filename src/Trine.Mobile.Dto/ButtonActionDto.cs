using System.Windows.Input;

namespace Trine.Mobile.Dto
{
    public class ButtonActionDto
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public ICommand ActionCommand { get; set; }
    }
}
