using AutoMapper;
using AutoMapper.Configuration.Conventions;
using BusinessLayer.License;
using DataLayerCore.Driver;
using DataLayerCore.License;
using Microsoft.AspNetCore.Http;  
using Microsoft.AspNetCore.Mvc;  
using System.Collections.Generic;  
using System.Threading.Tasks;  
using static DVLDApi.Helpers.ApiResponse;

[ApiController]
[Route("api/Licenses")]
public class LicensesController : ControllerBase
{
    private readonly IMapper _mapper;

    public LicensesController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("{licenseId}", Name = "GetLicenseById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLicense(int licenseId)
    {
        if (licenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }
        var license = await clsLicense.FindByLicenseID(licenseId);

        if (license is null)
        {
            return NotFound(CreateResponse(StatusFail, "License not found"));
        }

        return Ok(CreateResponse(StatusSuccess, _mapper.Map<LicenseInfoDTO>(license)));
    }

    [HttpGet("ByApplicationId/{ApplicationId}", Name = "GetLicenseByApplicationId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLicenseByApplicationId(int ApplicationId)
    {
        if (ApplicationId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid application id"));
        }
        var license = await clsLicense.FindByApplicationID(ApplicationId);

        if (license is null)
        {
            return NotFound(CreateResponse(StatusFail, "License not found"));
        }

        return Ok(CreateResponse(StatusSuccess, _mapper.Map<LicenseInfoDTO>(license)));
    }

    [HttpPost(Name = "AddLicense")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddLicense(LicenseInfoForCreateDTO licenseDTO)
    {
        if (licenseDTO == null)
        {
            return BadRequest(CreateResponse(StatusFail, "License object can't be null"));
        }

        var license = _mapper.Map<clsLicense>(licenseDTO);

        if (await license.Save())
        {
            return CreatedAtRoute("GetLicenseById", new { licenseId = license.LicenseID }, CreateResponse(StatusSuccess, license));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error adding license"));
        }
        
    }

    [HttpPut("{licenseId}", Name = "UpdateLicense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateLicense(int licenseId, LicenseInfoForUpdateDTO licenseDTO)
    {
        if (licenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }

        if (licenseDTO == null)
        {
            return BadRequest(CreateResponse(StatusFail, "License object can't be null"));
        }

        var license = await clsLicense.FindByLicenseID(licenseId);
        if (license == null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {licenseId} not found."));
        }

        _mapper.Map(licenseDTO, license);

        if (await license.Save())
        {
            return Ok(CreateResponse(StatusSuccess, license));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error updating license"));
        }
    }

    [HttpGet("All", Name = "GetAllLicenses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLicenses()
    {
        var licenses = await clsLicense.GetLicenses();
        if (licenses.Count == 0)
        {
            return NotFound(CreateResponse(StatusFail, "No licenses found!"));
        }

        return Ok(CreateResponse(StatusSuccess, new { length = licenses.Count, data = licenses }));
    }
    [HttpGet("Peopel/{PersonID}/LicenseClasses/{LicenseClassID}", Name = "GetActiveLicenseIDByPersonID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
    {
        if (PersonID < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid person id"));
        }

        if (LicenseClassID < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license class id"));
        }

        var LicenseId = await clsLicense.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        if (LicenseId is not null)
        {
            return Ok(CreateResponse(StatusSuccess, LicenseId));
        }
        else
        {
            return NotFound(CreateResponse(StatusFail, "No license found for this person with this license class!"));
        }
    }

    [HttpPost("Deactivate/{LicenseId}", Name = "DeactivateLicense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateLicense(int LicenseId)
    {
        if (LicenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }

        var license = await clsLicense.FindByLicenseID(LicenseId);
        if(license is null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {LicenseId} not found."));
        }

        if (await clsLicense.DeactivateLicense(LicenseId))
        {
            return Ok(CreateResponse(StatusSuccess, "License dactivated successfuly"));
        }
        else
        {
            return NotFound(CreateResponse(StatusFail, "Fail to dactivate this license"));
        }
    }

    [HttpGet("IsLicenseExpired/{LicenseId}", Name = "IsLicenseExpired")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsLicenseExpired(int LicenseId)
    {
        if (LicenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }

        var license = await clsLicense.FindByLicenseID(LicenseId);
        if (license is null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {LicenseId} not found."));
        }

        return Ok(CreateResponse(StatusSuccess, license.IsLicenseExpired()));
    }

    [HttpPut("{licenseId}/Renew", Name = "RenewLicense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> RenewLicense(int licenseId, RenewLicenseDTO renewLicenseDTO)
    {
        if (licenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }
        if (renewLicenseDTO == null)
        {
            return BadRequest(CreateResponse(StatusFail, "Renew license object can't be null"));
        }

        var license = await clsLicense.FindByLicenseID(licenseId);
        if (license is null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {licenseId} not found."));
        }

        var newLicense = await license.RenewLicense(renewLicenseDTO);
        if (newLicense is not null)
        {
            var result = CreateResponse(StatusSuccess, _mapper.Map<LicenseInfoDTO>(newLicense));
            return CreatedAtRoute("GetDriverById", new { licenseId = newLicense.LicenseID }, result);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error renewing license"));
        }
    }
    [HttpPut("{licenseId}/Replace", Name = "ReplaceFor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> ReplaceFor(int licenseId, ReplaceLicenseDTO replaceLicenseDTO)
    {
        if (licenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }
        if (replaceLicenseDTO == null)
        {
            return BadRequest(CreateResponse(StatusFail, "Replace license object can't be null"));
        }
        var license = await clsLicense.FindByLicenseID(licenseId);
        if (license is null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {licenseId} not found."));
        }

        var newLicense = await license.ReplaceFor(replaceLicenseDTO);
        if (newLicense is not null)
        {
            var result = CreateResponse(StatusSuccess, _mapper.Map<LicenseInfoDTO>(newLicense));
            return CreatedAtRoute("GetDriverById", new { licenseId = newLicense.LicenseID }, result);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error replacing license"));
        }
    }
    [HttpPost("{licenseId}/Detain", Name = "DetainLicense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> DetainLicense(int licenseId, DetainLicenseDTO detainLicenseDTO)
    {
        if (licenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }
        if (detainLicenseDTO == null)
        {
            return BadRequest(CreateResponse(StatusFail, "Detain license object can't be null"));
        }
        var license = await clsLicense.FindByLicenseID(licenseId);

        if (license == null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {licenseId} not found."));
        }

        var detainId = await license.Detain(detainLicenseDTO);
        if (detainId != null)
        {
            return Ok(CreateResponse(StatusSuccess, new { DetainID = detainId }));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error detaining license"));
        }
    }

    [HttpPost("{licenseId}/Users/{ReleasedByUserID}/Release")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReleaseDetainedLicense(int licenseId,int ReleasedByUserID)
    {
        if (licenseId < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid license id"));
        }
        if (ReleasedByUserID < 1)
        {
            return BadRequest(CreateResponse(StatusFail, "Invalid user id"));
        }

        var license = await clsLicense.FindByLicenseID(licenseId);

        if (license == null)
        {
            return NotFound(CreateResponse(StatusFail, $"License with ID {licenseId} not found."));
        }

        var applicationId = await license.ReleaseDetainedLicense(ReleasedByUserID);
        if (applicationId != null)
        {
            return Ok(CreateResponse(StatusSuccess, new { ApplicationID = applicationId }));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, CreateResponse(StatusError, "Error releasing detained license"));

        }
    }
}
