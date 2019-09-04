using System;

namespace Trine.Mobile.Dto
{
    public class InviteDto
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
        public string GuestEmail { get; set; }
    }
}
