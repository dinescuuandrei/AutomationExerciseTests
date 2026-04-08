using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace AutomationExerciseTests
{
    [TestFixture]
    public class Tests : PageTest
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
        }
        [Test]
        public async Task TC1()
        {
            var consentButton = Page.GetByRole(AriaRole.Button, new() { Name = "Consent", Exact = true });
            if (await consentButton.IsVisibleAsync())
            {
                await consentButton.ClickAsync();
            }
            await Page.GotoAsync("https://automationexercise.com/");
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");

            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            await Expect(Page.GetByText("New User Signup!")).ToBeVisibleAsync();

            var signup = Page.Locator("form").Filter(new() { HasText = "Signup" });

            await signup.GetByPlaceholder("Name").FillAsync("Andrei Dinescu");
            await signup.GetByPlaceholder("Email Address").FillAsync("andrei.test@gmail.com");
            await signup.GetByRole(AriaRole.Button, new() { Name = "Signup" }).ClickAsync();

            await Expect(Page.GetByText("Enter Account Information")).ToBeVisibleAsync();

            await Page.Locator("#id_gender1").CheckAsync();
            await Page.Locator("#password").FillAsync("ParolaSigura123!");
            await Page.Locator("#days").SelectOptionAsync("15");
            await Page.Locator("#months").SelectOptionAsync("May");
            await Page.Locator("#years").SelectOptionAsync("1995");

            await Page.Locator("#newsletter").CheckAsync();
            await Page.Locator("#optin").CheckAsync();

            await Page.Locator("#first_name").FillAsync("Andrei");
            await Page.Locator("#last_name").FillAsync("Dinescu");
            await Page.Locator("#company").FillAsync("SoftVision");
            await Page.Locator("#address1").FillAsync("Strada Programarii, Nr. 1");
            await Page.Locator("#address2").FillAsync("Bloc 2, Ap 10");
            await Page.Locator("#country").SelectOptionAsync("United States");
            await Page.Locator("#state").FillAsync("California");
            await Page.Locator("#city").FillAsync("Los Angeles");
            await Page.Locator("#zipcode").FillAsync("90001");
            await Page.Locator("#mobile_number").FillAsync("0712345678");

            await Page.GetByRole(AriaRole.Button, new() { Name = "Create Account" }).ClickAsync();

            await Expect(Page.GetByText("ACCOUNT CREATED!")).ToBeVisibleAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Continue" }).ClickAsync();
            await Expect(Page.GetByText("Logged in as " + "Andrei Dinescu")).ToBeVisibleAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = " Delete Account" }).ClickAsync();
            await Expect(Page.GetByText("ACCOUNT DELETED!")).ToBeVisibleAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Continue" }).ClickAsync();
        }


        [Test]
        public async Task TC2()
        {
            await Page.GotoAsync("http://automationexercise.com");
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            await Expect(Page.GetByText("Login to your account")).ToBeVisibleAsync();

            var login = Page.Locator("form").Filter(new() { HasText = "Login" });

            await login.Locator("[data-qa='login-email']").FillAsync("andrei.test1@gmail.com");
            await login.Locator("[data-qa='login-password']").FillAsync("ParolaSigura123!");
            await login.Locator("[data-qa='login-button']").ClickAsync();

            await Expect(Page.GetByText("Logged in as Andrei Dinescu")).ToBeVisibleAsync();

            /* await Page.GetByRole(AriaRole.Link, new() { Name = " Delete Account" }).ClickAsync();

             await Expect(Page.GetByText("ACCOUNT DELETED!")).ToBeVisibleAsync();
            */
        }
        [Test]
        public async Task TC4()
        {

            await Page.GotoAsync("http://automationexercise.com");
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            await Expect(Page.GetByText("Login to your account")).ToBeVisibleAsync();

            var login = Page.Locator("form").Filter(new() { HasText = "Login" });

            await login.Locator("[data-qa='login-email']").FillAsync("GeorgescuPopescu@gmail.com");
            await login.Locator("[data-qa='login-password']").FillAsync("ParolaSigura123!");
            await login.Locator("[data-qa='login-button']").ClickAsync();

            await Expect(Page.GetByText("Logged in as Popescu")).ToBeVisibleAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = " Logout" }).ClickAsync();
            await Expect(Page.GetByText("Login to your account")).ToBeVisibleAsync();
        }
        [Test]
        public async Task TC5()
        {
            await Page.GotoAsync("http://automationexercise.com");
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");
            await Page.GetByRole(AriaRole.Link, new() { Name = " Signup / Login" }).ClickAsync();
            await Expect(Page.GetByText("Login to your account")).ToBeVisibleAsync();
            var signup = Page.Locator("form").Filter(new() { HasText = "Signup" });
            await signup.GetByPlaceholder("Name").FillAsync("Popescu");
            await signup.Locator("[data-qa='signup-email']").FillAsync("GeorgescuPopescu@gmail.com");
            await signup.Locator("[data-qa='signup-button']").ClickAsync();
            await Expect(Page.GetByText("Email Address already exist!")).ToBeVisibleAsync();
        }

        [Test]
        public async Task TC6()
        {
            await Page.GotoAsync("http://automationexercise.com");
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");
            await Page.GetByRole(AriaRole.Link, new() { Name = " Contact us" }).ClickAsync();
            await Expect(Page.GetByText("GET IN TOUCH")).ToBeVisibleAsync();
            await Page.Locator("[data-qa='name']").FillAsync("Andrei");
            await Page.Locator("[data-qa='email']").FillAsync("andrei.test@gmail.com");
            await Page.Locator("[data-qa='subject']").FillAsync("Card Problem");
            await Page.Locator("[data-qa='message']").FillAsync("I have an issue when I try to order something and want to pay with my card!");
            Page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await Page.Locator("[data-qa='submit-button']").ClickAsync();

            await Expect(Page.Locator(".status")).ToHaveTextAsync("Success! Your details have been submitted successfully.");

            await Page.Locator("#form-section").GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Automation Exercise");
        }
    }
}
