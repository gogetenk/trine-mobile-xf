﻿namespace Trine.Mobile.Dto
{
    public class AbsenceDto
    {
        public ReasonEnum Reason { get; set; }
        public string Comment { get; set; }
    }
    public enum ReasonEnum
    {
        None,
        Absent,
        Formation,
        Holiday,
        Sickness,
        Recover
    }
}