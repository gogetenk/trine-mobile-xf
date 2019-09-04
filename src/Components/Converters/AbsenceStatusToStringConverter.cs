using System;
using System.Globalization;
using Trine.Mobile.Dto;
using Xamarin.Forms;

namespace Trine.Mobile.Components.Converters
{
    public class AbsenceStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetStatusTranscription((ReasonEnum)value);
        }

        private string GetStatusTranscription(ReasonEnum status)
        {
            string statusString;

            switch (status)
            {
                case ReasonEnum.Absent:
                    statusString = "Autre";
                    break;
                case ReasonEnum.Formation:
                    statusString = "Formation";
                    break;
                case ReasonEnum.Holiday:
                    statusString = "Congé";
                    break;
                case ReasonEnum.Recover:
                    statusString = "Récupération";
                    break;
                case ReasonEnum.Sickness:
                    statusString = "Maladie";
                    break;
                default:
                    statusString = "Autre";
                    break;
            }

            return statusString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetRevertTranscription((string)value);
        }

        private ReasonEnum GetRevertTranscription(string status)
        {
            ReasonEnum statusEnum;

            switch (status)
            {
                case "Autre":
                    statusEnum = ReasonEnum.Absent;
                    break;
                case "Formation":
                    statusEnum = ReasonEnum.Formation;
                    break;
                case "Congé":
                    statusEnum = ReasonEnum.Holiday;
                    break;
                case "Récupération":
                    statusEnum = ReasonEnum.Recover;
                    break;
                case "Maladie":
                    statusEnum = ReasonEnum.Sickness;
                    break;
                default:
                    statusEnum = ReasonEnum.Absent;
                    break;
            }

            return statusEnum;
        }
    }
}
