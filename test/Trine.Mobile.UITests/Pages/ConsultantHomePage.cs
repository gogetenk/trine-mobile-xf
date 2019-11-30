using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Pages
{
    public class ConsultantHomePage
    {
        private readonly IApp _app;

        public Func<AppQuery, AppQuery> cancelSignatureButton = e => e.Marked("bt_cancel_signature");
        readonly Func<AppQuery, AppQuery> _confirmSignButton = e => e.Marked("bt_confirm_sign");
        readonly Func<AppQuery, AppQuery> _signButton = e => e.Marked("bt_sign");
        readonly Func<AppQuery, AppQuery> _saveButton = e => e.Marked("bt_save");
        
        public ConsultantHomePage(IApp app)
        {
            _app = app;
        }

        public void TapSaveButton()
        {
            _app.Tap(_saveButton);
        }

        public void TapSignButton()
        {
            _app.Tap(_signButton);
        }

        public void TapConfirmSignButton()
        {
            _app.Tap(_confirmSignButton);
        }

        public void TapCancelSignatureButton()
        {
            _app.Tap(cancelSignatureButton);
        }
    }
}
