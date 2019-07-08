using static Trine.Mobile.Dto.OrganizationMemberDto;

namespace Trine.Mobile.Dto
{
    public class PartialOrganizationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsOwner { get; set; }
        public string Initials { get; set; }
        public RoleEnum UserRole { get; set; }
        public int MembersNb { get; set; }
        public int MissionsNb { get; set; }
    }
}
