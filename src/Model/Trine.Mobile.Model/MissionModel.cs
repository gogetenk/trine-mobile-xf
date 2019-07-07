using System;
using System.Collections.Generic;

namespace Trine.Mobile.Model
{
    public class MissionModel
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float DailyPrice { get; set; }
        public float CommercialFeePercentage { get; set; }
        public FrameContractModel FrameContract { get; set; }
        public StatusEnum Status { get; set; }
        public bool IsTripartite { get; set; }
        public bool IsFreelance { get; set; }
        public FrequencyEnum PaymentFrequency { get; set; }

        public UserModel Commercial { get; set; }
        public UserModel Consultant { get; set; }
        public UserModel Customer { get; set; }

        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationIcon { get; set; }
        public string ProjectName { get; set; }
        public string OwnerId { get; set; }

        public List<InvoiceModel> Invoices { get; set; }
        public List<ActivityModel> Activities { get; set; }
        public List<EventModel> Events { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime CanceledDate { get; set; }

        public enum StatusEnum
        {
            CREATED,
            CONFIRMED,
            CANCELED
        }
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
