using System;

namespace Trine.Mobile.Model
{
    public class UserActivityModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime? SignatureDate { get; set; }
        public string SignatureUri { get; set; }
    }
}