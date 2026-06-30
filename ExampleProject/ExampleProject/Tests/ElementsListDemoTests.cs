using ExampleProject.Pages;
using NUnit.Framework;

namespace ExampleProject.Tests
{
    internal class ElementsListDemoTests : BaseTest
    {
        private MainPage _mainPage;

        [SetUp]
        public async Task Setup()
        {
            await Page.GotoAsync(BaseUrl);

            _mainPage = new MainPage(Page);
            await _mainPage.WaitForDisplayedAsync();
        }

        [Test]
        public async Task MainPageContainsMultipleLinks()
        {
            // 1. Get count of links via ElementsList
            int linkCount = await _mainPage.GetLinksCountAsync();

            // 2. Assert that the page has a reasonable number of links
            Assert.That(linkCount, Is.GreaterThan(10),
                "The main page should contain more than 10 navigation links.");

            TestContext.WriteLine($"Found {linkCount} links on the main page.");
        }

        [Test]
        public async Task CanRetrieveAllLinkTexts()
        {
            // 1. Get all link texts via ElementsList.GetTextsAsync()
            var linkTexts = await _mainPage.GetAllLinkTextsAsync();

            // 2. Assert the list is not empty and contains known links
            Assert.That(linkTexts, Is.Not.Empty,
                "Link texts collection should not be empty.");
            Assert.That(linkTexts, Does.Contain("Form Authentication"),
                "The link list should contain 'Form Authentication'.");

            TestContext.WriteLine($"Link texts: {string.Join(", ", linkTexts)}");
        }

        [Test]
        public async Task CanClickLinkByIndex()
        {
            // 1. Click the first link via ElementsList.GetElementByIndex()
            await _mainPage.ClickLinkByIndexAsync(0);

            // 2. Assert that navigation occurred (URL changed from the main page)
            Assert.That(Page.Url, Is.Not.EqualTo(BaseUrl),
                "Clicking a link should navigate away from the main page.");

            TestContext.WriteLine($"Navigated to: {Page.Url}");
        }
    }
}
