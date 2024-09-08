using AutoMapper;
using BusinessLayer.LicenseClass;
using DataLayerCore.LicenseClass;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/LicenseClasses")]
    [ApiController]
    public class LicenseClassesController : ControllerBase
    {
        private readonly IMapper _mapper;
        public LicenseClassesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{licenseClassId:int}", Name = "GetLicenseClassById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLicenseClass(int licenseClassId)
        {
            if (licenseClassId < 1)
            {
                return BadRequest(CreateResponse(StatusFail, "Invalid license class id"));
            }

            var licenseClass = await clsLicenseClass.Find(licenseClassId);

            if (licenseClass is null)
            {
                return NotFound(CreateResponse(StatusFail, "License class not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<LicenseClassDTO>(licenseClass)));
        }

        [HttpGet("{ClassName:regex(^[[a-zA-Z ]]*$)}", Name = "GetLicenseClassByApplicationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLicenseClassByApplicationId(string ClassName)
        {
            if (string.IsNullOrEmpty(ClassName))
            {
                return BadRequest(CreateResponse(StatusFail, "Class name con't be empty"));
            }
            var licenseClass = await clsLicenseClass.Find(ClassName);

            if (licenseClass is null)
            {
                return NotFound(CreateResponse(StatusFail, "LicenseClass not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<LicenseClassDTO>(licenseClass)));
        }

        [HttpGet("All", Name = "ClassName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllLicenseClasss()
        {
            var licenseClasss = await clsLicenseClass.GetAllLicenseClasses();
            if (licenseClasss.Count == 0)
            {
                return NotFound(CreateResponse(StatusFail, "No license classs found!"));
            }

            return Ok(CreateResponse(StatusSuccess, new { length = licenseClasss.Count, data = licenseClasss }));
        }
    }
}
