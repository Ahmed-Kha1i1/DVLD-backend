using ApiLayer.Configurations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using static DVLDApi.Helpers.ApiResponse;

namespace ApiLayer.Controllers
{
    [Route("api/Files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        FileExtensionContentTypeProvider _FilePrivider;
        IOptionsSnapshot<ImagesOptions> _ImagesOptions;
        public FilesController(FileExtensionContentTypeProvider filePrivider, IOptionsSnapshot<ImagesOptions> imagesOptions)
        {
            _FilePrivider = filePrivider;
            _ImagesOptions = imagesOptions;
        }

        [HttpGet("GetImage/{fileName}", Name = "GetImage")]
        public IActionResult GetImage(string fileName)
        {
            // Directory where files are stored
            var uploadDirectory = _ImagesOptions.Value.ImagesDirectory;

            if (uploadDirectory == null)
                return NotFound(CreateResponse(StatusFail, "ImageService not found."));

            var filePath = Path.Combine(uploadDirectory, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(CreateResponse(StatusFail, "ImageService not found."));
            }

            // Open the image file for reading
            var image = System.IO.File.OpenRead(filePath);
            if (!_FilePrivider.TryGetContentType(fileName, out var contentType))
            {
                return BadRequest(CreateResponse(StatusFail, "File name not valid"));
            }

            // Return the file with the correct MIME type
            return File(image, contentType);
        }
        
    }
}
