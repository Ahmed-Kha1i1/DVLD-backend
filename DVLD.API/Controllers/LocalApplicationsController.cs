using DVLD.API.Base;
using DVLD.API.Helpers.ActionConstraints;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.LocalApplication.Commands.AddLocalApplicationCommand;
using DVLD.Application.Features.LocalApplication.Commands.CancelLocalApplicationCommand;
using DVLD.Application.Features.LocalApplication.Commands.DeleteLocalApplicationCommand;
using DVLD.Application.Features.LocalApplication.Commands.IssueLocalApplicationCommand;
using DVLD.Application.Features.LocalApplication.Commands.UpdateLocalApplicationCommand;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationQuery;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationsQuery;
using DVLD.Application.Features.TestAppointment.Common.Requests.ListPerTestTypeRequest;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/LocalApplications")]
    [ApiController]
    public class LocalApplicationsController(ILocalApplicationRepository localApplicationRepository) : AppControllerBase
    {

        [HttpGet("{Id:Int}", Name = "GetLocalApplication")] // LocalApplicationId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType("accept", "application/json", "application/vnd.LocalApplication.full+json")]
        [Produces("application/json", "application/vnd.LocalApplication.full+json")]
        public async Task<IActionResult> GetLocalApplication([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetLocalApplicationQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("{Id:Int}", Name = "GetLocalApplicationPref")] // LocalApplicationId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType("accept", "application/vnd.LocalApplication.pref+json")]
        [Produces("application/vnd.LocalApplication.pref+json")]
        public async Task<IActionResult> GetLocalApplicationPref([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetLocalApplicationPrefQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("PerTestType", Name = "GetApplicationPerTestType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApplicationPerTestType([FromQuery] GetLocalApplicationPerTestTypeQuery request)
        {
            var result = await _mediator.Send(request);
            return CreateResult(result);
        }


        [HttpGet("{Id}/ActiveLicenseId", Name = "GetActiveLicenseId")] // LocalApplicationId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveLicenseId([FromRoute()] IdRequest request)
        {
            int? result = await localApplicationRepository.GetActiveLicenseID(request.Id);
            return CreateResult(new Response<int?>(HttpStatusCode.OK, result));
        }

        [HttpGet("All", Name = "GetLocalDrivingLicenseApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocalDrivingLicenseApplications([FromQuery] GetLocalApplicationsQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }
        [HttpPut(Name = "UpdateLocalApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLocalApplication(UpdateLocalApplicationCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }
        [HttpPost(Name = "AddLocalApplication")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLocalApplication(AddLocalApplicationCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetLocalApplication", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }
        [HttpDelete("{Id:int}", Name = "DeleteLocalApplication")]//LocalApplicationId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteLocalApplication([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new DeleteLocalApplicationCommand(request.Id));
            return CreateResult(result);
        }
        [HttpPut("{Id:int}/Cancel", Name = "CancelLocalApplication")]//LocalApplicationId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CancelLocalApplication([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new CancelLocalApplicationCommand(request.Id));
            return CreateResult(result);
        }
        [HttpPost("issue")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IssueLocalLicense(IssueLocalApplicationCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetLicense", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }

        [HttpGet("TestType/AttendanceCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DoesAttendTestType([FromQuery] LocalAppTestTypeRequest request)
        {
            var result = await localApplicationRepository.DoesAttendTestType(request.LocalApplicationId, (int)request.TestTypeId);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("TestType/PreviousPassCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DoesPassPreviousTest([FromQuery] LocalAppTestTypeRequest request)
        {
            var result = await localApplicationRepository.DoesPassPreviousTest(request.LocalApplicationId, request.TestTypeId);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }
        [HttpGet("TestType/PassCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DoesPassTestType([FromQuery] LocalAppTestTypeRequest request)
        {
            var result = await localApplicationRepository.DoesPassTestType(request.LocalApplicationId, (int)request.TestTypeId);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }
        [HttpGet("TestType/ActiveScheduledCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsThereAnActiveScheduledTest([FromQuery] LocalAppTestTypeRequest request)
        {
            var result = await localApplicationRepository.IsThereAnActiveScheduledTest(request.LocalApplicationId, (int)request.TestTypeId);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }
    }
}
