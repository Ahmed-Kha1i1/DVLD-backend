using AutoMapper;
using BusinessLayer;
using DataLayerCore.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopelController : ControllerBase
    {
        private readonly IMapper _mapper;
        
        public PeopelController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{personId}", Name = "GetPersonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPerson(int personId)
        {
            if (personId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid person id"));
            }

            var person = await clsPerson.Find(personId);
            if (person == null)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            var personDto = _mapper.Map<PersonFullDTO>(person);

            return Ok(CreateResponse(StatusSuccess, personDto));
        }

        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPerson(PersonForCreateDTO personDTO)
        {
            if (personDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Person object can't be null"));
            }

            var person = _mapper.Map<clsPerson>(personDTO);

            if (await person.Save())
            {
                return CreatedAtRoute("GetPersonById", new { personId = person.PersonID }, CreateResponse(StatusSuccess, person));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding person"));
            }
        }

        [HttpPut("{personId}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePerson(int personId, PersonForUpdateDTO personDTO)
        {
            if (personId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid person id"));
            }

            if (personDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Person object can't be null"));
            }

            var person = await clsPerson.Find(personId);
            if (person == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Person with ID {personId} not found."));
            }

            _mapper.Map(personDTO, person);

            if (await person.Save())
            {
                return Ok(CreateResponse(StatusSuccess, person));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating person"));
            }
        }

        [HttpDelete("{personId}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            if (personId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid person id"));
            }

            var personExists = await clsPerson.IsPersonExists(personId);

            if (!personExists)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            if (await clsPerson.DeletePerson(personId))
            {
                var result = CreateResponse(StatusSuccess, $"Person with id {personId} has been deleted.");
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict, CreateResponse(StatusFail, $"Cannot delete person with id {personId}"));
            }
        }

        

        [HttpGet("All", Name = "GetAllPersons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await clsPerson.GetPeopleDetails();
            if (persons.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No persons found!"));
            }
            return Ok(CreateResponse(StatusSuccess, new { length = persons.Count, data = persons } ));
        }

        [HttpGet("Exists/NationalNo/{nationalNo}", Name = "IsPersonExistsByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsPersonExistsByNationalNo(string nationalNo)
        {
            if (string.IsNullOrEmpty(nationalNo))
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid national number"));
            }

            var exists = await clsPerson.IsPersonExists(nationalNo);
            return Ok(CreateResponse(StatusSuccess, exists));
        }

        [HttpGet("Exists/{personId}", Name = "IsPersonExistsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsPersonExistsById(int personId)
        {
            if (personId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid person id"));
            }


            var exists = await clsPerson.IsPersonExists(personId);
            return Ok(CreateResponse(StatusSuccess, exists));
        }
    }
}
