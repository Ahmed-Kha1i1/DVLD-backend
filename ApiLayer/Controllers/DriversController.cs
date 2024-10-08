using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer;
using DataLayerCore.Driver;
using DataLayerCore.Person;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/Drivers")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IMapper _mapper;

        public DriverController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("person/{DriverID}", Name = "GetDriverById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("DriverID")]

        public async Task<IActionResult> GetDriver(int DriverID)
        {

            clsDriver? driver = await clsDriver.FindByDriverID(DriverID);

            if (driver is null)
            {
                return NotFound(CreateResponse(StatusFail, "Driver not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<DriverFullDTO>(driver)));
        }

        [HttpGet("ByPersonId/{_personId}", Name = "GetDriverByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("_personId")]

        public async Task<IActionResult> GetDriverByPersonId(int PersonId)
        {


            clsDriver? driver = await clsDriver.FindByPersonID(PersonId);

            if (driver is null)
            {
                return NotFound(CreateResponse(StatusFail, "Driver not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<DriverDTO>(driver)));
        }

        [HttpGet("{driverId:int}", Name = "GetPersonByDriverId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("driverId")]
        public async Task<IActionResult> GetPerson(int driverId)
        {
            var person = await clsDriver.FindPerson(driverId);
            if (person == null)
            {
                return NotFound(CreateResponse(StatusFail, "Person not found"));
            }

            var personDto = _mapper.Map<PersonFullDTO>(person);

            return Ok(CreateResponse(StatusSuccess, personDto));
        }

        [HttpPost(Name = "AddDriver")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDriver(DriverForCreateDTO driverDTO)
        {
            if (driverDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Driver object cannot be null"));
            }

            var driver = _mapper.Map<clsDriver>(driverDTO);

            if (await driver.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<DriverDTO>(driver));
                return CreatedAtRoute("GetDriverById", new { DriverID = driver.DriverID }, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding driver"));
            }
        }

        [HttpPut("{id}", Name = "UpdateDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("id")]

        public async Task<IActionResult> UpdateDriver(int id, DriverForUpdateDTO driverDTO)
        {
            if (driverDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Driver object cannot be null"));
            }



            clsDriver? driver = await clsDriver.FindByDriverID(id);

            if (driver == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Driver with ID {id} not found."));
            }

            _mapper.Map(driverDTO, driver);

            if (await driver.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<DriverDTO>(driver));
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating driver"));
            }
        }


        [HttpGet("All", Name = "GetAllDrivers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDrivers()
        {
            var driversList = await clsDriver.GetAllDrivers();



            var result = CreateResponse(StatusSuccess, driversList);

            return Ok(result);
        }

        [HttpGet("{DriverID}/Licenses", Name = "GetDriverLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("DriverID")]

        public async Task<IActionResult> GetDriverLicenses(int DriverID)
        {

            var licenses = await clsDriver.GetLicenses(DriverID);


            var result = CreateResponse(StatusSuccess, licenses);
            return Ok(result);
        }

        [HttpGet("{DriverID}/InternationalLicenses", Name = "GetDriverInternationalLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("DriverID")]

        public async Task<IActionResult> GetDriverInternationalLicenses(int DriverID)
        {


            var internationalLicenses = await clsDriver.GetInternationalLicenses(DriverID);


            var result = CreateResponse(StatusSuccess, internationalLicenses);
            return Ok(result);
        }
    }

}
