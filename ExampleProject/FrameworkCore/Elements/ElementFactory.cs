using Microsoft.Playwright;
using System;
using FrameworkCore.Elements.Interfaces;

namespace FrameworkCore.Elements
{
    public class ElementFactory : IElementFactory
    {
        public T Get<T>(ILocator locator, string name) where T : IElement
        {
            return (T)Activator.CreateInstance(typeof(T), locator, name)!;
        }

        public IButton GetButton(ILocator locator, string name) => Get<Button>(locator, name);
        public ITextBox GetTextBox(ILocator locator, string name) => Get<TextBox>(locator, name);
        public ILabel GetLabel(ILocator locator, string name) => Get<Label>(locator, name);
        public ICheckBox GetCheckBox(ILocator locator, string name) => Get<CheckBox>(locator, name);
        public IComboBox GetComboBox(ILocator locator, string name) => Get<ComboBox>(locator, name);
    }
}