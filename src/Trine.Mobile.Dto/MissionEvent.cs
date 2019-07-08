using System;

namespace Trine.Mobile.Dto
{
    public class MissionEvent
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public EventTypeEnum EventType { get; set; }
        public string Text { get; set; }
        public string Actor { get; set; }
        public string PinSource { get; set; }
        public enum EventTypeEnum
        {
            Informative = 0,
            Alert = 1
        }
    }
}