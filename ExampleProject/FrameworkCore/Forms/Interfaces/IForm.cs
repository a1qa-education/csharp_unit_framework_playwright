using System.Threading.Tasks;

namespace FrameworkCore.Forms.Interfaces
{
    public interface IForm
    {
        string Name { get; }
        Task WaitForDisplayedAsync();
        Task<bool> IsDisplayedAsync();
    }
}