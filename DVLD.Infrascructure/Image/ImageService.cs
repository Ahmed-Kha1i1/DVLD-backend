using DVLD.Application.Common.Enums;
using DVLD.Application.Features.Images.Queries.GetImage;
using DVLD.Application.Infrastracture;
using DVLD.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace DVLD.Application.Services.ImageService
{
    public class ImageService : IImageService
    {

        ImagesOptions _ImagesOptions;
        FileExtensionContentTypeProvider _filePrivider;
        public ImageService(IOptionsSnapshot<ImagesOptions> imagesOptions, FileExtensionContentTypeProvider filePrivider)
        {
            _ImagesOptions = imagesOptions.Value;
            _filePrivider = filePrivider;
        }

        public GetImageQueryResponse? GetImage(string fileName)
        {
            var uploadDirectory = _ImagesOptions.ImagesDirectory;

            if (string.IsNullOrEmpty(uploadDirectory) || !Directory.Exists(uploadDirectory))
            {
                return null;
            }
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                return null;
            }

            // Open the image file for reading
            var image = File.OpenRead(filePath);

            if (!_filePrivider.TryGetContentType(fileName, out var contentType))
            {
                return default;
            }
            return new GetImageQueryResponse(image, contentType);
        }

        public async Task<string> UploadImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "";
            }

            var ImagesDirectory = _ImagesOptions.ImagesDirectory;

            if (string.IsNullOrEmpty(ImagesDirectory))
            {
                return "";
            }

            if (!Directory.Exists(ImagesDirectory))
            {
                Directory.CreateDirectory(ImagesDirectory);
            }

            var FileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var FilePath = Path.Combine(ImagesDirectory, FileName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return FileName;
        }

        public async Task<bool> DeleteImageAsync(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                return false;
            };

            var ImagesDirectory = _ImagesOptions.ImagesDirectory;

            if (!Directory.Exists(ImagesDirectory))
            {
                return false;
            }

            var FilePath = Path.Combine(ImagesDirectory, ImageName);

            if (File.Exists(FilePath))
            {
                await Task.Run(() => File.Delete(FilePath));
                return true;
            }

            return false;
        }

        public async Task<(enUpdateImage ImageStatus, string NewIMage)> ReplaceImageAsync(IFormFile? ImageFile, string? OldImageName, bool RemoveImage)
        {
            var newImageName = await UploadImageAsync(ImageFile);

            if (ImageFile != null && string.IsNullOrEmpty(newImageName))
            {
                return (enUpdateImage.UpdatedFail, "");
            }
            bool Deleted = false;
            if (!string.IsNullOrEmpty(OldImageName) && (RemoveImage || !string.IsNullOrEmpty(newImageName)))
            {
                Deleted = await DeleteImageAsync(OldImageName);
                if (!Deleted)
                {
                    return (enUpdateImage.DeletedFail, "");
                }
            }

            if (!string.IsNullOrEmpty(newImageName))
            {
                return (enUpdateImage.Updated, newImageName);
            }


            return Deleted ? (enUpdateImage.DeletedWithoutUpdate, "") : (enUpdateImage.None, "");
        }
    }
}
