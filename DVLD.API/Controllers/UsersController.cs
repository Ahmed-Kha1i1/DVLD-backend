using DVLD.API.Base;
using DVLD.API.Helpers.ActionConstraints;
using DVLD.Application.Common.Requests.Id;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.People.Queries.GetPersonQuery;
using DVLD.Application.Features.Users.Commands.AddUserCommand;
using DVLD.Application.Features.Users.Commands.DeleteUserCommand;
using DVLD.Application.Features.Users.Commands.UpdatePasswordCommand;
using DVLD.Application.Features.Users.Commands.UpdateUserCommand;
using DVLD.Application.Features.Users.Common.Requests.PasswordValid;
using DVLD.Application.Features.Users.Common.Requests.Username;
using DVLD.Application.Features.Users.Common.Requests.Username.Unique;
using DVLD.Application.Features.Users.Queries.GetUserQuery;
using DVLD.Application.Features.Users.Queries.GetUsersQuery;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DVLD.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController(IUserRepository userRepository) : AppControllerBase
    {
        [HttpGet("{Id:Int}", Name = "GetUserFull")] // UserID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType("accept", "application/json", "application/vnd.user.full+json")]
        [Produces("application/json", "application/vnd.user.full+json")]
        public async Task<IActionResult> GetUser([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetUserQuery(request.Id));
            return CreateResult(result);
        }

        [HttpGet("{Id:Int}", Name = "GetUserOverview")]//UserID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType("accept", "application/vnd.user.pref+json")]
        [Produces("application/vnd.user.pref+json")]
        public async Task<IActionResult> GetUserOverview([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetUserOverviewQuery(request.Id));
            return CreateResult(result);
        }
        [HttpGet("{Id:int}/person", Name = "GetPersonByUserId")]//UserID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPerson([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new GetUserPersonQuery(request.Id));
            return CreateResult(result);
        }

        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser(AddUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtRoute("GetUserFull", new { Id = result.Data }, result);
            }
            return CreateResult(result);
        }
        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }
        [HttpPut("UpdatePassword", Name = "UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }
        [HttpDelete("{Id:Int}", Name = "DeleteUser")] //userId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromRoute()] IdRequest request)
        {
            var result = await _mediator.Send(new DeleteUserCommand(request.Id));
            return CreateResult(result);
        }

        [HttpGet("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResult(result);
        }

        [HttpGet("Exists/{Id}", Name = "IsUserExistByUserId")]//personId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserExistByUserId([FromRoute()] IdRequest request)
        {
            var result = await userRepository.IsUserExistByUserId(request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }


        [HttpGet("Exists/Person/{Id}", Name = "IsUserExistByPersonId")]//personId
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserExistByPersonId([FromRoute()] IdRequest request)
        {
            var result = await userRepository.IsUserExistByPersonId(request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("Exists/UserName/{userName}", Name = "IsUserExistByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserExistByUserName([FromRoute()] UsernameRequest request)
        {
            var result = await userRepository.IsUserExistByUserName(request.Username);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("Active/{Id}", Name = "IsUserActive")]//UserID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserActive([FromRoute()] IdRequest request)
        {
            var result = await userRepository.IsUserActive(request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("Unique/Username", Name = "IsUsernameUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUsernameUnique([FromQuery] UsernameUniqueRequest request)
        {
            var result = await userRepository.IsUsernameUnique(request.Username, request.Id);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

        [HttpGet("PasswordValid", Name = "IsPasswordValid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsPasswordValid([FromQuery()] PasswordValidRequest request)
        {
            var result = await userRepository.IsPasswordValid(request.UserId, request.CurrentPassword);
            return CreateResult(new Response<bool>(HttpStatusCode.OK, result));
        }

    }
}
