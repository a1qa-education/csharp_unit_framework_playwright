using Microsoft.Playwright;
using FrameworkCore.Forms;
using FrameworkCore.Elements;
using FrameworkCore.Elements.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleProject.Pages
{
    public class MainPage : BaseForm
    {
        public MainPage(IPage page)
            : base(page, page.Locator("h1.heading"), "Main Internet Page")
        {
        }

        private IButton FormAuthenticationLink => ElementFactory.GetButton(
            Page.GetByRole(AriaRole.Link, new() { Name = "Form Authentication" }),
            "Form Auth Link");

        private IButton JavaScriptAlertsLink => ElementFactory.GetButton(
            Page.GetByRole(AriaRole.Link, new() { Name = "JavaScript Alerts" }),
            "Alerts Link");

        // ElementsList demo: all navigation links on the main page
        private IElementsList<Button> AvailableLinks => ElementFactory.GetList<Button>(
            Page.Locator("#content ul li a"),
            "Available Links");

        public async Task GoToFormAuthenticationAsync()
        {
            await FormAuthenticationLink.ClickAsync();
        }

        public async Task<int> GetLinksCountAsync()
        {
            return await AvailableLinks.CountAsync();
        }

        public async Task<IReadOnlyList<string>> GetAllLinkTextsAsync()
        {
            return await AvailableLinks.GetTextsAsync();
        }

        public async Task ClickLinkByIndexAsync(int index)
        {
            await AvailableLinks.GetElementByIndex(index).ClickAsync();
        }
    }
}