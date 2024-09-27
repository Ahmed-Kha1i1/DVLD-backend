using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer.Tests.Test_Appointment;
using DataLayerCore.TestAppointment;
using DataLayerCore.TestType;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;
namespace DVLDApi.Controllers
{
    [Route("api/TestAppointments")]
    [ApiController]
    public class TestAppointmentsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public TestAppointmentsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{testAppointmentId}", Name = "GetTestAppointmentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testAppointmentId")]
        public async Task<IActionResult> GetTestAppointment(int testAppointmentId)
        {
            var testAppointment = await clsTestAppointment.Find(testAppointmentId);

            if (testAppointment is null)
            {
                return NotFound(CreateResponse(StatusFail, "Test appointment not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<TestAppointmentFullDTO>(testAppointment)));
        }

        [HttpPost(Name = "AddTestAppointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTestAppointment(TestAppointmentForCreateDTO testAppointmentDTO)
        {
            if (testAppointmentDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Test appointment object can't be null"));
            }

            var testAppointment = _mapper.Map<clsTestAppointment>(testAppointmentDTO);

            if (await testAppointment.Save())
            {
                return CreatedAtRoute("GetTestAppointmentById", new { testAppointmentId = testAppointment.TestAppointmentID }, CreateResponse(StatusSuccess, testAppointment));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding test appointment"));
            }
        }

        [HttpPut("{testAppointmentId}", Name = "UpdateTestAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("testAppointmentId")]
        public async Task<IActionResult> UpdateTestAppointment(int testAppointmentId, TestAppointmentForUpdateDTO testAppointmentDTO)
        {


            if (testAppointmentDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Test appointment object can't be null"));
            }

            var testAppointment = await clsTestAppointment.Find(testAppointmentId);
            if (testAppointment == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Test appointment with ID {testAppointmentId} not found."));
            }

            _mapper.Map(testAppointmentDTO, testAppointment);

            if (await testAppointment.Save())
            {
                return Ok(CreateResponse(StatusSuccess, testAppointment));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating test appointment"));
            }
        }

        [HttpGet("All/{testTypeId}/{localApplicationId}", Name = "GetAllTestAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testTypeId", "localApplicationId")]
        public async Task<IActionResult> GetAllTestAppointments(enTestType testTypeId, int localApplicationId)
        {

            var testAppointments = await clsTestAppointment.GetAllTestAppointments(testTypeId, localApplicationId);


            return Ok(CreateResponse(StatusSuccess, new { length = testAppointments.Count, data = testAppointments }));
        }

        [HttpDelete("{testAppointmentId}", Name = "DeleteTestAppointment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testAppointmentId")]
        public async Task<IActionResult> DeleteTestAppointment(int testAppointmentId)
        {


            var deleted = await clsTestAppointment.DeleteTestAppointment(testAppointmentId);
            if (!deleted)
            {
                return NotFound(CreateResponse(StatusFail, $"Test appointment with ID {testAppointmentId} not found."));
            }

            return NoContent();
        }

        [HttpGet("Last/{localApplicationId}/{testTypeId}", Name = "GetLastTestAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testTypeId", "localApplicationId")]
        public async Task<IActionResult> GetLastTestAppointment(int localDrivingLicenseApplicationId, enTestType testTypeId)
        {

            var testAppointment = await clsTestAppointment.FindLastTestAppointment(localDrivingLicenseApplicationId, testTypeId);
            if (testAppointment is null)
            {
                return NotFound(CreateResponse(StatusFail, "No last test appointment found for this application and test type!"));
            }

            return Ok(CreateResponse(StatusSuccess, testAppointment));
        }

        [HttpPut("{testAppointmentId}/Lock", Name = "LockTestAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testAppointmentId")]
        public async Task<IActionResult> LockTestAppointment(int testAppointmentId)
        {

            if (await clsTestAppointment.LockAppointment(testAppointmentId))
            {
                return Ok(CreateResponse(StatusSuccess, "Test appointment locked successfully"));
            }
            else
            {
                return NotFound(CreateResponse(StatusFail, "Fail to lock this test appointment"));
            }

        }
    }
}
