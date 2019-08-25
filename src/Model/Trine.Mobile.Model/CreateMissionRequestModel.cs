using System;

namespace Trine.Mobile.Model
{
    public class CreateMissionRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float DailyPrice { get; set; }
        public float CommercialFeePercentage { get; set; }
        public bool IsTripartite { get; set; }
        public bool IsFreelance { get; set; }
        public FrequencyEnum PaymentFrequency { get; set; }

        // FKs
        public UserModel Commercial { get; set; }
        public UserModel Consultant { get; set; }
        public UserModel Customer { get; set; }
        public string OrganizationId { get; set; }
        public string ProjectName { get; set; }
        public string OwnerId { get; set; }

        public enum FrequencyEnum
        {
            DAILY,
            WEEKLY,
            MONTHLY,
            ANNUALLY,
            ONTERM
        }
    }
}
