using Microsoft.Playwright;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class AccountTests : BaseTest
    {
        [Test]
        public async Task TC1_RegisterUser()
        {
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();

            var signup = Page.Locator("form").Filter(new() { HasText = "Signup" });
            await signup.GetByPlaceholder("Name").FillAsync("Andrei Dinescu");
            await signup.GetByPlaceholder("Email Address").FillAsync("andrei.test" + new Random().Next(1000) + "@gmail.com");
            await signup.GetByRole(AriaRole.Button, new() { Name = "Signup" }).ClickAsync();

            await Page.Locator("#id_gender1").CheckAsync();
            await Page.Locator("#password").FillAsync("ParolaSigura123!");
            await Page.Locator("#first_name").FillAsync("Andrei");
            await Page.Locator("#last_name").FillAsync("Dinescu");
            await Page.Locator("#address1").FillAsync("Strada Programarii, Nr. 1");
            await Page.Locator("#country").SelectOptionAsync("United States");
            await Page.Locator("#state").FillAsync("California");
            await Page.Locator("#city").FillAsync("Los Angeles");
            await Page.Locator("#zipcode").FillAsync("90001");
            await Page.Locator("#mobile_number").FillAsync("0712345678");

            await Page.GetByRole(AriaRole.Button, new() { Name = "Create Account" }).ClickAsync();
            await Expect(Page.GetByText("ACCOUNT CREATED!")).ToBeVisibleAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Continue" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = " Delete Account" }).ClickAsync();
            await Expect(Page.GetByText("ACCOUNT DELETED!")).ToBeVisibleAsync();
        }

        [Test]
        public async Task TC2_LoginCorrect()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            var login = Page.Locator("form").Filter(new() { HasText = "Login" });
            await login.Locator("[data-qa='login-email']").FillAsync("andrei.test1@gmail.com");
            await login.Locator("[data-qa='login-password']").FillAsync("ParolaSigura123!");
            await login.Locator("[data-qa='login-button']").ClickAsync();
            await Expect(Page.GetByText("Logged in as Andrei Dinescu")).ToBeVisibleAsync();
        }

        [Test]
        public async Task TC4_Logout()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            var login = Page.Locator("form").Filter(new() { HasText = "Login" });
            await login.Locator("[data-qa='login-email']").FillAsync("GeorgescuPopescu@gmail.com");
            await login.Locator("[data-qa='login-password']").FillAsync("ParolaSigura123!");
            await login.Locator("[data-qa='login-button']").ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = " Logout" }).ClickAsync();
            await Expect(Page.GetByText("Login to your account")).ToBeVisibleAsync();
        }

        [Test]
        public async Task TC5_RegisterExistingEmail()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            var signup = Page.Locator("form").Filter(new() { HasText = "Signup" });
            await signup.GetByPlaceholder("Name").FillAsync("Popescu");
            await signup.Locator("[data-qa='signup-email']").FillAsync("GeorgescuPopescu@gmail.com");
            await signup.Locator("[data-qa='signup-button']").ClickAsync();
            await Expect(Page.GetByText("Email Address already exist!")).ToBeVisibleAsync();
        }
    }
}