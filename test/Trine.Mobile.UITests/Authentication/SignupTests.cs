using NUnit.Framework;
using System.Linq;
using Trine.Mobile.UITests.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Trine.Mobile.UITests.Authentication
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class SignupTests
    {
        private IApp _app;
        private readonly Platform platform;
        private SignupPage _signupPage;

        public SignupTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _app = AppInitializer.StartApp(platform);
            _signupPage = new SignupPage(_app);
        }


        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = _app.WaitForElement(c => c.Marked("Bienvenue"));
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void SignUp_NominalCase()
        {
            _signupPage.EnterEmail("toto@titi.fr");
            _app.DismissKeyboard();
            _signupPage.EnterPassword("1234");
            _app.DismissKeyboard();
            _signupPage.TapStartButton();

            AppResult[] results = _app.WaitForElement(c => c.Marked("Nous voudrions mieux vous connaître."));
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void SignUp_WhenEmailIsEmpty_ExpectErrorMessage()
        {
            _app.EnterText(c => c.Marked("tb_password"), "1234");
            _app.DismissKeyboard();
            _app.Tap(c => c.Marked("Commencer"));

            AppResult[] results = _app.WaitForElement(c => c.Marked("Veuillez spécifier un e-mail valide"));

            Assert.IsTrue(results.Count() == 1);
        }

        [Test]
        public void SignUp_WhenPasswordIsEmpty_ExpectErrorMessage()
        {
            _app.EnterText(c => c.Marked("tb_email"), "toto@titi.fr");
            _app.DismissKeyboard();
            _app.Tap(c => c.Marked("Commencer"));

            AppResult[] results = _app.WaitForElement(c => c.Marked("Veuillez spécifier un mot de passe valide"));

            Assert.IsTrue(results.Count() == 1);
        }

        [Test]
        public void SignUp_WhenBothEntriesAreEmpty_ExpectBothErrorMessages()
        {
            _app.Tap(c => c.Marked("Commencer"));

            AppResult[] results = _app.WaitForElement(c => c.Marked("Veuillez spécifier un mot de passe valide"));
            Assert.IsTrue(results.Count() == 1);

            results = _app.WaitForElement(c => c.Marked("Veuillez spécifier un e-mail valide"));
            Assert.IsTrue(results.Count() == 1);
        }

        [Test]
        public void AlreadyAnAccount_NominalCase_ExpectNavigated()
        {
            AppResult[] results = _app.WaitForElement(c => c.Marked("Bienvenue"));

            _signupPage.TapLoginButton();

            results = _app.WaitForElement(c => c.Marked("Ravi de vous revoir !"));
            Assert.IsTrue(results.Any());
        }
    }
}
