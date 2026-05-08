using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    public class BaseTest : PageTest
    {
        [SetUp]
        public async Task Setup()
        {
            await Context.RouteAsync("**/*", async r =>
            {
                string adr = r.Request.Url;

                if (adr.Contains("google") || adr.Contains("doubleclick") || adr.Contains("adservice"))
                {
                    await r.AbortAsync();
                }
                else
                {
                    await r.ContinueAsync();
                }
            });

            await Page.GotoAsync("https://automationexercise.com/");

            try
            {
                var btn = Page.Locator("button:has-text('Consent')");
                await btn.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 2500 });
                await btn.ClickAsync();
            }
            catch
            {
            }
        }
    }
}