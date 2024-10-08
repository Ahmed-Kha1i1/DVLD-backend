using ApiLayer.DTOs.Person;
using ApiLayer.Filters;
using ApiLayer.Services;
using AutoMapper;
using BusinessLayer;
using DataLayerCore.Person;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopelController(IMapper _mapper, ImageService _imageService) : ControllerBase
    {

        [HttpGet("{personId:int}", Name = "GetPersonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("personId")]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var person = await clsPerson.Find(personId);
            if (person == null)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            var personDto = _mapper.Map<PersonFullDTO>(person);

            return Ok(CreateResponse(StatusSuccess, personDto));
        }

        [HttpGet("NationalNumber/{nationalNumber}", Name = "GetPersonByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersonByNationalNo([FromRoute(Name = "nationalNumber")] string NationalNo)
        {
            var person = await clsPerson.Find(NationalNo);
            if (person == null)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            var personDto = _mapper.Map<PersonFullDTO>(person);

            return Ok(CreateResponse(StatusSuccess, personDto));
        }

        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPerson([FromForm] PersonForCreateDTO? personDTO)
        {
            if (personDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Person object can't be null"));
            }

            var person = _mapper.Map<clsPerson>(personDTO);

            if (!await person.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding person"));
            }

            if (personDTO.ImageFile is null)
            {
                return CreatedAtRoute("GetPersonById", new { personId = person.PersonID }, CreateResponse(StatusSuccess, new { person.PersonID }));
            }

            string ImageName = await _imageService.UploadImageAsync(personDTO.ImageFile);

            if (string.IsNullOrEmpty(ImageName))
            {
                return StatusCode(StatusCodes.Status207MultiStatus,
                    CreateResponse(StatusPartialSuccess, "Person was added successfully, but image upload failed", new { person.PersonID }));
            }

            person.ImageName = ImageName;

            if (await person.Save())
            {
                return CreatedAtRoute("GetPersonById", new { personId = person.PersonID }, CreateResponse(StatusSuccess, new { person.PersonID }));
            }
            else
            {
                return StatusCode(StatusCodes.Status207MultiStatus,
                    CreateResponse(StatusPartialSuccess, "Person was added successfully, but fail updating person with image name", new { person.PersonID }));
            }

        }

        [HttpPut("{personId:int}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("personId")]
        public async Task<IActionResult> UpdatePerson(int personId, [FromForm] PersonForUpdateDTO personDTO)
        {

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
            (enUpdateImage Replace, string newIMagepath) = await _imageService.ReplaceImageAsync(personDTO.ImageFile, person.ImageName, personDTO.RemoveImage);

            if (Replace == enUpdateImage.UpdatedFail)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Image upload failed"));
            }

            if (Replace == enUpdateImage.DeletedFail)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Image delete failed"));
            }

            person.ImageName = Replace == enUpdateImage.Updated ? newIMagepath : Replace == enUpdateImage.DeletedWithoutUpdate ? null : person.ImageName;

            if (!await person.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating person"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<PersonFullDTO>(person)));
        }

        [HttpPut("UpdateContact/{personId:int}", Name = "UpdateContactPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("personId")]
        public async Task<IActionResult> UpdateContactPerson(int personId, ContactPersonDTO personDTO)
        {
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
                return Ok(CreateResponse(StatusSuccess, "Person was updated successfully", _mapper.Map<PersonFullDTO>(person)));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating person"));
            }
        }
        [HttpDelete("{personId:int}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("personId")]
        public async Task<IActionResult> DeletePerson(int personId)
        {


            var personExists = await clsPerson.IsPersonExists(personId);

            if (!personExists)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            if (await clsPerson.DeletePerson(personId))
            {
                var result = CreateResponse(StatusSuccess, $"Person with id {personId} has been deleted.", new { personId });
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

            return Ok(CreateResponse(StatusSuccess, persons));
        }

        [HttpGet("Unique/NationalNo/{nationalNo}", Name = "IsNationalNoUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsNationalNoUnique(string nationalNo, [FromQuery] int? Id)
        {
            if (string.IsNullOrEmpty(nationalNo))
            {
                return BadRequest(CreateResponse(StatusFail, "National number is not valid"));
            }

            var unique = await clsPerson.IsNationalNoUnique(nationalNo, Id);
            return Ok(CreateResponse(StatusSuccess, unique));
        }

        [HttpGet("Unique/Email/{email}", Name = "IsEmaiUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsEmaiUnique(string email, [FromQuery] int? Id)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(CreateResponse(StatusFail, "Email is not valid"));
            }

            var unique = await clsPerson.IsEmailUnique(email, Id);
            return Ok(CreateResponse(StatusSuccess, unique));
        }

        [HttpGet("Unique/Phone/{phone}", Name = "IsPhoneUnique")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsPhoneUnique(string phone, [FromQuery] int? Id)
        {

            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest(CreateResponse(StatusFail, "Phone is not valid"));
            }

            var unique = await clsPerson.IsPhoneUnique(phone, Id);
            return Ok(CreateResponse(StatusSuccess, unique));
        }


        [HttpGet("Exists/{personId:int}", Name = "IsPersonExistsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("personId")]
        public async Task<IActionResult> IsPersonExistsById(int personId)
        {
            var exists = await clsPerson.IsPersonExists(personId);
            return Ok(CreateResponse(StatusSuccess, exists));
        }
    }
}
