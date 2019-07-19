using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Model
{
    public class InviteModel
    {
        public string Id { get; set; }
        public Guid Code { get; set; }
        // Partial object
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        // Champ rempli uniquement si l'invite concerne une mission. TODO: Penser à donner le rôle de membre au lieu de guest
        public string MissionId { get; set; }
        public string InviterId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public string GuestEmail { get; set; }
    }
}
