using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.TestType.Commands.UpdateTestTypeCommand;
using DVLD.Application.Features.TestType.Common.Models;
using DVLD.Application.Features.TestType.Queries.GetTestTypeGuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/TestTypes")]
    [ApiController]
    public class TestTypesController(ITestTypeRepository testTypeRepository) : AppControllerBase
    {
        [HttpGet("{Id:Int}", Name = "GetTestType")] // testTypeId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestType([FromRoute()] IdRequest request)
        {
            var user = await _mediator.Send(new GetTestTypeGuery(request.Id));
            return CreateResult(user);
        }
        [HttpGet("All", Name = "GetAllTestTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTestTypes()
        {
            var result = await testTypeRepository.ListOverviewAsync();
            return CreateResult(new Response<IReadOnlyList<TestTypeDTO>>(HttpStatusCode.OK, result));
        }

        [HttpPut(Name = "UpdateTestType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTestType(UpdateTestTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }

    }
}
