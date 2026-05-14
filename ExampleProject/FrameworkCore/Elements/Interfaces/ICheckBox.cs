using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface ICheckBox : IElement
    {
        Task CheckAsync();
        Task UncheckAsync();
        Task<bool> IsCheckedAsync();
    }
}