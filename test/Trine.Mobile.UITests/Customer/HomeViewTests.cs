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
        public void CustomerNavigatesToHomeAndManipulatesActivity()
        {
            //AppResult[] results = _app.WaitForElement(c => c.Marked("grid_header"));
            //Assert.IsTrue(results.Any());
            //_app.Screenshot("Home page is displayed.");

            AppResult[] activityList = _app.WaitForElement(_homePage.activityList, timeout: new System.TimeSpan(0, 1, 0));
            Assert.IsTrue(activityList.Any());
            _app.Screenshot("Activities are displayed.");

            _homePage.TapActivityCard(0);
            _app.Tap(x => x.Marked("card_activity"));
            AppResult[] calendar = _app.WaitForElement(c => c.Marked("calendar"));
            Assert.IsTrue(calendar.Any());
            _app.Screenshot("Tapped the first card and got the activity calendar displayed.");

            _homePage.TapRefuseButton(0);
            AppResult[] refusalPopup = _app.WaitForElement(_homePage.refusalPopup);
            Assert.IsTrue(refusalPopup.Any());
            _app.Screenshot("Tapped refuse button and got the refusal popup displayed.");

            _homePage.TapCancelActivityButton();
            _app.Screenshot("Canceled refuse popup successfully.");

            _homePage.TapAcceptButton(0);
            AppResult[] signupPopup = _app.WaitForElement(_homePage.acceptPopup);
            Assert.IsTrue(signupPopup.Any());
            _app.Screenshot("Tapped accept button and got the signature popup displayed.");

            _homePage.TapCancelActivityButton();
            _app.Screenshot("Canceled accept popup successfully.");
        }
    }
}
