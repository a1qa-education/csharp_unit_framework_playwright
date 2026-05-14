using Microsoft.Playwright;
using FrameworkCore.Forms;
using FrameworkCore.Elements.Interfaces;
using System.Threading.Tasks;

namespace ExampleProject.Pages
{
    public class LoginPage : BaseForm
    {
        // The unique locator defining this page is the login button
        public LoginPage(IPage page)
            : base(page, page.GetByRole(AriaRole.Button, new() { Name = "Login" }), "Login Page")
        {
        }

        // Element Definitions using Native Playwright Locators
        private ITextBox UsernameInput => ElementFactory.GetTextBox(Page.Locator("#username"), "Username Field");
        private ITextBox PasswordInput => ElementFactory.GetTextBox(Page.Locator("#password"), "Password Field");
        private IButton SubmitButton => ElementFactory.GetButton(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }), "Submit Button");

        // This label appears dynamically after a login attempt
        private ILabel FlashAlert => ElementFactory.GetLabel(Page.Locator("#flash"), "Flash Alert Banner");

        public async Task LoginAsAsync(string username, string password)
        {
            await UsernameInput.TypeAsync(username);
            await PasswordInput.TypeAsync(password);
            await SubmitButton.ClickAsync();
        }

        public async Task<string> GetAlertMessageAsync()
        {
            // We wait for the alert to be displayed before trying to extract its text
            await FlashAlert.State.WaitForDisplayedAsync();
            return await FlashAlert.GetTextAsync();
        }
    }
}