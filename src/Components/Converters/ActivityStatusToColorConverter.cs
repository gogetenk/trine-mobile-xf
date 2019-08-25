using System;
using System.Globalization;
using Trine.Mobile.Dto;
using Xamarin.Forms;

namespace Trine.Mobile.Components.Converters
{
    public class ActivityStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetStatusTranscription((ActivityStatusEnum)value);
        }

        private Color GetStatusTranscription(ActivityStatusEnum status)
        {
            Color color;

            switch (status)
            {
                case ActivityStatusEnum.Generated:
                    color = Color.FromHex("#F0B429");
                    break;
                case ActivityStatusEnum.ConsultantSigned:
                case ActivityStatusEnum.CustomerSigned:
                    color = Color.FromHex("#3EBD93");
                    break;
                default:
                    color = Color.FromHex("#F0B429");
                    break;
            }

            return color;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
