using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Trine.Mobile.Dto
{
    public class ActivityDto
    {
        public string Id { get; set; }
        public string MissionId { get; set; }
        public UserActivityDto Commercial { get; set; }
        public UserActivityDto Customer { get; set; }
        public UserActivityDto Consultant { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ActivityStatusEnum Status { get; set; }
        public List<GridDayDto> Days { get; set; }
        public bool CanSign { get; set; }
        public bool CanModify { get; set; }
        public List<ModificationProposalDto> ModificationProposals { get; set; }

        // UI only 
        public Color PinColor { get; set; }
        public float DaysNb { get; set; }
        public string TranscodedStatus { get; set; }
    }

    public enum ActivityStatusEnum
    {
        Generated,
        ConsultantSigned,
        ModificationsRequired,
        CustomerSigned
    }
}
