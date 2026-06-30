using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface IElementsList<T> where T : IElement
    {
        string Name { get; }

        Task<int> CountAsync();
        Task<IReadOnlyList<T>> GetElementsAsync();
        T GetElementByIndex(int index);
        Task<IReadOnlyList<string>> GetTextsAsync();
    }
}
