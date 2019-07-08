using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Trine.Mobile.Dto
{
    public class MenuItemDto
    {
        public string Icon { get; set; }
        public string Text { get; set; }
        public string TargetUri { get; set; }
        public Color TextColor { get; set; }
    }
}
