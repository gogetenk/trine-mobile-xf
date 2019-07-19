using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Model
{
    public class CreateInvitationRequestModel
    {
        public string Mail { get; set; }
        public string InviterId { get; set; }
        public string MissionId { get; set; }
    }
}
