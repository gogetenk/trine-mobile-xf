using System;
using System.Globalization;
using Xamarin.Forms;
using static Trine.Mobile.Dto.MissionDto;

namespace Trine.Mobile.Components.Converters
{
    /// <summary>
    /// Converter that returns true only if the mission is confirmed
    /// </summary>
    public class MissionStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (StatusEnum)value;
            return (status == StatusEnum.CONFIRMED);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
