using System;
using System.Globalization;
using Trine.Mobile.Dto;
using Xamarin.Forms;

namespace Trine.Mobile.Components.Converters
{
    public class IsActivityRefusedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((ActivityStatusEnum)value) == ActivityStatusEnum.ModificationsRequired);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
