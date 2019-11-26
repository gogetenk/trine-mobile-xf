using Java.Lang;
using Trine.Mobile.Bll;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bootstrapper.Droid.Services
{
    public class IntercomService : ISupportService
    {
        public void RegisterAnonymousUser()
        {
            //Intercom.Client().RegisterUnidentifiedUser();
        }

        public void RegisterUser(UserModel user)
        {
            //Registration registration;

            //if (user == null)
            //{
            //    Intercom.Client().RegisterUnidentifiedUser();
            //    return;
            //}

            //var currentUser = user;
            //Company company = null;

            //if (currentUser.Company != null)
            //    company = new Company.Builder()
            //        .WithName(currentUser.Company.Name)
            //        .WithCompanyId(currentUser.Company.Id)
            //        .Build();

            //var attributes = new UserAttributes.Builder()
            //    .WithName(currentUser.Firstname)
            //    .WithEmail(currentUser.Mail)
            //    .WithUserId(currentUser.Id)
            //    .WithPhone(currentUser.PhoneNumber)
            //    .WithSignedUpAt((Long)Long.ParseLong(currentUser.SubscriptionDate.Ticks.ToString()))
            //    .WithCompany(company)
            //    .Build();

            //registration = Registration.Create().WithUserId(currentUser.Id).WithUserAttributes(attributes);
            //Intercom.Client().RegisterIdentifiedUser(registration);

            //Intercom.Client().HandlePushMessage();
        }

        public void ShowHelpCenter()
        {
            //Intercom.Client().DisplayHelpCenter();
        }

        public void ShowMessenger()
        {
            //Intercom.Client().DisplayMessenger();
        }

        public void Logout()
        {
            //Intercom.Client().Logout();
        }
    }
}