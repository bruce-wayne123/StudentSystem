using StudXam.Models;
using System.Threading.Tasks;

namespace StudXam.Interfaces
{
    public interface IFilePickerService
    {
        Task<FileModel> GetFileAsync(string[] allowedTypes = null, bool shouldShowImagePainting = true);
    }
}