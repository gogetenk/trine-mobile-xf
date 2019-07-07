using System;

namespace Trine.Mobile.Model
{
    public class GridDayModel
    {
        public DateTime Day { get; set; }
        public bool IsOpen { get; set; }
        public DayPartEnum WorkedPart { get; set; }
        public AbsenceModel Absence { get; set; }
    }

    public enum DayPartEnum
    {
        None,
        Morning,
        Afternoon,
        Full
    }
}
