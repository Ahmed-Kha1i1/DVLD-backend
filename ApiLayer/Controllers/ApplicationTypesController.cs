using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer.ApplicationTypes;
using DataLayerCore.ApplicationType;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/ApplicationTypes")]
    [ApiController]
    public class ApplicationTypesController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ApplicationTypesController(IMapper mapper)
        {
            this._mapper = mapper;
        }
        [HttpGet("{ApplicationTypeId}", Name = "GetApplicationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("ApplicationTypeId")]
        public async Task<ActionResult> GetApplicationType([Range(1, 7)] enApplicationType ApplicationTypeId)
        {
            clsApplicationType? ApplicationType = await clsApplicationType.Find(ApplicationTypeId);

            if (ApplicationType is null)
            {
                return NotFound(CreateResponse(StatusFail, "Application type not found"));
            }

            var ApplicationTypeDTO = _mapper.Map<ApplicationTypeDTO>(ApplicationType);

            return Ok(CreateResponse(StatusSuccess, ApplicationTypeDTO));
        }

        [HttpGet("All", Name = "GetAllApplicationTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ApplicationTypeDTO>>> GetAllApplicationTypes()
        {
            var ApplicationTypesList = await clsApplicationType.GetApplicationTypes();



            return Ok(CreateResponse(StatusSuccess, new { Length = ApplicationTypesList.Count, data = ApplicationTypesList }));
        }
    }
}
