using DVLD.API.Base;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.People.Commands.AddPersonCommand;
using DVLD.Application.Features.People.Commands.DeletePersonCommand;
using DVLD.Application.Features.People.Commands.UpdatePersonCommand;
using DVLD.Application.Features.People.Common.Requests.Email.Unique;
using DVLD.Application.Features.People.Common.Requests.NationalNumber;
using DVLD.Application.Features.People.Common.Requests.Phone.Unique;
using DVLD.Application.Features.People.Queries.GetPeopleQuery;
using DVLD.Application.Features.People.Queries.GetPersonQuery;
using DVLD.Application.Features.People.Queries.GetPersonQuery.ByNationalNumber;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopleController(IPersonRepository personRepository) : AppControllerBase
    {

        [HttpGet("{Id:int}", Name = "GetPersonById")] // PersonId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPerson([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetPersonByIdQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("{Id:int}/DriverId", Name = "GetDriverId")] //PersonId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDriverId([FromRoute()] IdRequest request)
        {
            var result = await personRepository.GetDriverId(request.Id);
            return CreateResult(new Response<int?>(HttpStatusCode.OK, result));
        }

        [HttpGet("NationalNumber/{nationalNumber}", Name = "GetPersonByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetPerson([FromRoute()] NationalNumberRequest request)
        {
            var person = await _mediator.Send(new GetPersonByNationalNumberQuery(request.NationalNumber));
            return CreateResult(person);
        }

        [HttpGet("All", Name = "GetAllPersons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPersons([FromQuery] GetPeopleQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }

        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        public async Task<IActionResult> AddPerson([FromForm] AddPersonCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetPersonById", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }

        [HttpPut(Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePerson([FromForm] UpdatePersonCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }

        [HttpDelete("{Id:int}", Name = "DeletePerson")] //PersonId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePerson([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new DeletePersonCommand(request.Id));
            return CreateResult(result);
        }

        [HttpGet("Unique/NationalNumber", Name = "IsNationalNoUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsNationalNoUnique([FromQuery] NationalNumberUniqueRequest request)
        {
            var result = await personRepository.IsNationalNoUnique(request.NationalNumber, request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("Unique/Email", Name = "IsEmaiUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsEmaiUnique([FromQuery] EmailUniqueRequest emailRequest)
        {
            var result = await personRepository.IsEmailUnique(emailRequest.Email, emailRequest.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("Unique/Phone", Name = "IsPhoneUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsPhoneUnique([FromQuery] PhoneUniqueRequest request)
        {
            var result = await personRepository.IsPhoneUnique(request.Phone, request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("Exists/{Id:int}", Name = "IsPersonExistsById")]//DriverId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsPersonExistsById([FromRoute()] IdRequest request)
        {
            var result = await personRepository.IsPersonExists(request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }
    }
}
