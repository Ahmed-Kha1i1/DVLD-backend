using AutoMapper;
using BusinessLayer;
using DataLayerCore.User;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (userId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
            }

            var user = await clsUser.FindUser(userId);
            if (user == null)
            {
                return NotFound(CreateResponse(StatusFail, "User not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<UserFullDTO>(user)));
        }

        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser(UserForCreateDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "User object can't be null"));
            }

            var user = _mapper.Map<clsUser>(userDTO);

            if (await user.Save())
            {
                return CreatedAtRoute("GetUserById", new { userId = user.UserID }, CreateResponse(StatusSuccess, user));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding user"));
            }
        }

        [HttpPut("{userId}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int userId, UserForUpdateDTO userDTO)
        {
            if (userId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
            }

            if (userDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "User object can't be null"));
            }

            var user = await clsUser.FindUser(userId);
            if (user == null)
            {
                return NotFound(CreateResponse(StatusFail, $"User with ID {userId} not found."));
            }

            _mapper.Map(userDTO, user);

            if (await user.Save())
            {
                return Ok(CreateResponse(StatusSuccess, user));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating user"));
            }
        }

        [HttpDelete("{userId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
            }

            var userExists = await clsUser.IsUserExistByUserID(userId);
            if (!userExists)
            {
                return NotFound(CreateResponse(StatusFail, "User not found"));
            }

            if (await clsUser.DeleteUser(userId))
            {
                var result = CreateResponse(StatusSuccess, $"User with id {userId} has been deleted.");
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict, CreateResponse(StatusFail, $"Cannot delete user with id {userId}"));
            }
        }

        [HttpGet("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await clsUser.GetAllUsers();
            if (users.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No users found!"));
            }
            return Ok(CreateResponse(StatusSuccess, new { length = users.Count, data = users }));
        }

        [HttpGet("Exists/{userId}", Name = "IsUserExistByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserExistByUserId(int userId)
        {
            if (userId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
            }

            var exists = await clsUser.IsUserExistByUserID(userId);
            return Ok(CreateResponse(StatusSuccess, exists));
        }

        [HttpGet("Exists/Person/{personId}", Name = "IsUserExistByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserExistByPersonId(int personId)
        {
            if (personId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid person id"));
            }

            var exists = await clsUser.IsUserExistByPersonID(personId);
            return Ok(CreateResponse(StatusSuccess, exists));
        }

        [HttpGet("Exists/UserName/{userName}", Name = "IsUserExistByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserExistByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid username"));
            }

            var exists = await clsUser.IsUserExistByUserName(userName);
            return Ok(CreateResponse(StatusSuccess, exists));
        }

        [HttpGet("Active/{userId}", Name = "IsUserActive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserActive(int userId)
        {
            if (userId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
            }

            var isActive = await clsUser.IsUserActive(userId);
            return Ok(CreateResponse(StatusSuccess, isActive));
        }

        [HttpPut("{userId}/Password", Name = "UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePassword(int userId, [FromBody] string newPassword)
        {
            if (userId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
            }

            var userExists = await clsUser.IsUserExistByUserID(userId);
            if (!userExists)
            {
                return NotFound(CreateResponse(StatusFail, "User not found"));
            }

            if (await clsUser.UpdatePassword(userId, newPassword))
            {
                return Ok(CreateResponse(StatusSuccess, "Password updated successfully"));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating password"));
            }
        }
    }
}
