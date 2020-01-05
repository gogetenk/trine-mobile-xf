namespace Trine.Mobile.Model
{
    public class AbsenceModel
    {
        public ReasonEnum Reason { get; set; }
        public string Comment { get; set; }
    }

    public enum ReasonEnum
    {
        None,
        Formation,
        Holiday,
        Absent,
        Sickness,
        Recover
    }
}