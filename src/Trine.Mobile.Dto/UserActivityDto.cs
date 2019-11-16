using System;

namespace Trine.Mobile.Dto
{
    public class UserActivityDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime? SignatureDate { get; set; }
    }
}