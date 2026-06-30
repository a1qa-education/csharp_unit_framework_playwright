using ExampleProject.Pages;
using NUnit.Framework;

namespace ExampleProject.Tests
{
    internal class DropdownAndIFrameTests : BaseTest
    {
        [Test]
        public async Task CanInteractWithDropdown()
        {
            await Page.GotoAsync($"{BaseUrl}dropdown");
            var dropdownPage = new DropdownPage(Page);
            await dropdownPage.WaitForDisplayedAsync();

            // 1. Select by Index
            await dropdownPage.SelectOptionByIndexAsync(1);
            string selectedText = await dropdownPage.GetSelectedOptionTextAsync();
            Assert.That(selectedText, Is.EqualTo("Option 1"), "Failed to select 'Option 1' by index.");

            // 2. Select by Value
            await dropdownPage.SelectOptionByValueAsync("2");
            selectedText = await dropdownPage.GetSelectedOptionTextAsync();
            Assert.That(selectedText, Is.EqualTo("Option 2"), "Failed to select 'Option 2' by value.");

            // 3. Select by Text
            await dropdownPage.SelectOptionByTextAsync("Option 1");
            selectedText = await dropdownPage.GetSelectedOptionTextAsync();
            Assert.That(selectedText, Is.EqualTo("Option 1"), "Failed to select 'Option 1' by text.");
        }

        [Test]
        public async Task CanInteractWithIFrame()
        {
            await Page.GotoAsync($"{BaseUrl}iframe");
            var iframePage = new IFramePage(Page);
            await iframePage.WaitForDisplayedAsync();

            // Verify text inside the IFrame
            // TinyMCE starts with default text. We just read it to prove we can access elements inside the IFrame.
            string content = await iframePage.GetEditorTextAsync();
            Assert.That(content, Does.Contain("Your content goes here."), "Failed to read the default text from the IFrame editor.");
        }
    }
}
