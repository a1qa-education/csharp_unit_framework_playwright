using Microsoft.Playwright;

namespace FrameworkCore.Elements.Interfaces
{
    public interface IElementFactory
    {
        T Get<T>(ILocator locator, string name) where T : IElement;

        IButton GetButton(ILocator locator, string name);
        ITextBox GetTextBox(ILocator locator, string name);
        ILabel GetLabel(ILocator locator, string name);
        ICheckBox GetCheckBox(ILocator locator, string name);
        IComboBox GetComboBox(ILocator locator, string name);
    }
}