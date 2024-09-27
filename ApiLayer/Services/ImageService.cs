using ApiLayer.Configurations;
using Microsoft.Extensions.Options;

namespace ApiLayer.Services
{
    public enum enUpdateImage
    {
        None,
        Updated,
        DeletedWithoutUpdate,
        DeletedFail,
        UpdatedFail
    }
    public class ImageService
    {

        ImagesOptions _ImagesOptions;
        public ImageService(IOptions<ImagesOptions> imagesOptions)
        {
            _ImagesOptions = imagesOptions.Value;
        }

        public async Task<string> UploadImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "";
            }

            var ImagesDirectory = _ImagesOptions.ImagesDirectory;

            if (ImagesDirectory == null)
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
                return Deleted ? (enUpdateImage.Updated, newImageName) : (enUpdateImage.None, "");
            }


            return Deleted ? (enUpdateImage.DeletedWithoutUpdate, "") : (enUpdateImage.None, "");
        }
    }
}
