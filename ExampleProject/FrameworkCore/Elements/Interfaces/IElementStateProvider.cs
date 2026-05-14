using System.Threading.Tasks;

namespace FrameworkCore.Elements.Interfaces
{
    public interface IElementStateProvider
    {
        // State checks
        Task<bool> IsDisplayedAsync();
        Task<bool> IsExistAsync();
        Task<bool> IsEnabledAsync();

        // Wait strategies
        Task WaitForDisplayedAsync();
        Task WaitForNotDisplayedAsync();
        Task WaitForEnabledAsync();
    }
}