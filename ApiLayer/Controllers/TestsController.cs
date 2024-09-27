using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer.Tests.Test;
using DataLayerCore.Test;
using DataLayerCore.TestType;
using Microsoft.AspNetCore.Mvc;
using static DataLayerCore.Test.clsTestData;
using static DVLDApi.Helpers.ApiResponse;
namespace DVLDApi.Controllers
{
    [Route("api/Tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public TestsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{testId}", Name = "GetTestById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testId")]
        public async Task<IActionResult> GetTest(int testId)
        {


            var test = await clsTest.Find(testId);
            if (test == null)
            {
                return NotFound(CreateResponse(StatusFail, "Test not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<TestDTO>(test)));
        }

        [HttpPost(Name = "AddTest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTest(TestForCreateDTO testDTO)
        {
            if (testDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Test object can't be null"));
            }

            var test = _mapper.Map<clsTest>(testDTO);

            if (await test.Save())
            {
                return CreatedAtRoute("GetTestById", new { testId = test.TestID }, CreateResponse(StatusSuccess, test));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding test"));
            }
        }

        [HttpPut("{testId}", Name = "UpdateTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("testId")]
        public async Task<IActionResult> UpdateTest(int testId, TestForUpdateDTO testDTO)
        {


            if (testDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Test object can't be null"));
            }

            var test = await clsTest.Find(testId);
            if (test == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Test with ID {testId} not found."));
            }

            _mapper.Map(testDTO, test);

            if (await test.Save())
            {
                return Ok(CreateResponse(StatusSuccess, test));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating test"));
            }
        }

        [HttpGet("All", Name = "GetAllTests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTests()
        {
            var tests = await clsTest.GetTests();

            return Ok(CreateResponse(StatusSuccess, tests));
        }



        [HttpGet("{testTypeID}/Last", Name = "GetLastTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("testTypeID")]
        public async Task<IActionResult> GetLastTest(enTestType testTypeID, LastTestDTO lastTestDTO)
        {
            var test = await clsTest.FindLastTestPerPersonAndLicenseClass(testTypeID, lastTestDTO);
            if (test == null)
            {
                return NotFound(CreateResponse(StatusFail, "No last test found for this person and test type!"));
            }

            return Ok(CreateResponse(StatusSuccess, test));
        }

        [HttpGet("LocalApplciation/{localApplicationId}/IsPassedAllTests", Name = "IsPassedAllTests")]
        [ValidateId("localApplicationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsPassedAllTests(int localApplicationId)
        {

            var allPassed = await clsTest.IsPassedAllTests(localApplicationId);
            return Ok(CreateResponse(StatusSuccess, allPassed));
        }
    }
}
