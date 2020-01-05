using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Pages
{
    public class SignupPage
    {
        readonly Func<AppQuery, AppQuery> _mailEntry = e => e.Marked("tb_email");
        readonly Func<AppQuery, AppQuery> _passwordEntry = e => e.Marked("tb_password");
        readonly Func<AppQuery, AppQuery> _startButton = e => e.Marked("bt_start");
        readonly Func<AppQuery, AppQuery> _alreadyAnAccountLabel = e => e.Marked("lb_alreadyAccount");
        private readonly IApp _app;

        public SignupPage(IApp app)
        {
            _app = app;
        }

        public void EnterEmail(string email)
        {
            _app.EnterText(_mailEntry, email);
        }

        public void EnterPassword(string password)
        {
            _app.EnterText(_passwordEntry, password);
        }

        public void TapStartButton()
        {
            _app.Tap(_startButton);
        }

        public void TapLoginButton()
        {
            _app.Tap(_alreadyAnAccountLabel);
        }

       
    }
}
