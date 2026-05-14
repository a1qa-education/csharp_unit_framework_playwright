using Microsoft.Playwright;
using FrameworkCore.Forms;
using FrameworkCore.Elements.Interfaces;
using System.Threading.Tasks;

namespace ExampleProject.Pages
{
    public class MainPage : BaseForm
    {
        public MainPage(IPage page)
            : base(page, page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to the Internet" }), "Main Internet Page")
        {
        }

        private IButton FormAuthenticationLink => ElementFactory.GetButton(
            Page.GetByRole(AriaRole.Link, new() { Name = "Form Authentication" }),
            "Form Auth Link");

        private IButton JavaScriptAlertsLink => ElementFactory.GetButton(
            Page.GetByRole(AriaRole.Link, new() { Name = "JavaScript Alerts" }),
            "Alerts Link");

        public async Task GoToFormAuthenticationAsync()
        {
            await FormAuthenticationLink.ClickAsync();
        }
    }
}