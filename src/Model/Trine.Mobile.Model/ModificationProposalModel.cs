using System;
using System.Collections.Generic;

namespace Trine.Mobile.Model
{
    public class ModificationProposalModel
    {
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
        public Status CurrentStatus { get; set; }
        public string UserId { get; set; }
        public List<GridDayModel> Modifications { get; set; }

        public enum Status
        {
            Pending,
            Accepted,
            Rejected
        }
    }
}
