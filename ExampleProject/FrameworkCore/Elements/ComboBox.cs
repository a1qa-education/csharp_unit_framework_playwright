using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class ComboBox(ILocator locator, string name) : BaseElement(locator, name), IComboBox
    {
        public async Task SelectByTextAsync(string text)
        {
            Logger.Info($"Selecting option '{text}' from {GetType().Name} '{Name}'");
            await Locator.SelectOptionAsync(new[] { new SelectOptionValue { Label = text } });
        }

        public async Task SelectByValueAsync(string value)
        {
            Logger.Info($"Selecting value '{value}' from {GetType().Name} '{Name}'");
            await Locator.SelectOptionAsync(new[] { new SelectOptionValue { Value = value } });
        }
    }
}