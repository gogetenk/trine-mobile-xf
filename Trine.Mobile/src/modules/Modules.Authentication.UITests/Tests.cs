using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Modules.Authentication.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
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
            app.Screenshot("Welcome screen.");

            app.EnterText(c => c.Marked("tb_email"), "toto@titi.fr");
            app.EnterText(c => c.Marked("tb_password"), "1234");
            app.Tap(c => c.Marked("Commencer"));

            Assert.IsTrue(results.Any());
        }
    }
}
