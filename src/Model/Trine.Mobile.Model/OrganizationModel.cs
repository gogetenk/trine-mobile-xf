using System;
using System.Collections.Generic;

namespace Trine.Mobile.Model
{
    public class OrganizationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public List<OrganizationMemberModel> Members { get; set; }
        public string Icon { get; set; }
        public DateTime Created { get; set; }
    }
}
