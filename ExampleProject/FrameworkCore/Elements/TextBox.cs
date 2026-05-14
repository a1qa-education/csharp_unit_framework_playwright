using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class TextBox(ILocator locator, string name) : BaseElement(locator, name), ITextBox
    {
        public async Task TypeAsync(string text)
        {
            Logger.Info($"Typing '{text}' into {GetType().Name} '{Name}'");
            await Locator.FillAsync(text);
        }

        public async Task ClearAsync()
        {
            Logger.Info($"Clearing text from {GetType().Name} '{Name}'");
            await Locator.ClearAsync();
        }
    }
}