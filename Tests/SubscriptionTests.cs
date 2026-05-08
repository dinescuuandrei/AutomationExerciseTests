using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class SubscriptionTests : BaseTest
    {
        [Test]
        public async Task VerifySubscription()
        {
            var selectie = Page.Locator("h2:has-text('Subscription')");
            var text = await selectie.InnerTextAsync();

            Assert.That(text.ToUpper().Contains("SUBSCRIPTION"), Is.True);

            await Page.FillAsync("#susbscribe_email", "andrei.test@gmail.com");
            await Page.ClickAsync("#subscribe");

            var mesaj = Page.Locator("#success-subscribe");
            await mesaj.WaitForAsync();

            var vizibil = await mesaj.IsVisibleAsync();
            Assert.That(vizibil, Is.True);
        }
    }
}