using ApiLayer.Filters;
using AutoMapper;
using BusinessLayer.NewLocalLicesnse;
using DataLayerCore.LocalDrivingLicenseApplication;
using DataLayerCore.Test;
using DataLayerCore.TestType;
using Microsoft.AspNetCore.Mvc;
using static BusinessLayer.NewLocalLicesnse.clsLocalDrivingLicenseApplication;
using static DVLDApi.Helpers.ApiResponse;

namespace DVLDApi.Controllers
{
    [Route("api/LocalApplications")]
    [ApiController]
    public class LocalApplicationsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public LocalApplicationsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetLocalApplicationById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> GetLocalApplication(int id)
        {


            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, "local Application not found"));
            }
            else
            {
                return Ok(CreateResponse(StatusSuccess, _mapper.Map<LocalDrivingLicenseApplicationDTO>(application)));

            }
        }

        [HttpGet("ByApplicationID/{id}", Name = "GetLocalApplicationByApplicationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> GetLocalApplicationByApplicationID(int id)
        {


            var application = await clsLocalDrivingLicenseApplication.FindByApplicationID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, "local Application not found"));
            }
            else
            {
                return Ok(CreateResponse(StatusSuccess, _mapper.Map<LocalDrivingLicenseApplicationDTO>(application)));

            }
        }

        [HttpPost(Name = "AddLocalDrivingLicenseApplication")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationForCreateDTO applicationDTO)
        {
            if (applicationDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Application object can't be null"));
            }

            var application = _mapper.Map<clsLocalDrivingLicenseApplication>(applicationDTO);
            if (await application.Save())
            {
                return CreatedAtRoute("GetLocalDrivingLicenseApplicationById", new { id = application.LocalDrivingLicenseApplicationID }, CreateResponse(StatusSuccess, application));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding application"));
        }

        [HttpPut("{id}", Name = "UpdateLocalDrivingLicenseApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("id")]
        public async Task<IActionResult> UpdateLocalDrivingLicenseApplication(int id, LocalDrivingLicenseApplicationForUpdateDTO applicationDTO)
        {
            if (applicationDTO == null)
            {
                return BadRequest(CreateResponse(StatusFail, "Application object can't be null"));
            }

            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            _mapper.Map(applicationDTO, application);
            if (await application.Save())
            {
                return Ok(CreateResponse(StatusSuccess, application));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating application"));
        }

        [HttpDelete("{id}", Name = "DeleteLocalDrivingLicenseApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ValidateId("id")]
        public async Task<IActionResult> DeleteLocalDrivingLicenseApplication(int id)
        {


            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            if (await application.Delete())
            {
                return Ok(CreateResponse(StatusSuccess, $"Application with ID {id} has been deleted."));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error deleting application"));
        }

        [HttpGet("All", Name = "GetAllLocalDrivingLicenseApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllLocalDrivingLicenseApplications()
        {
            var applications = await clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplications();


            return Ok(CreateResponse(StatusSuccess, applications));
        }
        [HttpGet("{id}/tests/{testTypeId}/pass", Name = "DoesPassTestType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id", "testTypeId")]
        public async Task<IActionResult> DoesPassTestType(int id, enTestType testTypeId)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }


            var result = await application.DoesPassTestType(testTypeId);


            return Ok(CreateResponse(StatusSuccess, result));
        }

        [HttpGet("{id}/tests/{testTypeId}/passPrevies", Name = "DoesPassPreviousTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id", "testTypeId")]
        public async Task<IActionResult> DoesPassPreviousTest(int id, enTestType testTypeId)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            var result = await application.DoesPassPreviousTest(testTypeId);


            return Ok(CreateResponse(StatusSuccess, result));
        }

        [HttpGet("{id}/tests/{testTypeId}/attend", Name = "DoesAttendTestType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id", "testTypeId")]
        public async Task<IActionResult> DoesAttendTestType(int id, enTestType testTypeId)
        {

            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            bool result = await application.DoesAttendTestType(testTypeId);

            return Ok(CreateResponse(StatusSuccess, result));
        }

        [HttpGet("{id}/tests/{testTypeId}/trials", Name = "TotalTrialsPerTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id", "testTypeId")]
        public async Task<IActionResult> TotalTrialsPerTest(int id, enTestType testTypeId)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            byte result = await application.TotalTrialsPerTest(testTypeId);

            return Ok(CreateResponse(StatusSuccess, result));

        }


        [HttpGet("{id}/tests/{testTypeId}/active", Name = "IsThereAnActiveScheduledTes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id", "testTypeId")]
        public async Task<IActionResult> IsThereAnActiveScheduledTes(int id, enTestType testTypeId)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            bool result = await application.IsThereAnActiveScheduledTest(testTypeId);
            return Ok(CreateResponse(StatusSuccess, result));
        }
        [HttpGet("{id}/passCount", Name = "GetTestpassCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> GetTestpassCount(int id)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            var result = await application.GetTestpassCount();

            return Ok(CreateResponse(StatusSuccess, result));
        }
        [HttpGet("{id}/IsIssued", Name = "IsLicenseIssued")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> IsLicenseIssued(int id)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            bool result = await application.IsLicenseIssued();
            return Ok(CreateResponse(StatusSuccess, result));
        }


        [HttpGet("{id}/ActiveLicenseID", Name = "GetActiveLicenseID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> GetActiveLicenseID(int id)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            var result = await application.GetActiveLicenseID();

            return Ok(CreateResponse(StatusSuccess, result));
        }

        [HttpGet("{id}/tests/{testTypeId}/LastTest", Name = "GetLastTestPerTestType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id", "testTypeId")]
        public async Task<IActionResult> GetLastTestPerTestType(int id, enTestType TestTypeID)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            var Test = await application.GetLastTestPerTestType(TestTypeID);

            if (Test == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} has no test yet."));
            }

            return Ok(CreateResponse(StatusSuccess, _mapper.Map<TestDTO>(Test)));
        }

        [HttpGet("{id}/passAll", Name = "passedAllTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> passedAllTest(int id)
        {
            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            bool result = await application.passedAllTest();
            return Ok(CreateResponse(StatusSuccess, result));
        }


        [HttpPost("{id}/issue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateId("id")]
        public async Task<IActionResult> IssueLocalLicense(int id, IssueLocalLicenseDTO issueLocalLicenseDTO)
        {

            var application = await clsLocalDrivingLicenseApplication.FindByID(id);
            if (application == null)
            {
                return NotFound(CreateResponse(StatusFail, $"Application with ID {id} not found."));
            }

            var licenseId = await application.IssueLocalLicense(issueLocalLicenseDTO);
            if (licenseId != null)
            {
                return Ok(CreateResponse(StatusSuccess, new { LicenseID = licenseId }));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error issuing license"));
            }
        }

    }
}


