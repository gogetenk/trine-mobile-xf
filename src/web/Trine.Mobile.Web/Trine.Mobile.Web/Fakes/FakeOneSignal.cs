using System.Collections.Generic;
using Com.OneSignal.Abstractions;

namespace Trine.Mobile.Web.Fakes
{
    public class FakeOneSignal : IOneSignal
    {
        public void AddTrigger(string key, object value)
        {
        }

        public void AddTriggers(Dictionary<string, object> triggers)
        {
        }

        public void ClearAndroidOneSignalNotifications()
        {
        }

        public void DeleteTag(string key)
        {
        }

        public void DeleteTags(IList<string> keys)
        {
        }

        public void GetTags(TagsReceived tagsReceivedDelegate)
        {
        }

        public object GetTriggerValueForKey(string key)
        {
            return default(object);
        }

        public void IdsAvailable(IdsAvailableCallback idsAvailableCallback)
        {
        }

        public void LogoutEmail(OnSetEmailSuccess success, OnSetEmailFailure failure)
        {
        }

        public void LogoutEmail()
        {
        }

        public void PauseInAppMessages(bool pause)
        {
        }

        public void PostNotification(Dictionary<string, object> data, OnPostNotificationSuccess success, OnPostNotificationFailure failure)
        {
        }

        public void PostNotification(Dictionary<string, object> data)
        {
        }

        public void PromptLocation()
        {
        }

        public void RegisterForPushNotifications()
        {
        }

        public void RemoveExternalUserId()
        {
        }

        public void RemoveTriggerForKey(string key)
        {
        }

        public void RemoveTriggersForKeys(List<string> keys)
        {
        }

        public bool RequiresUserPrivacyConsent()
        {
            return false;
        }

        public void SendOutcome(string name)
        {
        }

        public void SendOutcome(string name, SendOutcomeEventSuccess sendOutcomeEventSuccess)
        {
        }

        public void SendOutcomeWithValue(string name, float value)
        {
        }

        public void SendOutcomeWithValue(string name, float value, SendOutcomeEventSuccess sendOutcomeEventSuccess)
        {
        }

        public void SendTag(string tagName, string tagValue)
        {
        }

        public void SendTags(IDictionary<string, string> tags)
        {
        }

        public void SendUniqueOutcome(string name)
        {
        }

        public void SendUniqueOutcome(string name, SendOutcomeEventSuccess sendOutcomeEventSuccess)
        {
        }

        public void SetEmail(string email, string emailAuthToken, OnSetEmailSuccess success, OnSetEmailFailure failure)
        {
        }

        public void SetEmail(string email, string emailAuthToken)
        {
        }

        public void SetEmail(string email, OnSetEmailSuccess success, OnSetEmailFailure failure)
        {
        }

        public void SetEmail(string email)
        {
        }

        public void SetExternalUserId(string externalId)
        {
        }

        public void SetLocationShared(bool shared)
        {
        }

        public void SetLogLevel(LOG_LEVEL logLevel, LOG_LEVEL visualLevel)
        {
        }

        public void SetRequiresUserPrivacyConsent(bool required)
        {
        }

        public void SetSubscription(bool enable)
        {
        }

        public XamarinBuilder StartInit(string appID)
        {
            return default(XamarinBuilder);
        }

        public void SyncHashedEmail(string email)
        {
        }

        public void UserDidProvidePrivacyConsent(bool granted)
        {
        }
    }
}
