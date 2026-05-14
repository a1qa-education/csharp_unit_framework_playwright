using Microsoft.Playwright;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class Button(ILocator locator, string name) : BaseElement(locator, name), IButton
    {
    }
}