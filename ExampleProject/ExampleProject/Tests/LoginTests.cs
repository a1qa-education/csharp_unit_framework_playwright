using ExampleProject.Pages;
using FrameworkCore.Utilities;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace ExampleProject.Tests
{
    internal class LoginTests : BaseTest
    {
        private LoginPage _loginPage;

        [SetUp]
        public async Task Setup()
        {
            // Navigate to the specific test route
            await Page.GotoAsync($"{BaseUrl}login");

            // Instantiate the Page Object
            _loginPage = new LoginPage(Page);

            // Aquality Pattern: Always ensure the form is fully loaded before test execution begins
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