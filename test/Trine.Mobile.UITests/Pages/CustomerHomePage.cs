using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Pages
{
    public class CustomerHomePage
    {
        private readonly IApp _app;

        public Func<AppQuery, AppQuery> acceptPopup = e => e.Marked("popup_signature");
        public Func<AppQuery, AppQuery> refusalPopup = e => e.Marked("popup_refusal");
        public Func<AppQuery, AppQuery> cancelActivityButton = e => e.Marked("bt_cancel");
        public Func<AppQuery, AppQuery> acceptActivityButton = e => e.Marked("bt_accept");
        public Func<AppQuery, AppQuery> activityList = e => e.Marked("sl_activities");
        public Func<AppQuery, AppQuery> activityCard(int index) => e => e.Marked("sl_activities").Index(index);
        public Func<AppQuery, AppQuery> refuseButton(int index) => e => e.Marked("sl_activities").Index(index).Button("bt_refuse");
        public Func<AppQuery, AppQuery> acceptButton(int index) => e => e.Marked("sl_activities").Index(index).Button("bt_accept");
        readonly Func<AppQuery, AppQuery> _signButton = e => e.Marked("bt_sign");

        public CustomerHomePage(IApp app)
        {
            _app = app;
        }

        public void TapActivityCard(int index)
        {
            _app.Tap(activityCard(index));
        }

        public void TapRefuseButton(int index)
        {
            _app.Tap(refuseButton(index));
        }

        public void TapAcceptButton(int index)
        {
            _app.Tap(acceptButton(index));
        }

        public void TapAcceptActivityButton()
        {
            _app.Tap(acceptActivityButton);
        }

        public void TapCancelActivityButton()
        {
            _app.Tap(cancelActivityButton);
        }

        
    }
}
