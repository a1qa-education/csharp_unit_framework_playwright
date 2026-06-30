using Microsoft.Playwright;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class Label(ILocator locator, string name) : BaseElement(locator, name), ILabel
    {
    }
}