using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface IComboBox : IElement
    {
        Task SelectByTextAsync(string text);
        Task SelectByValueAsync(string value);
        Task SelectByIndexAsync(int index);
        Task<string> GetSelectedOptionTextAsync();
    }
}