using Microsoft.Playwright;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class BonusTests : BaseTest
    {
        [Test]
        public async Task TC_BONUS_PriceAndPersistence()
        {
            await Page.Locator(".choose a").First.ClickAsync();
            await Page.Locator("#quantity").FillAsync("4");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add to cart" }).ClickAsync();
            await Page.Locator("u").Filter(new() { HasText = "View Cart" }).ClickAsync();

            var priceText = await Page.Locator(".cart_price p").InnerTextAsync();
            var totalText = await Page.Locator(".cart_total_price").InnerTextAsync();

            double unitPrice = double.Parse(priceText.Replace("Rs. ", ""));
            double totalPrice = double.Parse(totalText.Replace("Rs. ", ""));

            Assert.That(totalPrice, Is.EqualTo(unitPrice * 4));

            await Page.ReloadAsync();
            var totalAfterRefresh = await Page.Locator(".cart_total_price").InnerTextAsync();
            Assert.That(totalAfterRefresh, Is.EqualTo(totalText));
        }

        [Test]
        public async Task TC_BONUS_CrossTabCartSynchronization()
        {
            await Page.Locator(".choose a").First.ClickAsync();
            await Page.Locator("#quantity").FillAsync("2");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add to cart" }).ClickAsync();
            await Page.Locator("text=Continue Shopping").ClickAsync();

            var secondTab = await Page.Context.NewPageAsync();
            await secondTab.GotoAsync("http://automationexercise.com/view_cart");

            var cartRowCount = await secondTab.Locator("tbody tr").CountAsync();
            var quantityInSecondTab = await secondTab.Locator(".cart_quantity button").InnerTextAsync();

            Assert.That(cartRowCount, Is.EqualTo(1));
            Assert.That(quantityInSecondTab, Is.EqualTo("2"));
            await secondTab.CloseAsync();
        }
    }
}