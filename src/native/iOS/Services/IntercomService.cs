using Binding.Intercom.iOS;
using Foundation;
using System;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Model;

namespace Trine.Mobile.iOS.Services
{
    public class IntercomService : ISupportService
    {
        public void RegisterAnonymousUser()
        {
            Intercom.RegisterUnidentifiedUser();
        }

        public void RegisterUser(UserModel user)
        {
            if (user == null)
            {
                Intercom.RegisterUnidentifiedUser();
                return;
            }

            var currentUser = AppSettings.CurrentUser;

            ICMCompany company = null;

            if (currentUser.Company != null)
            {

                company = new ICMCompany();
                company.Name = currentUser.Company.Name;
                company.CompanyId = currentUser.Company.Id;
            }

            var attributes = new ICMUserAttributes();
            attributes.Name = currentUser.Firstname;
            attributes.Email = currentUser.Mail;
            attributes.UserId = currentUser.Id;
            attributes.Phone = currentUser.PhoneNumber;
            attributes.SignedUpAt = DateTimeToNSDate(currentUser.SubscriptionDate);

            var companies = new ICMCompany[1];
            companies[0] = company;
            attributes.Companies = companies;

            Intercom.UpdateUser(attributes);
            Intercom.RegisterUserWithUserId(user.Id, user.Mail);
            //Intercom.HandleIntercomPushNotification();
        }

        private NSDate DateTimeToNSDate(DateTime date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }

        public void ShowHelpCenter()
        {
            Intercom.PresentHelpCenter();
        }

        public void ShowMessenger()
        {
            Intercom.PresentMessenger();
        }

        public void Logout()
        {
            Intercom.Logout();
        }
    }
}