using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DVLD.Application.Common.Enums;
using DVLD.Application.Features.Images.Queries.GetImage;
using DVLD.Application.Infrastracture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
namespace DVLD.Infrastructure.Image.Cloudinary
{
    public class CloudinaryService : IImageService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        private readonly CloudinaryOptions _cloudinaryOptions;
        private readonly FileExtensionContentTypeProvider _filePrivider;
        public CloudinaryService(IOptionsSnapshot<CloudinaryOptions> cloudinaryOptions, FileExtensionContentTypeProvider filePrivider)
        {
            _cloudinaryOptions = cloudinaryOptions.Value;
            Account cloundAccount = new Account(_cloudinaryOptions.CouldName, _cloudinaryOptions.ApiKey, _cloudinaryOptions.ApiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(cloundAccount);
            _filePrivider = filePrivider;
        }

        private string GetPublicId(string FileName)
        {
            string PublidId = $"{_cloudinaryOptions.Folder}/{FileName.Substring(0, FileName.LastIndexOf("."))}";
            return PublidId;
        }

        public async Task<bool> DeleteImageAsync(string ImageName)
        {
            string OldPublicId = GetPublicId(ImageName);
            var DeleteResult = await _cloudinary.DeleteResourcesAsync(ResourceType.Image, OldPublicId);
            bool Deleted = DeleteResult.StatusCode == System.Net.HttpStatusCode.OK;
            return Deleted;
        }

        public async Task<GetImageQueryResponse?> GetImageAsync(string fileName)
        {
            string PublicId = GetPublicId(fileName);
            var imageUrI = _cloudinary.Api.UrlImgUp.Transform(new Transformation().Quality("auto").FetchFormat("auto")).BuildUrl(PublicId);

            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(imageUrI);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            if (!_filePrivider.TryGetContentType(fileName, out var contentType))
            {
                return null;
            }

            return new GetImageQueryResponse(await response.Content.ReadAsStreamAsync(), contentType);
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

                string OldPublicId = GetPublicId(OldImageName);
                var DeleteResult = await _cloudinary.DeleteResourcesAsync(ResourceType.Image, OldPublicId);
                Deleted = DeleteResult.StatusCode == System.Net.HttpStatusCode.OK;

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

        public async Task<string> UploadImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "";
            }

            ImageUploadResult UploadResult = null;
            var guit = Guid.NewGuid().ToString();
            var FileName = guit + Path.GetExtension(imageFile.FileName);

            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(FileName, stream),
                    Folder = _cloudinaryOptions.Folder,
                    PublicId = guit
                };
                UploadResult = await _cloudinary.UploadAsync(uploadParams);
            };

            if (UploadResult == null || UploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return UploadResult.Error.Message;
            }

            return FileName;
        }
    }
}

