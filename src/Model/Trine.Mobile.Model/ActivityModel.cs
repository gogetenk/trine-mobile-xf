using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Model
{
    public class ActivityModel
    {
        public string Id { get; set; }
        public string MissionId { get; set; }
        public UserActivityModel Commercial { get; set; }
        public UserActivityModel Customer { get; set; }
        public UserActivityModel Consultant { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ActivityStatusEnum Status { get; set; }
        public List<GridDayModel> Days { get; set; }
        public bool CanSign { get; set; }
        public bool CanModify { get; set; }
    }

    public enum ActivityStatusEnum
    {
        Generated,
        ConsultantSigned,
        CustomerSigned
    }
}
