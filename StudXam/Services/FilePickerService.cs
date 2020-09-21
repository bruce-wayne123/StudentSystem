using Plugin.FilePicker;
using StudXam.Helpers;
using StudXam.Interfaces;
using StudXam.Locale;
using StudXam.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudXam.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<FileModel> GetFileAsync(string[] allowedTypes = null, bool shouldShowImagePainting = true)
        {
            var fileDataModelInstance = new FileModel();
            var fileData = await CrossFilePicker.Current.PickFile(allowedTypes);
            if (fileData == null)
            {
                fileDataModelInstance.Message = Strings.NoFilePickedMessage;
            }
            else
            {
                var fileExtension = System.IO.Path.GetExtension(fileData.FilePath);
                var isImageExtension = GlobalConstants.FileTypeExtensionsForImages.Any(a => a == fileExtension);
                fileDataModelInstance.FileName = fileData.FileName;
                fileDataModelInstance.FileType = fileExtension;
                fileDataModelInstance.IsFiledPicked = true;
                fileDataModelInstance.Base6String = Convert.ToBase64String(fileData.DataArray, 0, fileData.DataArray.Length);
            }

            return fileDataModelInstance;
        }
    }
}