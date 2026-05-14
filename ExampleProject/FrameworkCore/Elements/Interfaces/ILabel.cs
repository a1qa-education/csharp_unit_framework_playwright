using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface ILabel : IElement
    {
        Task<string> GetTextAsync();
    }
}