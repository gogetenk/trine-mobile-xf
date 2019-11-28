using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Pages
{
    public class LoginPage
    {
        private readonly IApp _app;

        readonly Func<AppQuery, AppQuery> _mailEntry = e => e.Marked("tb_email");
        readonly Func<AppQuery, AppQuery> _passwordEntry = e => e.Marked("tb_password");
        readonly Func<AppQuery, AppQuery> _startButton = e => e.Marked("bt_start");

        public LoginPage(IApp app)
        {
            _app = app;
        }

        public void EnterEmail(string email)
        {
            _app.ClearText(_mailEntry);
            _app.EnterText(_mailEntry, email);
        }

        public void EnterPassword(string password)
        {
            _app.ClearText(_passwordEntry);
            _app.EnterText(_passwordEntry, password);
        }

        public void TapLoginButton()
        {
            _app.Tap(_startButton);
        }
    }
}
