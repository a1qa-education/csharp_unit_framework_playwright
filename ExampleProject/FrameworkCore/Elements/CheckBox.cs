using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class CheckBox(ILocator locator, string name) : BaseElement(locator, name), ICheckBox
    {
        public async Task CheckAsync()
        {
            Logger.Info($"Checking {GetType().Name} '{Name}'");
            await Locator.CheckAsync();
        }

        public async Task UncheckAsync()
        {
            Logger.Info($"Unchecking {GetType().Name} '{Name}'");
            await Locator.UncheckAsync();
        }

        public async Task<bool> IsCheckedAsync()
        {
            Logger.Info($"Checking state of {GetType().Name} '{Name}'");
            return await Locator.IsCheckedAsync();
        }
    }
}