using System.Linq;
using NUnit.Framework;
using Trine.Mobile.UITests.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Authentication
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class SignupTests
    {
        private IApp app;
        private readonly Platform platform;
        private SignupPage _signupPage;

        public SignupTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            _signupPage = new SignupPage(app);
        }


        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Bienvenue"));
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void SignUp_NominalCase()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Commencer"));

            _signupPage.EnterEmail("toto@titi.fr");
            _signupPage.EnterPassword("1234");
            _signupPage.TapStartButton();

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void SignUp_WhenEmailIsEmpty_ExpectErrorMessage()
        {
            app.EnterText(c => c.Marked("tb_password"), "1234");
            app.Tap(c => c.Marked("Commencer"));

            AppResult[] results = app.WaitForElement(c => c.Marked("Veuillez spécifier un e-mail valide"));

            Assert.IsTrue(results.Count() == 1);
        }

        [Test]
        public void SignUp_WhenPasswordIsEmpty_ExpectErrorMessage()
        {
            app.EnterText(c => c.Marked("tb_email"), "toto@titi.fr");
            app.Tap(c => c.Marked("Commencer"));

            AppResult[] results = app.WaitForElement(c => c.Marked("Veuillez spécifier un mot de passe valide"));

            Assert.IsTrue(results.Count() == 1);
        }

        [Test]
        public void SignUp_WhenBothEntriesAreEmpty_ExpectBothErrorMessages()
        {
            app.Tap(c => c.Marked("Commencer"));

            AppResult[] results = app.WaitForElement(c => c.Marked("Veuillez spécifier un mot de passe valide"));
            Assert.IsTrue(results.Count() == 1);

            results = app.WaitForElement(c => c.Marked("Veuillez spécifier un e-mail valide"));
            Assert.IsTrue(results.Count() == 1);
        }

        [Test]
        public void AlreadyAnAccount_NominalCase_ExpectNavigated()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Bienvenue"));

            _signupPage.TapLoginButton();

            results = app.WaitForElement(c => c.Marked("Ravi de vous revoir !"));
            Assert.IsTrue(results.Any());
        }
    }
}
