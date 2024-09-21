using AutoMapper;
using BusinessLayer;
using DataLayerCore.Driver;
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

        [HttpGet("{DriverID}", Name = "GetDriverById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDriver(int DriverID)
        {
            if (DriverID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid driver ID"));
            }

            clsDriver? driver = await clsDriver.FindByDriverID(DriverID);

            if (driver is null)
            {
                return NotFound(CreateResponse(StatusFail, "Driver not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<DriverFullDTO>(driver)));
        }

        [HttpGet("ByPersonId/{PersonId}", Name = "GetDriverByPersonId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDriverByPersonId(int PersonId)
        {
            if (PersonId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid perosn ID"));
            }

            clsDriver? driver = await clsDriver.FindByPersonID(PersonId);

            if (driver is null)
            {
                return NotFound(CreateResponse(StatusFail, "Driver not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<DriverDTO>(driver)));
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
        public async Task<IActionResult> UpdateDriver(int id, DriverForUpdateDTO driverDTO)
        {
            if (driverDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Driver object cannot be null"));
            }

            if (id < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid driver ID"));
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

            if (driversList.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No drivers found!"));
            }

            var result = CreateResponse(StatusSuccess, new { length = driversList.Count, data = driversList });

            return Ok(result);
        }

        [HttpGet("{DriverID}/Licenses", Name = "GetDriverLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDriverLicenses(int DriverID)
        {
            if(DriverID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid driver ID"));
            }
            var licenses = await clsDriver.GetLicenses(DriverID);

            if (licenses.Count == 0)
            {
                return Ok(CreateResponse(StatusSuccess, "No licenses found for this driver"));
            }

            var result = CreateResponse(StatusSuccess, new { length = licenses.Count, data = licenses });
            return Ok(result);
        }

        [HttpGet("{DriverID}/InternationalLicenses", Name = "GetDriverInternationalLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDriverInternationalLicenses(int DriverID)
        {
            if (DriverID < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid driver ID"));
            }

            var internationalLicenses = await clsDriver.GetInternationalLicenses(DriverID);

            if (internationalLicenses.Count == 0)
            {
                return Ok(CreateResponse(StatusSuccess, "No international licenses found for this driver"));
            }

            var result = CreateResponse(StatusSuccess, new { length = internationalLicenses.Count, data = internationalLicenses });
            return Ok(result);
        }
    }

}
