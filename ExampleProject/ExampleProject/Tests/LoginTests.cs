using ExampleProject.Pages;
using FrameworkCore.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace ExampleProject.Tests
{
    [TestFixture]
    internal class LoginTests : PageTest
    {
        private LoginPage _loginPage;

        [SetUp]
        public async Task Setup()
        {
            // 1. Pull environment settings dynamically
            string baseUrl = ConfigReader.GetFrameworkSetting<string>("BaseUrl");

            // 2. Navigate to the specific test route
            await Page.GotoAsync($"{baseUrl}login");

            // 3. Instantiate the Page Object
            _loginPage = new LoginPage(Page);

            // 4. Aquality Pattern: Always ensure the form is fully loaded before test execution begins
            await _loginPage.WaitForDisplayedAsync();
        }

        [Test]
        public async Task UserCanLoginWithValidCredentials()
        {
            // 1. Pull test data dynamically from testData.json
            var validUser = ConfigReader.GetTestDataSection("ValidCredentials");
            string username = validUser.GetValue<string>("Username");
            string password = validUser.GetValue<string>("Password");

            // 2. Execute business logic
            await _loginPage.LoginAsAsync(username, password);

            // 3. Retrieve state for assertion
            string actualMessage = await _loginPage.GetAlertMessageAsync();

            // 4. Assert outcome
            Assert.That(actualMessage, Does.Contain("You logged into a secure area!"),
                "The success message was not displayed after a valid login attempt.");
        }

        [Test]
        public async Task UserCannotLoginWithInvalidCredentials()
        {
            // 1. Pull negative test data
            var invalidUser = ConfigReader.GetTestDataSection("InvalidCredentials");
            string username = invalidUser.GetValue<string>("Username");
            string password = invalidUser.GetValue<string>("Password");

            // 2. Execute business logic
            await _loginPage.LoginAsAsync(username, password);

            // 3. Retrieve state for assertion
            string actualMessage = await _loginPage.GetAlertMessageAsync();

            // 4. Assert outcome
            Assert.That(actualMessage, Does.Contain("Your username is invalid!"),
                "The expected error message was not displayed for invalid credentials.");
        }
    }
}