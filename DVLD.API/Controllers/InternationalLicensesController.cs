using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.InternationalLicense.Commands.AddInternationalLicenseCommand;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseApplicationQuery;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicensesQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/InternationalLicenses")]
    [ApiController]
    public class InternationalLicensesController(IInternationalLicenseRepository internationalLicenseRepository, IMediator mediator) : AppControllerBase
    {
        [HttpGet("All", Name = "GetAllInternationalLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllInternationalLicenses([FromQuery] GetInternationalLicensesQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }

        [HttpGet("{Id}/Application", Name = "GetInternationalLicenseApplication")] // internationalLicenseId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInternationalLicenseApplication([FromRoute] IdRequest request)
        {
            var result = await mediator.Send(new GetInternationalLicenseApplicationQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("{Id:Int}", Name = "GetInternationalLicense")]// internationalLicenseId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInternationalLicense([FromRoute] IdRequest request)
        {
            var result = await mediator.Send(new GetInternationalLicenseQuery(request.Id));
            return CreateResult(result);
        }

        [HttpPost(Name = "AddInternationalLicense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddInternationalLicense(AddInternationalLicenseCommand command)
        {
            var result = await mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetInternationalLicense", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }

    }
}
