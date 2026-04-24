using Microsoft.Playwright;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class ContactTests : BaseTest
    {
        [Test]
        public async Task TC6_ContactUsForm()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = " Contact us" }).ClickAsync();
            await Page.Locator("[data-qa='name']").FillAsync("Andrei");
            await Page.Locator("[data-qa='email']").FillAsync("andrei.test@gmail.com");
            await Page.Locator("[data-qa='subject']").FillAsync("Problem");
            await Page.Locator("[data-qa='message']").FillAsync("Help me please");

            Page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await Page.Locator("[data-qa='submit-button']").ClickAsync();

            await Expect(Page.Locator(".status")).ToHaveTextAsync("Success! Your details have been submitted successfully.");
        }
    }
}
