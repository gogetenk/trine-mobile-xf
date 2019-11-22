using System;
using System.Globalization;
using Trine.Mobile.Dto;
using Xamarin.Forms;

namespace Trine.Mobile.Components.Converters
{
    public class ActivityStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Can consultant modify ?
            return value != null && (((ActivityStatusEnum)value) == ActivityStatusEnum.Generated || ((ActivityStatusEnum)value) == ActivityStatusEnum.ModificationsRequired);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
