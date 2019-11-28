using NUnit.Framework;
using System.Linq;
using Trine.Mobile.UITests.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Consultant
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class HomeViewTests
    {
        private const string _DummyUserEmail = "testconsultant@sogetrel.fr";
        private const string _DummyUserPassword = "123";
        //private const string _DummyUserEmail = "ytocreau@trine.com";
        //private const string _DummyUserPassword = "123";

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
            _loginPage.EnterPassword(_DummyUserPassword);
            _loginPage.TapLoginButton();
        }

        [Test]
        public void HomePageIsDisplayed()
        {
            AppResult[] results = _app.WaitForElement(c => c.Marked("grid_header"));
            Assert.IsTrue(results.Any());
        }
    }
}
