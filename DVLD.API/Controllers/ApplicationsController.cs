using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Application.Common.Model;
using DVLD.Application.Features.Application.Common.Requests.GetApplicationId;
using DVLD.Application.Features.Application.Queries.GetApplicationQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/Applications")]
    [ApiController]
    public class ApplicationsController(IApplicationRepository applicationRepository) : AppControllerBase
    {
        [HttpGet("All", Name = "GetAllApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllApplications()
        {
            var result = await applicationRepository.ListOverviewAsync();
            return CreateResult(new Response<IReadOnlyList<ApplicationOverviewDTO>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{Id:Int}", Name = "GetApplication")] // ApplicationId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApplication([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetApplicationQuery(request.Id));
            return CreateResult(result);
        }


        [HttpGet("ActiveApplicationId", Name = "GetActiveApplicationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveApplicationId([FromQuery] GetApplicationIdRequest request)
        {
            var result = await applicationRepository.GetActiveApplicationId(request.PersonId, request.LicenseClassId);
            return CreateResult(new Response<int?>(HttpStatusCode.OK, result));
        }
    }
}
