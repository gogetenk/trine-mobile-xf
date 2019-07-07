using System;
using System.Collections.Generic;
using System.Text;
using static Trine.Mobile.Model.OrganizationMemberModel;

namespace Trine.Mobile.Model
{
    public class PartialOrganizationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsOwner { get; set; }
        public RoleEnum UserRole { get; set; }
        public int MembersNb { get; set; }
        public int MissionsNb { get; set; }
    }
}
