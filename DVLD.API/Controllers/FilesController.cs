using DVLD.API.Base;
using DVLD.Application.Features.Images.Queries.GetImage;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/Files")]
    [ApiController]
    public class FilesController : AppControllerBase
    {

        [HttpGet("GetImage/{fileName}", Name = "GetImageAsync")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            var result = await _mediator.Send(new GetImageQuery(fileName));
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                return CreateResult(result);

            return File(result.Data.Image, result.Data.ContentType);
        }

    }
}
