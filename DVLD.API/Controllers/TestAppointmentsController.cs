using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.TestAppointment.Commands.AddTestAppointmentCommand;
using DVLD.Application.Features.TestAppointment.Commands.UpdateTestAppointmentCommand;
using DVLD.Application.Features.TestAppointment.Common.Models;
using DVLD.Application.Features.TestAppointment.Common.Requests.ListPerTestTypeRequest;
using DVLD.Application.Features.TestAppointment.Queries.GetTestAppointmentQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/TestAppointments")]
    [ApiController]
    public class TestAppointmentsController(ITestAppointmentRepository testAppointmentRepository) : AppControllerBase
    {

        [HttpGet("All/PerTestType", Name = "GetAllTestAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTestAppointments([FromQuery] LocalAppTestTypeRequest request)
        {
            var result = await testAppointmentRepository.ListPerTestTypeAsync(request.LocalApplicationId, request.TestTypeId);
            return CreateResult(new Response<IReadOnlyList<TestAppointmentOverviewDTO>>(HttpStatusCode.OK, result));
        }

        [HttpGet("{Id:Int}", Name = "GetTestAppointmentById")] // TestApointmetnId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTestAppointmentById([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetTestAppointmentQuery(request.Id));
            return CreateResult(result);
        }

        [HttpPost(Name = "AddTestAppointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTestAppointment(AddTestAppointmentCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetTestAppointmentById", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }

        [HttpPut(Name = "UpdateTestAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTestAppointment(UpdateTestAppointmentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }

    }
}

