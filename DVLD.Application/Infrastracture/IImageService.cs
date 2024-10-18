using DVLD.Application.Common.Enums;
using DVLD.Application.Features.Images.Queries.GetImage;
using Microsoft.AspNetCore.Http;

namespace DVLD.Application.Infrastracture
{
    public interface IImageService
    {
        GetImageQueryResponse? GetImage(string fileName);
        Task<string> UploadImageAsync(IFormFile? imageFile);
        Task<bool> DeleteImageAsync(string ImageName);
        Task<(enUpdateImage ImageStatus, string NewIMage)> ReplaceImageAsync(IFormFile? ImageFile, string? OldImageName, bool RemoveImage);
    }
}
