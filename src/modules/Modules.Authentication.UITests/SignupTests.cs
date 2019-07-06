using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Modules.Authentication.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class SignupTests
    {
        private IApp app;
        private readonly Platform platform;

        public SignupTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
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

            app.EnterText(c => c.Marked("tb_email"), "toto@titi.fr");
            app.EnterText(c => c.Marked("tb_password"), "1234");
            app.Tap(c => c.Marked("Commencer"));

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
    }
}
