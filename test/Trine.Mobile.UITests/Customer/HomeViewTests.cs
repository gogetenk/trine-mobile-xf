using NUnit.Framework;
using System.Linq;
using Trine.Mobile.UITests.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Customer
{
    /// <summary>
    /// Customer side
    /// </summary>
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class HomeViewTests 
    {
        private const string _DummyUserEmail = "remiroycourt@trine.com"; // Overriden by CI
        private const string _DummyUserPassword = "123"; 

        private IApp _app;
        private readonly Platform platform;
        private SignupPage _signupPage;
        private CustomerHomePage _homePage;
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
            _homePage = new CustomerHomePage(_app);
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
        public void HomePageIsDisplayed()
        {
            AppResult[] results = _app.WaitForElement(c => c.Marked("grid_header"), timeout: new System.TimeSpan(0, 0, 40));
            Assert.IsTrue(results.Any());
        }
    }
}
