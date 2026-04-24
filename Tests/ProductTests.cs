using Microsoft.Playwright;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class ProductTests : BaseTest
    {
        [Test]
        public async Task TC7_VerifyTestCasesPage()
        {
            var testCasesLink = Page.Locator("ul.navbar-nav a[href='/test_cases']");
            await testCasesLink.ClickAsync(new() { Force = true });
            await Expect(Page).ToHaveURLAsync("https://automationexercise.com/test_cases");
        }

        [Test]
        public async Task TC8_VerifyProductDetails()
        {
            await Page.Locator("a[href='/products']").ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Automation Exercise - All Products");
            await Page.Locator(".choose a").First.ClickAsync();
            await Expect(Page.Locator(".product-information h2")).ToBeVisibleAsync();
            await Expect(Page.Locator("p:has-text('Category:')")).ToBeVisibleAsync();
            await Expect(Page.Locator("p:has-text('Availability:')")).ToBeVisibleAsync();
            await Expect(Page.Locator("p:has-text('Brand:')")).ToBeVisibleAsync();
        }

        [Test]
        public async Task TC9_SearchProduct()
        {
            await Page.Locator("a[href='/products']").ClickAsync();
            await Page.Locator("#search_product").FillAsync("Polo");
            await Page.Locator("#submit_search").ClickAsync();
            await Expect(Page.Locator(".title").Filter(new() { HasText = "Searched Products" })).ToBeVisibleAsync();
            await Expect(Page.Locator(".single-products").First).ToBeVisibleAsync();
        }
    }
}