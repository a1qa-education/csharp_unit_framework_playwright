using FrameworkCore.Elements.Interfaces;
using FrameworkCore.Elements;
using FrameworkCore.Forms;
using Microsoft.Playwright;

namespace ExampleProject.Pages
{
    public class DropdownPage : BaseForm
    {
        private readonly IElementFactory _elementFactory = new ElementFactory();

        public DropdownPage(IPage page) 
            : base(page, page.Locator("h3").Filter(new() { HasText = "Dropdown List" }), "Dropdown Page")
        {
        }

        // Elements must be private in POM
        private IComboBox DropdownMenu => _elementFactory.GetComboBox(Page.Locator("#dropdown"), "Options Dropdown");

        // Public methods for interaction
        public async Task SelectOptionByIndexAsync(int index)
        {
            await DropdownMenu.SelectByIndexAsync(index);
        }

        public async Task SelectOptionByValueAsync(string value)
        {
            await DropdownMenu.SelectByValueAsync(value);
        }

        public async Task SelectOptionByTextAsync(string text)
        {
            await DropdownMenu.SelectByTextAsync(text);
        }

        public async Task<string> GetSelectedOptionTextAsync()
        {
            return await DropdownMenu.GetSelectedOptionTextAsync();
        }
    }
}
