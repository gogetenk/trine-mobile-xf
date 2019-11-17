using System;
using System.Globalization;
using Trine.Mobile.Dto;
using Xamarin.Forms;

namespace Trine.Mobile.Components.Converters
{
    public class IsActivityCompletedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Can consultant modify ?
            return (((ActivityStatusEnum)value) == ActivityStatusEnum.CustomerSigned);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
