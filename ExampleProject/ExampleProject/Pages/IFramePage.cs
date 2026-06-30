using FrameworkCore.Elements.Interfaces;
using FrameworkCore.Elements;
using FrameworkCore.Forms;
using FrameworkCore.Utilities;
using Microsoft.Playwright;

namespace ExampleProject.Pages
{
    public class IFramePage : BaseForm
    {
        private readonly IElementFactory _elementFactory = new ElementFactory();

        public IFramePage(IPage page) 
            : base(page, page.Locator("h3").Filter(new() { HasText = "An iFrame containing the TinyMCE WYSIWYG Editor" }), "IFrame Page")
        {
        }

        // Elements must be private in POM
        private ITextBox EditorTextBox => _elementFactory.GetTextBox(
            FrameUtils.GetLocatorInFrame(Page, "#mce_0_ifr", "#tinymce"), 
            "TinyMCE Editor Body");

        // Public methods for interaction
        public async Task<string> GetEditorTextAsync()
        {
            return await EditorTextBox.GetTextAsync();
        }
    }
}
