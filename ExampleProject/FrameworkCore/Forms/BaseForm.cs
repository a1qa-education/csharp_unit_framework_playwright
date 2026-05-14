using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Forms.Interfaces;
using FrameworkCore.Elements;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Forms
{
    public abstract class BaseForm : IForm
    {
        protected readonly IPage Page;
        protected readonly IElementFactory ElementFactory;

        private readonly ILocator _uniqueFormLocator;
        public string Name { get; }

        protected BaseForm(IPage page, ILocator uniqueFormLocator, string name)
        {
            Page = page;
            _uniqueFormLocator = uniqueFormLocator;
            Name = name;

            ElementFactory = new ElementFactory();
        }

        public async Task WaitForDisplayedAsync()
        {
            Logger.Info($"Waiting for form '{Name}' to be displayed");
            await _uniqueFormLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }

        public async Task<bool> IsDisplayedAsync()
        {
            Logger.Info($"Checking if form '{Name}' is currently displayed");
            return await _uniqueFormLocator.IsVisibleAsync();
        }
    }
}