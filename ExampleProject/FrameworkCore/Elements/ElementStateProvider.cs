using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class ElementStateProvider : IElementStateProvider
    {
        private readonly ILocator _locator;
        private readonly string _name;

        public ElementStateProvider(ILocator locator, string name)
        {
            _locator = locator;
            _name = name;
        }

        public async Task<bool> IsDisplayedAsync()
        {
            Logger.Info($"Checking if element '{_name}' is displayed.");
            return await _locator.IsVisibleAsync();
        }

        public async Task<bool> IsExistAsync()
        {
            Logger.Info($"Checking if element '{_name}' exists in the DOM.");
            return await _locator.CountAsync() > 0;
        }

        public async Task<bool> IsEnabledAsync()
        {
            Logger.Info($"Checking if element '{_name}' is enabled.");
            return await _locator.IsEnabledAsync();
        }

        public async Task WaitForDisplayedAsync()
        {
            Logger.Info($"Waiting for element '{_name}' to become displayed.");
            await _locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }

        public async Task WaitForNotDisplayedAsync()
        {
            Logger.Info($"Waiting for element '{_name}' to disappear.");
            await _locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden });
        }

        public async Task WaitForEnabledAsync()
        {
            // Playwright auto-waits for actionability on click/type, but sometimes 
            // you need an explicit wait for business logic validation.
            Logger.Info($"Waiting for element '{_name}' to become enabled.");

            // In Playwright, an element is considered enabled if it is attached, visible, and not disabled.
            // We can wait for the 'disabled' attribute to be removed or use a custom assertion.
            // A clean way to force a wait for state without an action:
            await _locator.Locator("xpath=self::*[not(@disabled)]").WaitForAsync();
        }
    }
}