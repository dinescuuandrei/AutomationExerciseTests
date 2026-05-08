using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests
{
    [TestFixture]
    public class AccountTests : BaseTest
    {
        [Test]
        public async Task RegisterUser()
        {
            string titlu_pagina = await Page.TitleAsync();
            Assert.That(titlu_pagina, Is.EqualTo("Automation Exercise"));

            await Page.ClickAsync("text=Signup / Login");

            var nume = Page.Locator("[data-qa='signup-name']");
            var email = Page.Locator("[data-qa='signup-email']");

            await nume.FillAsync("Andrei Dinescu");

            string adresa_mail = "andrei.test" + new Random().Next(1000, 9999) + "@gmail.com";
            await email.FillAsync(adresa_mail);

            await Page.ClickAsync("[data-qa='signup-button']");

            await Page.CheckAsync("#id_gender1");

            await Page.FillAsync("#password", "ParolaSigura123!");
            await Page.SelectOptionAsync("#days", "1");
            await Page.SelectOptionAsync("#months", "March");
            await Page.SelectOptionAsync("#years", "2005");

            await Page.CheckAsync("#newsletter");
            await Page.CheckAsync("#optin");
            await Page.FillAsync("#first_name", "Andrei");
            await Page.FillAsync("#last_name", "Dinescu");
            await Page.FillAsync("#address1", "Calea Bucuresti 107C");

            await Page.SelectOptionAsync("#country", "United States");

            await Page.FillAsync("#state", "Dolj");
            await Page.FillAsync("#city", "Craiova");
            await Page.FillAsync("#zipcode", "200478");
            await Page.FillAsync("#mobile_number", "0788153183");

            await Page.ClickAsync("[data-qa='create-account']");

            var este_creat = await Page.Locator("text=ACCOUNT CREATED!").IsVisibleAsync();
            Assert.That(este_creat, Is.True);

            await Page.ClickAsync("[data-qa='continue-button']");

            await Page.ClickAsync("text=Delete Account");

            var este_sters = await Page.Locator("text=ACCOUNT DELETED!").IsVisibleAsync();
            Assert.That(este_sters, Is.True);
        }
    


[Test]
        public async Task LoginCorrect()
        {
            await Page.ClickAsync("text=Signup / Login");

            await Page.FillAsync("[data-qa='login-email']", "andrei.test1@gmail.com");
            await Page.FillAsync("[data-qa='login-password']", "ParolaSigura123!");
            await Page.ClickAsync("[data-qa='login-button']");

            var ok = await Page.Locator("text=Logged in as Andrei Dinescu").IsVisibleAsync();
            Assert.That(ok, Is.True);
        }

        [Test]
        public async Task Logout()
        {
            await Page.ClickAsync("text=Signup / Login");

            await Page.FillAsync("[data-qa='login-email']", "GeorgescuPopescu@gmail.com");
            await Page.FillAsync("[data-qa='login-password']", "ParolaSigura123!");
            await Page.ClickAsync("[data-qa='login-button']");

            await Page.ClickAsync("text=Logout");

            var ok = await Page.Locator("text=Login to your account").IsVisibleAsync();
            Assert.That(ok, Is.True);
        }

        [Test]
        public async Task RegisterExistingEmail()
        {
            await Page.ClickAsync("text=Signup / Login");

            await Page.FillAsync("[data-qa='signup-name']", "Popescu");
            await Page.FillAsync("[data-qa='signup-email']", "GeorgescuPopescu@gmail.com");

            await Page.ClickAsync("[data-qa='signup-button']");

            var ok = await Page.Locator("text=Email Address already exist!").IsVisibleAsync();
            Assert.That(ok, Is.True);
        }
    }
}