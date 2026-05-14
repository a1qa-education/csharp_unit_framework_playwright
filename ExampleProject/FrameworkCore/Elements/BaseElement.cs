using Microsoft.Playwright;
using System.Threading.Tasks;
using FrameworkCore.Utilities;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public abstract class BaseElement : IElement
    {
        protected readonly ILocator Locator;
        public string Name { get; }
        public IElementStateProvider State { get; }

        protected BaseElement(ILocator locator, string name)
        {
            Locator = locator;
            Name = name;
            State = new ElementStateProvider(Locator, Name);
        }

        public async Task ClickAsync()
        {
            Logger.Info($"Clicking on {GetType().Name} '{Name}'");
            await Locator.ClickAsync();
        }
    }
}