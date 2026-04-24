using Microsoft.Playwright;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class SubscriptionTests : BaseTest
    {
        [Test]
        public async Task TC10_VerifySubscription()
        {
            await Expect(Page.Locator("h2").Filter(new() { HasText = "Subscription" })).ToBeVisibleAsync();
            await Page.GetByPlaceholder("Your email address").FillAsync("andrei.test@gmail.com");
            await Page.Locator("#subscribe").ClickAsync();
            await Expect(Page.Locator("#success-subscribe")).ToBeVisibleAsync();
        }
    }
}