using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using static DVLDApi.Helpers.ApiResponse;

namespace ApiLayer.Controllers
{
    [Route("api/Files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        FileExtensionContentTypeProvider _FilePrivider;
        IConfiguration _Configuration;
        public FilesController(FileExtensionContentTypeProvider filePrivider, IConfiguration configuration)
        {
            _FilePrivider = filePrivider;
            _Configuration = configuration;
        }

        [HttpGet("GetImage/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            // Directory where files are stored
            var uploadDirectory = _Configuration["ImagesDirectory"];

            if (uploadDirectory == null)
                return NotFound(CreateResponse(StatusFail, "Image not found."));

            var filePath = Path.Combine(uploadDirectory, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(CreateResponse(StatusFail, "Image not found."));
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
