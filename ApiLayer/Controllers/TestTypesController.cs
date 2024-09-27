using ApiLayer.Filters;
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
        [HttpGet("{testTypeId}", Name = "GetTestTypeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("testTypeId")]
        public async Task<IActionResult> GetTestType(enTestType testTypeId)
        {
            if (testTypeId == enTestType.None)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid test type"));
            }

            clsTestType? TestType = await clsTestType.Find(testTypeId);

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



            var result = CreateResponse(StatusSuccess, new { length = TestTypesList.Count, data = TestTypesList });
            return Ok(result);
        }
    }
}
