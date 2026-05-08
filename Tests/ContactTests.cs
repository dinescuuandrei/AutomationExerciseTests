using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class ContactTests : BaseTest
    {
        [Test]
        public async Task ContactForm()
        {
            await Page.ClickAsync("text=Contact us");

            await Page.FillAsync("[data-qa='name']", "Andrei");
            await Page.FillAsync("[data-qa='email']", "andrei.test@gmail.com");
            await Page.FillAsync("[data-qa='subject']", "Card Problem");
            await Page.FillAsync("[data-qa='message']", "Help me ! i cant pay with my card");

            Page.Dialog += async (_, dialog) =>
            {
                await dialog.AcceptAsync();
            };

            await Page.ClickAsync("[data-qa='submit-button']");

            var status = Page.Locator(".status");
            await status.WaitForAsync();

            string ok = await status.InnerTextAsync();

            Assert.That(ok.Contains("Success!"), Is.True);
        }
    }

}