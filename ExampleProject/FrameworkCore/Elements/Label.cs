using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class Label(ILocator locator, string name) : BaseElement(locator, name), ILabel
    {
        public async Task<string> GetTextAsync()
        {
            Logger.Info($"Getting text from {GetType().Name} '{Name}'");
            return await Locator.InnerTextAsync();
        }
    }
}