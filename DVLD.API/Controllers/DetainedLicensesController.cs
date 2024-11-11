using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.DetainedLicense.Commands.DetainLicenseCommand;
using DVLD.Application.Features.DetainedLicense.Commands.ReleaseDetainedLicenseCommand;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicenseQuery;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesByDateRangeQuery;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/DetainedLicenses")]
    [ApiController]
    public class DetainedLicensesController(IDetainedLicenseRepository detainedLicenseRepository) : AppControllerBase
    {
        [HttpGet("All", Name = "GetAllDetainedLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDetainedLicenses([FromQuery] GetDetainedLicensesQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }

        [HttpGet("stats", Name = "GetDetainedLIcenesStats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetainedLIcenesStats([FromQuery] GetDetainedLicensesByDateRangeQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }

        [HttpGet("{Id:int}", Name = "GetDetainedLicense")]// DetainId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetainedLicense([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetDetainedLicenseQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("ByLicenseId/{Id:int}", Name = "GetDetainedLicenseByLicensId")]// LicenseId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetainedLicenseByLicensId([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetDetainedLicenseByLicenseIdQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("IsDetained/{id}", Name = "IsLicenseDetained")] //LicenseId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsLicenseDetained([FromRoute()] IdRequest request)
        {
            var result = await detainedLicenseRepository.IsLicenseDetained(request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpPost("Detain", Name = "DetainLicense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DetainLicense(DetainLicenseCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetDetainedLicense", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }

        [HttpPut("Release", Name = "ReleaseLicense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReleaseLicense(ReleaseDetainedLicenseCommand command)
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
