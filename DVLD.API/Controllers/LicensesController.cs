using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.License.Commands.RenewLicenseCommand;
using DVLD.Application.Features.License.Commands.ReplaceLicenseCommand;
using DVLD.Application.Features.License.Common.Requests.IsLicenseExistsRequest;
using DVLD.Application.Features.License.Queries.GetLicenseQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/Licenses")]
    [ApiController]
    public class LicensesController(ILicenseRepository licenseRepository) : AppControllerBase
    {
        [HttpGet("{Id:Int}", Name = "GetLicense")] // licenseId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLicense([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetLicenseQuery(request.Id));
            return CreateResult(result);
        }
        [HttpGet("IsLicenseExist", Name = "IsLicenseExist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsLicenseExist([FromQuery] IsLicenseExistsRequest request)
        {
            var result = await licenseRepository.IsLicenseExist(request.PersonId, request.LicenseClassId);
            return CreateResult(new Response<bool?>(HttpStatusCode.OK, result));
        }

        [HttpPost("Replace", Name = "Replace")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Replace(ReplaceLicenseCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetApplication", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }

        [HttpPost("Renew", Name = "Renew")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Renew(RenewLicenseCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetApplication", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }
    }
}
