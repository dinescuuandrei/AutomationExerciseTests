using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class CartTests : BaseTest
    {
        [Test]
        public async Task AddProducts()
        {
            await Page.ClickAsync("text=Products");

            var produs1 = Page.Locator(".single-products").First;
            await produs1.HoverAsync();

            var buton = Page.Locator(".add-to-cart").First;
            await buton.ClickAsync();

            var cos = Page.Locator("u");
            await cos.WaitForAsync();
            await cos.ClickAsync();

            int randuri = await Page.Locator("tbody tr").CountAsync();

            Assert.That(randuri, Is.EqualTo(1));
        }
    }

}