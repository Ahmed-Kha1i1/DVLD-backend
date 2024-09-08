using AutoMapper;
using BusinessLayer.Tests.TestTypes;
using DataLayerCore.TestType;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/TestTypes")]
    [ApiController]
    public class TestTypesController : ControllerBase
    {

        private readonly IMapper _mapper;

        public TestTypesController(IMapper mapper)
        {
            this._mapper = mapper;
        }
        [HttpGet("{TestTypeId}", Name = "GetTestTypeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTestType(enTestType TestTypeId)
        {
            if (TestTypeId == enTestType.None)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid test type"));
            }

            clsTestType? TestType = await clsTestType.Find(TestTypeId);

            if (TestType is null)
            {
                return NotFound(CreateResponse(StatusFail, "Test type not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<TestTypeDTO>(TestType)));
        }

        [HttpGet("All", Name = "GetAllTestTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTestTypes()
        {
            var TestTypesList = await clsTestType.GetTestTypes();

            if (TestTypesList.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No test types found!"));
            }

            var result = CreateResponse(StatusSuccess, new { length = TestTypesList.Count, data = TestTypesList });
            return Ok(result);
        }
    }
}
