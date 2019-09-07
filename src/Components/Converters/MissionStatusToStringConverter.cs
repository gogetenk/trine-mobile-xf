using System;
using System.Globalization;
using Trine.Mobile.Dto;
using Xamarin.Forms;
using static Trine.Mobile.Dto.MissionDto;

namespace Trine.Mobile.Components.Converters
{
    /// <summary>
    /// Converter that returns an explicit text for the welcome screen
    /// </summary>
    public class MissionStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (StatusEnum)value;
            return GetText(status);
        }

        private string GetText(StatusEnum status)
        {
            string statusName;
            switch (status)
            {
                case StatusEnum.CREATED:
                    statusName = "En attente des trois parties";
                    break;
                case StatusEnum.CONFIRMED:
                    statusName = "Prête à démarrer";
                    break;
                case StatusEnum.CANCELED:
                    statusName = "Refusée par l'un des membres";
                    break;
                default:
                    statusName = "error";
                    break;
            }
            return statusName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
