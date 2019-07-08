namespace Trine.Mobile.Dto
{
    public class AbsenceDto
    {
        public ReasonEnum Reason { get; set; }
        public string Comment { get; set; }
    }
    public enum ReasonEnum
    {
        Formation,
        Holiday,
        Absent,
        Sickness,
        Recover
    }
}