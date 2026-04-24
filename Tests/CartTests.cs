using Microsoft.Playwright;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class CartTests : BaseTest
    {
        [Test]
        public async Task TC12_AddProductsInCart()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = " Products" }).ClickAsync();

            var firstProduct = Page.Locator(".features_items .col-sm-4").First;
            await firstProduct.HoverAsync();

            await firstProduct.Locator(".overlay-content .add-to-cart").ClickAsync();

            var viewCartModalLink = Page.Locator("#cartModal u");
            await viewCartModalLink.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await viewCartModalLink.ClickAsync();

            var cartItems = Page.Locator("#cart_info_table tbody tr");
            await Expect(cartItems).ToHaveCountAsync(1);
        }
    }
}