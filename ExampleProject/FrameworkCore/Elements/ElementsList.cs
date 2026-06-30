using Microsoft.Playwright;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrameworkCore.Elements.Interfaces;
using FrameworkCore.Utilities;

namespace FrameworkCore.Elements
{
    public class ElementsList<T> : IElementsList<T> where T : IElement
    {
        private readonly ILocator _locator;
        private readonly ElementFactory _elementFactory;
        public string Name { get; }

        public ElementsList(ILocator locator, string name)
        {
            _locator = locator;
            Name = name;
            _elementFactory = new ElementFactory();
        }

        public async Task<int> CountAsync()
        {
            Logger.Info($"Getting count of elements in list '{Name}'");
            var count = await _locator.CountAsync();
            Logger.Info($"List '{Name}' contains {count} element(s)");
            return count;
        }

        public async Task<IReadOnlyList<T>> GetElementsAsync()
        {
            Logger.Info($"Getting all elements from list '{Name}'");
            var locators = await _locator.AllAsync();
            var elements = locators
                .Select((loc, index) => _elementFactory.Get<T>(loc, $"{Name} [{index}]"))
                .ToList();
            Logger.Info($"Found {elements.Count} element(s) in list '{Name}'");
            return elements;
        }

        public T GetElementByIndex(int index)
        {
            Logger.Info($"Getting element at index {index} from list '{Name}'");
            var locator = _locator.Nth(index);
            return _elementFactory.Get<T>(locator, $"{Name} [{index}]");
        }

        public async Task<IReadOnlyList<string>> GetTextsAsync()
        {
            Logger.Info($"Getting texts from all elements in list '{Name}'");
            var texts = await _locator.AllInnerTextsAsync();
            Logger.Info($"Got {texts.Count} text(s) from list '{Name}'");
            return texts;
        }
    }
}
