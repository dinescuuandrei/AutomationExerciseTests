using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AutomationExerciseTests.Tests
{
    public class BaseTest : PageTest
    {
        [SetUp]
        public async Task Setup()
        {
            await Context.RouteAsync("**/*", async route =>
            {
                string url = route.Request.Url;
                if (url.Contains("googlesyndication") || url.Contains("adservice") || url.Contains("doubleclick"))
                {
                    await route.AbortAsync();
                }
                else
                {
                    await route.ContinueAsync();
                }
            });

            await Page.GotoAsync("https://automationexercise.com/");

            var consentButton = Page.GetByRole(AriaRole.Button, new() { Name = "Consent", Exact = true });
            if (await consentButton.IsVisibleAsync())
            {
                await consentButton.ClickAsync();
            }
        }
    }
}