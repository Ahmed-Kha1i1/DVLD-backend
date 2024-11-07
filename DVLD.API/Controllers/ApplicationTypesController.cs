using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.ApplicationType.Commands.UpdateApplicationTypeCommand;
using DVLD.Application.Features.ApplicationType.Common.Models;
using DVLD.Application.Features.ApplicationType.Queries.GetApplicationType;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/ApplicationTypes")]
    [ApiController]
    public class ApplicationTypesController(IApplicationTypeRepository applicationTypeRepository) : AppControllerBase
    {
        [HttpGet("{Id:Int}", Name = "GetApplicationType")] // DrievrId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApplicationType([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetApplicationTypeQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("All", Name = "GetAllApplicationTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllApplicationTypes()
        {
            var result = await applicationTypeRepository.ListOverviewAsync();
            return CreateResult(new Response<IReadOnlyList<ApplicationTypeDTO>>(HttpStatusCode.OK, result));
        }

        [HttpPut(Name = "UpdateApplicationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateApplicationType(UpdateApplicationTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }

    }
}
