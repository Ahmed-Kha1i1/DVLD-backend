using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Features.Test.Commands.AddTestCommand;
using DVLD.Application.Features.Test.Commands.GetTestQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/Tests")]
    [ApiController]

    public class TestsController : AppControllerBase
    {
        [HttpGet("{Id:Int}", Name = "GetTest")] // TestId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTest([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetTestQuery(request.Id));
            return CreateResult(result);
        }

        [HttpPost(Name = "AddNewTest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewTest(AddTestCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetTest", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }
    }
}
