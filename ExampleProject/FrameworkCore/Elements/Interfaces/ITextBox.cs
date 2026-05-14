using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface ITextBox : IElement
    {
        Task TypeAsync(string text);
        Task ClearAsync();
    }
}