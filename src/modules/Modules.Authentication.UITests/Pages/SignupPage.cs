using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Modules.Authentication.UITests.Pages
{
    public class SignupPage
    {
        private readonly Func<AppQuery, AppQuery> _emailEntry = e => e.Marked("tb_email");
        private readonly Func<AppQuery, AppQuery> _passwordEntry = e => e.Marked("tb_password");
        private readonly Func<AppQuery, AppQuery> _startButton = e => e.Marked("bt_start");
        private readonly IApp _app;

        public SignupPage(IApp app)
        {
            _app = app;
        }

        public void TapStartButton()
        {
            _app.Tap(_startButton);
        }
    }
}
