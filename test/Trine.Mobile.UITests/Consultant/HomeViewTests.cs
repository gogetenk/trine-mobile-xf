using NUnit.Framework;
using System.Linq;
using Trine.Mobile.UITests.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Consultant
{
    /// <summary>
    /// Consultant side
    /// </summary>
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class HomeViewTests
    {
        private const string _DummyUserEmail = "ytocreau@trine.com"; // Overriden by CI
        private const string _DummyUserPassword = "123"; 

        private IApp _app;
        private readonly Platform platform;
        private SignupPage _signupPage;
        private ConsultantHomePage _homePage;
        private LoginPage _loginPage;

        public HomeViewTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _app = AppInitializer.StartApp(platform);
            _signupPage = new SignupPage(_app);
            _homePage = new ConsultantHomePage(_app);
            _loginPage = new LoginPage(_app);

            NavigateToHomePage();
        }

        private void NavigateToHomePage()
        {
            _signupPage.TapLoginButton();
            _loginPage.EnterEmail(_DummyUserEmail);
            _app.DismissKeyboard();
            _loginPage.EnterPassword(_DummyUserPassword);
            _app.DismissKeyboard();
            _loginPage.TapLoginButton();
        }

        [Test]
        public void ConsultantNavigatesToHomeAndManipulatesActivity()
        {
            AppResult[] results = _app.WaitForElement(c => c.Marked("grid_header"));
            Assert.IsTrue(results.Any());
            _app.Screenshot("Home page is displayed.");

            AppResult[] calendarSpheres = _app.WaitForElement(c => c.Marked("calendar_sphere"), timeout: new System.TimeSpan(0, 1, 0));
            Assert.IsTrue(results.Any());
            _app.Screenshot("Calendar is displayed and buttons are ready to be used.");

            _homePage.TapSaveButton();
            _app.Screenshot("Saving the activity.");

            _homePage.TapSignButton();
            _app.Screenshot("Signing the activity.");

            AppResult[] cancelButtonResults = _app.WaitForElement(_homePage.cancelSignatureButton);
            Assert.IsTrue(results.Any());
            _app.Screenshot("Signing popup is displayed successfully.");

            _homePage.TapCancelSignatureButton();
            _app.Screenshot("Canceled signing popup successfully.");
        }

    }
}
