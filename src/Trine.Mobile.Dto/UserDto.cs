using System;
using System.ComponentModel;

namespace Trine.Mobile.Dto
{
    public class UserDto : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePicUrl { get; set; }
        public string Mail { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastLoginDate { get; set; }

        public string CompanySiret { get; set; }
        public CompanyDto Company { get; set; }
        public string DisplayName { get; set; }

        // Uniquement rempli dans le cas d'un membre 
        public string Role { get; set; }
        public bool IsDummy { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}