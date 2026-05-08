using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class ProductTests : BaseTest
    {
        [Test]
        public async Task VerifyTestCase()
        {
            var buton = Page.Locator("a[href='/test_cases']").First;
            await buton.ClickAsync();

            string url = Page.Url;

            Assert.That(url, Is.EqualTo("https://automationexercise.com/test_cases"));
        }

        [Test]
        public async Task VerifyProduct()
        {
            await Page.ClickAsync("text=Products");

            string titlu = await Page.TitleAsync();
            Assert.That(titlu, Is.EqualTo("Automation Exercise - All Products"));

            await Page.Locator(".choose a").First.ClickAsync();

            var nume = await Page.Locator(".product-information h2").IsVisibleAsync();
            Assert.That(nume, Is.True);

            var categorie = await Page.Locator("p:has-text('Category:')").IsVisibleAsync();
            var stoc = await Page.Locator("p:has-text('Availability:')").IsVisibleAsync();
            var brand = await Page.Locator("p:has-text('Brand:')").IsVisibleAsync();

            Assert.That(categorie, Is.True);
            Assert.That(stoc, Is.True);
            Assert.That(brand, Is.True);
        }

        [Test]
        public async Task SearchProduct()
        {
            await Page.ClickAsync("a[href='/products']");

            var bara_search = Page.Locator("#search_product");
            await bara_search.FillAsync("Polo");

            await Page.ClickAsync("#submit_search");

            var titlu = await Page.Locator(".title").InnerTextAsync();
            Assert.That(titlu.ToUpper().Contains("SEARCHED PRODUCTS"), Is.True);

            var primul_produs = await Page.Locator(".single-products").First.IsVisibleAsync();
            Assert.That(primul_produs, Is.True);
        }
    }
}