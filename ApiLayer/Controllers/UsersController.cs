using ApiLayer.ActionConstraints;
using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer;
using DataLayerCore.Person;
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
        [HttpGet("person/{userId:int}", Name = "GetPersonByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("userId")]
        public async Task<IActionResult> GetPerson(int userId)
        {
            var person = await clsUser.FindPerson(userId);
            if (person == null)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            var personDto = _mapper.Map<PersonFullDTO>(person);

            return Ok(CreateResponse(StatusSuccess, personDto));
        }


        [HttpGet("{userId}", Name = "GetUserFull")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType("accept", "application/json", "application/vnd.user.full+json")]
        [Produces("application/json", "application/vnd.user.full+json")]
        [ValidateId("userId")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await clsUser.FindUser(userId);
            if (user == null)
            {
                return NotFound(CreateResponse(StatusFail, "User not found"));
            }

            var userDTO = _mapper.Map<UserFullDTO>(user);
            return Ok(CreateResponse(StatusSuccess, userDTO));
        }


        [HttpGet("{userId}", Name = "GetUserPref")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType("accept", "application/vnd.user.pref+json")]
        [Produces("application/vnd.user.pref+json")]
        [ValidateId("userId")]
        public async Task<IActionResult> GetUserPref(int userId)
        {
            var user = await clsUser.FindUser(userId);
            if (user == null)
            {
                return NotFound(CreateResponse(StatusFail, "User not found"));
            }

            var userDTO = _mapper.Map<UserPrefDTO>(user);
            return Ok(CreateResponse(StatusSuccess, userDTO));
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

            if (await user.Save(userDTO.Password))
            {
                return CreatedAtRoute("GetUserById", new { userId = user.UserID }, CreateResponse(StatusSuccess, new { user.UserID }));
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
        [ValidateId("userId")]
        public async Task<IActionResult> UpdateUser(int userId, UserForUpdateDTO userDTO)
        {
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
                return Ok(CreateResponse(StatusSuccess, _mapper.Map<UserFullDTO>(user)));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating user"));
            }
        }

        [HttpPut("UpdatePassword/{userId}", Name = "UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("userId")]
        public async Task<IActionResult> UpdatePassword(int userId, [FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "UpdatePassword object can't be null"));
            }


            if (await clsUser.UpdatePassword(userId, updatePasswordDTO))
            {
                return Ok(CreateResponse(StatusSuccess));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating password"));
            }
        }


        [HttpDelete("{userId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("userId")]
        public async Task<IActionResult> DeleteUser(int userId)
        {


            var userExists = await clsUser.IsUserExistByUserID(userId);
            if (!userExists)
            {
                return NotFound(CreateResponse(StatusFail, "User not found"));
            }

            if (await clsUser.DeleteUser(userId))
            {
                var result = CreateResponse(StatusSuccess, $"User with id {userId} has been deleted.", new { userId });
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

            return Ok(CreateResponse(StatusSuccess, users));
        }

        [HttpGet("Exists/{userId}", Name = "IsUserExistByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("userId")]
        public async Task<IActionResult> IsUserExistByUserId(int userId)
        {


            var exists = await clsUser.IsUserExistByUserID(userId);
            return Ok(CreateResponse(StatusSuccess, exists));
        }

        [HttpGet("Exists/Person/{personId}", Name = "IsUserExistByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("personId")]
        public async Task<IActionResult> IsUserExistByPersonId(int personId)
        {

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
        [ValidateId("userId")]
        public async Task<IActionResult> IsUserActive(int userId)
        {

            var isActive = await clsUser.IsUserActive(userId);
            return Ok(CreateResponse(StatusSuccess, isActive));
        }

        [HttpGet("Unique/Username/{Username}", Name = "IsUsernameUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUsernameUnique(string Username, [FromQuery] int? Id)
        {
            if (string.IsNullOrEmpty(Username))
            {
                return BadRequest(CreateResponse(StatusFail, "Username is not valid"));
            }

            var unique = await clsUser.IsUsernameUnique(Username, Id);
            return Ok(CreateResponse(StatusSuccess, unique));
        }
    }
}
