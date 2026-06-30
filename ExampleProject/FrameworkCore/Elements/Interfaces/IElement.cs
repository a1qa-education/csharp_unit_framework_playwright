using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface IElement
    {
        string Name { get; }

        IElementStateProvider State { get; }

        Task ClickAsync();
        Task<string> GetTextAsync();
    }
}