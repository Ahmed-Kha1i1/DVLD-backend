using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer.ApplicationsDescendants.Applications;
using DataLayerCore.Application;
using Microsoft.AspNetCore.Mvc;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/Applications")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ApplicationsController(IMapper mapper)
        {
            this._mapper = mapper;
        }

        [HttpGet("{ApplicationId}", Name = "GetApplicationById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateId("ApplicationId")]
        public async Task<IActionResult> GetApplication(int ApplicationId)
        {
            clsApplication? Application = await clsApplication.FindApplication(ApplicationId);

            if (Application is null)
            {
                return NotFound(CreateResponse(StatusFail, "Application not found"));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<ApplicationDTO>(Application)));
        }

        [HttpPost(Name = "AddApplication")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddApplication(ApplicationForCreateDTO ApplicationDTO)
        {
            if (ApplicationDTO == null)
            {
                return Ok(CreateResponse(StatusFail, "Application object cann't be null"));
            }

            var application = _mapper.Map<clsApplication>(ApplicationDTO);

            if (await application.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<ApplicationDTO>(application));
                return CreatedAtRoute("GetApplicationById", new { ApplicationId = application.ApplicationID }, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding application"));
            }

        }

        [HttpPut("{id}", Name = "UpdateApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("id")]
        public async Task<IActionResult> UpdateApplication(int id, ApplicationForUpdateDTO ApplicationDTO)
        {
            if (ApplicationDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Application object cann't be null"));
            }



            clsApplication? application = await clsApplication.FindApplication(id);

            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            _mapper.Map(ApplicationDTO, application);

            if (await application.Save())
            {
                var result = CreateResponse(StatusSuccess, _mapper.Map<ApplicationDTO>(application));
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating application"));
            }

        }


        [HttpDelete("{id}", Name = "DeleteApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> DeleteApplication(int id)
        {


            if (await clsApplication.DeleteApplication(id))
            {
                var result = CreateResponse(StatusSuccess, $"Application with ID {id} has been deleted.");
                return Ok(result);
            }
            else
            {
                var result = CreateResponse(StatusFail, $"Application with ID {id} not found. no rows deleted!");
                return NotFound(result);
            }
        }

        [HttpGet("All", Name = "GetAllApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllApplications()
        {
            var ApplicationsList = await clsApplication.GetApplications();


            var result = CreateResponse(StatusSuccess, ApplicationsList);
            return Ok(result);
        }
    }
}
