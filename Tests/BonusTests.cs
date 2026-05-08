using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class BonusTests : BaseTest
    {
        [Test]
        public async Task CheckCartTotal()
        {
            await Page.Locator(".choose a").First.ClickAsync();

            var qty = Page.Locator("#quantity");
            await qty.FillAsync("4");

            await Page.ClickAsync("button:has-text('Add to cart')");
            await Page.ClickAsync("u");

            await Page.WaitForTimeoutAsync(1500);

            string pret1 = await Page.InnerTextAsync(".cart_price p");
            string pret2 = await Page.InnerTextAsync(".cart_total_price");

            pret1 = pret1.Replace("Rs. ", "");
            pret2 = pret2.Replace("Rs. ", "");

            int pretMic = Int32.Parse(pret1);
            int pretMare = Int32.Parse(pret2);

            int calcul = pretMic * 4;

            Assert.That(pretMare, Is.EqualTo(calcul));

            await Page.ReloadAsync();

            string pretNou = await Page.InnerTextAsync(".cart_total_price");

            pretNou = pretNou.Replace("Rs. ", "");

            int dupaRefresh = Convert.ToInt32(pretNou);

            Assert.That(dupaRefresh, Is.EqualTo(pretMare));
        }

        [Test]
        public async Task CartVisibleInNewTab()
        {
            await Page.Locator(".choose a").First.ClickAsync();

            await Page.FillAsync("#quantity", "2");

            await Page.ClickAsync("button:has-text('Add to cart')");

            await Page.ClickAsync("text=Continue Shopping");

            var tabNou = await Context.NewPageAsync();

            await tabNou.GotoAsync("https://automationexercise.com/view_cart");

            int cateRanduri = await tabNou.Locator("tbody tr").CountAsync();

            string qtyText = await tabNou.Locator(".cart_quantity button").InnerTextAsync();

            Assert.That(cateRanduri, Is.EqualTo(1));

            Assert.That(qtyText, Is.EqualTo("2"));

            await tabNou.CloseAsync();
        }
    }

}