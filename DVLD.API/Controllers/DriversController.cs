using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Driver.Common.Model;
using DVLD.Application.Features.Driver.Queries.GetDriverPersonQuery;
using DVLD.Application.Features.Driver.Queries.GetDriverQuery;
using DVLD.Application.Features.Driver.Queries.GetDriversQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/Drivers")]
    [ApiController]
    public class DriversController(IDriverRepository driverRepository, IInternationalLicenseRepository internationalLicenseRepository) : AppControllerBase
    {

        [HttpGet("{Id:Int}", Name = "GetDriver")] // DrievrId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDriver([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetDriverQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("{Id}/ActiveInternationalLicenseId", Name = "GetActiveInternationalLicenseID")] //DriverId
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveInternationalLicenseID([FromRoute] IdRequest request)
        {
            var result = await internationalLicenseRepository.GetActiveInternationalLicenseIDByDriverID(request.Id);
            return CreateResult(new Response<int?>(HttpStatusCode.OK, result));
        }

        [HttpGet("ByPersonId/{Id}", Name = "GetDriverByPersonId")] // PersonId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDriverByPersonId([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetDriverByPersonIdQuery(request.Id));
            return CreateResult(result);
        }
        [HttpGet("{Id:int}/person", Name = "GetPersonByDriverId")]//DriverId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPerson([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetDriverPersonQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("All", Name = "GetAllDrivers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDrivers([FromQuery] GetDriversQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }
        [HttpGet("{Id}/Licenses", Name = "GetDriverLicenses")] // DriverId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDriverLicenses([FromRoute()] IdRequest request)
        {
            var result = await driverRepository.ListDriverLicensesAsync(request.Id);
            return CreateResult(new Response<IReadOnlyList<DriverLicenseDTO>>(HttpStatusCode.OK, result));
        }

        [HttpGet("{Id}/InternationalLicenses", Name = "GetDriverInternationalLicenses")]// DriverId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDriverInternationalLicenses([FromRoute()] IdRequest request)
        {
            var result = await driverRepository.ListDriverInternationalLicensesAsync(request.Id);
            return CreateResult(new Response<IReadOnlyList<DriverInternationalLicenseDTO>>(HttpStatusCode.OK, result));
        }

    }
}
